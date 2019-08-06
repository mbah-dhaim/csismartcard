using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace com.csi.smartcard
{
    /// <summary>
    /// The set of terminals supported by a TerminalFactory.
    /// </summary>
    public partial class CardTerminals
    {
        private object lockObject = new object();
        /// <summary>
        /// Internal list of <see cref="CardTerminal"/>
        /// </summary>
        protected List<CardTerminal> terminals;
        /// <summary>
        /// Current <see cref="CardTerminals.State"/>
        /// </summary>
        protected State state = State.ALL;

        private bool requestDestroy;
        /// <summary>
        /// Constructor
        /// </summary>
        protected CardTerminals() => terminals = new List<CardTerminal>();

        private static CardTerminals instance = null;

        internal static CardTerminals of()
        {
            instance = instance ?? new CardTerminals();
            return instance;
        }
        /// <summary>
        /// Get <see cref="CardTerminal"/> from internal list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CardTerminal getTerminal(string name) => terminals.Find(o => o.getName() == name);
        /// <summary>
        /// Get <see cref="CardTerminal"/> from internal list by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CardTerminal getTerminal(int index) => index < 0 || index >= terminals.Count ? null : terminals[index];
        /// <summary>
        /// Refresh internal list
        /// </summary>
        /// <returns></returns>
        protected List<CardTerminal> refreshList()
        {
            if (!TerminalFactory.getDefault().isEstablished())
                throw new ApplicationException("Smartcard Service not found or started");
            List<CardTerminal> result = new List<CardTerminal>();
            IntPtr zero = IntPtr.Zero;
            uint len = uint.MaxValue;
            uint rv = NativeMethods.SCardListReaders(TerminalFactory.getDefault().getHandle(), null, ref zero, ref len);
            List<string> readerList = new List<string>();
            if (rv != 0) return result;
            string msz = Marshal.PtrToStringAuto(zero, (int)len);
            int readerCount = splitMultiString(msz, readerList);
            NativeMethods.SCardFreeMemory(TerminalFactory.getDefault().getHandle(), zero);
            if (readerCount == 0) return result;
            readerList.ForEach(s => result.Add(CardTerminal.of(s)));
            return result;
        }
        /// <summary>
        /// Returns an unmodifiable list of all available terminals.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<CardTerminal> list(State state = State.ALL)
        {
            // TODO: check state each terminal to filter output here
            List<CardTerminal> result = new List<CardTerminal>();
            switch (state)
            {
                case State.ALL:
                    result = refreshList();
                    //add if not found
                    result.ForEach(item =>
                    {
                        CardTerminal terminal = terminals.Find(o => o.getName() == item.getName());
                        if (null == terminal) terminals.Add(item);
                    });
                    //remove if not found
                    int index = 0;
                    while (true)
                    {
                        CardTerminal terminal = result.Find(o => o.getName() == terminals[index].getName());
                        if (null == terminal)
                        {
                            terminals.RemoveAt(index);
                            if (index >= terminals.Count)
                            {
                                break;
                            }
                            continue;
                        }
                        index++;
                        if (index >= terminals.Count)
                        {
                            break;
                        }
                    }
                    result = terminals;
                    break;
                case State.CARD_PRESENT:
                case State.CARD_INSERTION:
                    result = terminals.FindAll(o => o.State == SCardState.PRESENT);
                    break;
                case State.CARD_ABSENT:
                case State.CARD_REMOVAL:
                    result = terminals.FindAll(o => o.State == SCardState.EMPTY || o.State == SCardState.UNAWARE);
                    break;
                default: break;
            }
            this.state = state;
            return result;
        }

        private StatusChangeContainer statusChangeContainer;
        /// <summary>
        /// Waits for card insertion or removal in any of the terminals of this object or until the timeout expires.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual bool waitForChange(uint timeout = 0u)
        {
            List<CardTerminal> result = new List<CardTerminal>();
            uint dwCurrentState = (uint)SCardState.UNAWARE;
            switch (state)
            {
                case State.ALL:
                    result = terminals;
                    break;
                case State.CARD_PRESENT:
                case State.CARD_INSERTION:
                    result = terminals.FindAll(o => o.State == SCardState.PRESENT);
                    dwCurrentState = (uint)SCardState.PRESENT;
                    break;
                case State.CARD_ABSENT:
                case State.CARD_REMOVAL:
                    result = terminals.FindAll(o => o.State == SCardState.EMPTY || o.State == SCardState.UNAWARE);
                    dwCurrentState = (uint)SCardState.EMPTY;
                    break;
                default: break;
            }
            if (result.Count == 0) return false;
            //untuk menghindari looping yang INFINITE, timeout minimal 100 miliseconds
            NativeMethods.SCARD_READERSTATE[] rgReaderStates = new NativeMethods.SCARD_READERSTATE[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                rgReaderStates[i].szReader = result[i].getName();
                rgReaderStates[i].dwCurrentState = dwCurrentState;
            }
            statusChangeContainer = new StatusChangeContainer();
            statusChangeContainer.Context = TerminalFactory.getDefault().getHandle();
            statusChangeContainer.ReaderStates = rgReaderStates;
            statusChangeContainer.Count = (uint)result.Count;
            statusChangeContainer.Timeout = timeout > 0 ? timeout : uint.MaxValue;
            statusChangeContainer.IsCompleted = false;

            Task t = Task.Run(() =>
            {
                uint count = 0u;
                IntPtr context = IntPtr.Zero;
                NativeMethods.SCARD_READERSTATE[] readerStates;
                lock (lockObject)
                {
                    context = statusChangeContainer.Context;
                    count = statusChangeContainer.Count;
                    readerStates = statusChangeContainer.ReaderStates;
                }
                bool isCompleted = false;
                DateTime start = DateTime.Now;
                while (true)
                {
                    lock (lockObject)
                    {
                        if (requestDestroy) break;
                    }
                    TimeSpan end;
                    if (NativeMethods.SCardGetStatusChange(context, 100u, readerStates, count) != 0u)
                    {
                        end = DateTime.Now.Subtract(start);
                        if (end.Milliseconds >= statusChangeContainer.Timeout) break;
                        continue;
                    }
                    bool changed = false;
                    foreach (var item in readerStates)
                    {
                        if ((item.dwEventState & (uint)SCardState.CHANGED) != (uint)SCardState.CHANGED) continue;
                        changed = true;
                        break;
                    }
                    if (changed)
                    {
                        isCompleted = true;
                        break;
                    }
                    end = DateTime.Now.Subtract(start);
                    if (end.Milliseconds >= statusChangeContainer.Timeout) break;
                }
                lock (lockObject)
                {
                    if (!isCompleted) return;
                    statusChangeContainer.IsCompleted = true;
                    //copy result;
                    statusChangeContainer.ReaderStates = readerStates;
                }
            });
            t.Wait((int)statusChangeContainer.Timeout);
            if (t.IsFaulted) return false;
            //update reader states
            for (int i = 0; i < statusChangeContainer.Count; i++)
            {
                CardTerminal terminal = getTerminal(statusChangeContainer.ReaderStates[i].szReader);
                if (null == terminal) continue;
                uint eventState = statusChangeContainer.ReaderStates[i].dwEventState;

                if ((eventState & (uint)SCardState.EMPTY) == (uint)SCardState.EMPTY)
                {
                    terminal.State = SCardState.EMPTY;
                }
                else if ((eventState & (uint)SCardState.PRESENT) == (uint)SCardState.PRESENT || (eventState & (uint)SCardState.ATRMATCH) == (uint)SCardState.ATRMATCH)
                {
                    terminal.State = SCardState.PRESENT;
                }
            }
            return !statusChangeContainer.IsCompleted ? false : true;
        }

        internal void cleanUp()
        {
            //cleans up child running thread
            terminals.ForEach(o =>
            {
                o.cleanUp();
            });
            //cleans up running thread
            requestDestroy = true;
        }

        private int splitMultiString(string msz, IList aList)
        {
            int num = 0;
            int num2 = 0;
            while (msz[num2] != '\0')
            {
                int num3 = num2;
                while (msz[num3] != '\0')
                {
                    num3++;
                }
                aList.Add(msz.Substring(num2, num3 - num2));
                num++;
                num2 = num3 + 1;
            }
            return num;
        }
    }
}
