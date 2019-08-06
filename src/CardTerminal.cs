using System;
using System.Reflection;
using System.Threading.Tasks;

namespace com.csi.smartcard
{
    public class CardTerminal
    {

        private object lockObject = new object();

        private bool requestDestroy;

        private string name;

        internal SCardState State { get; set; }

        internal ATR atr { get; private set; }

        protected CardTerminal() => State = SCardState.UNAWARE;

        internal static CardTerminal of(string name) => new CardTerminal().setName(name);

        internal CardTerminal setName(string name)
        {
            this.name = name; return this;
        }

        public string getName() => name;

        public T connect<T>(SCardProtocolIdentifiers protocol = SCardProtocolIdentifiers.Default) where T : Card
        {
            if (!TerminalFactory.getDefault().isEstablished())
                throw new ApplicationException("Smartcard Service not found or started");
            uint rv = uint.MaxValue;
            uint pProtocol = uint.MinValue;
            IntPtr hCard = IntPtr.Zero;
            if (protocol == SCardProtocolIdentifiers.Default)
            {
                rv = NativeMethods.SCardConnect(TerminalFactory.getDefault().getHandle(), getName(), (uint)SCardShare.SHARED, (uint)(SCardProtocolIdentifiers.T0 | SCardProtocolIdentifiers.T1), out hCard, out pProtocol);
            }
            else
            {
                rv = NativeMethods.SCardConnect(TerminalFactory.getDefault().getHandle(), getName(), (uint)SCardShare.SHARED, (uint)protocol, out hCard, out pProtocol);
            }
            if (rv != 0) throw new CardException();
            Type t = typeof(T);
            T result = (T)t.GetConstructor(BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance, null, new Type[0], null).Invoke(new object[0]);
            result.setHandle(hCard).setProtocol((SCardProtocolIdentifiers)pProtocol).setATR(atr);
            return result;
        }

        public bool isCardPresent()
        {
            if ((State & SCardState.PRESENT) == SCardState.PRESENT || (State & SCardState.ATRMATCH) == SCardState.ATRMATCH)
            {
                return true;
            }
            atr = null;
            NativeMethods.SCARD_READERSTATE[] rgReaderStates = new NativeMethods.SCARD_READERSTATE[1];
            rgReaderStates[0].szReader = getName();
            rgReaderStates[0].dwCurrentState = (uint)State;
            if (NativeMethods.SCardGetStatusChange(TerminalFactory.getDefault().getHandle(), 100u, rgReaderStates, 1u) != 0u)
            {
                return false;
            }
            uint eventState = rgReaderStates[0].dwEventState;
            if ((eventState & (uint)SCardState.CHANGED) != (uint)SCardState.CHANGED)
            {
                //tidak ada perubahan, check current state
                if ((rgReaderStates[0].dwCurrentState & (uint)SCardState.PRESENT) == (uint)SCardState.PRESENT || (rgReaderStates[0].dwCurrentState & (uint)SCardState.ATRMATCH) == (uint)SCardState.ATRMATCH)
                {
                    State = (rgReaderStates[0].dwCurrentState & (uint)SCardState.PRESENT) == (uint)SCardState.PRESENT ? SCardState.PRESENT : SCardState.ATRMATCH;
                    byte[] bytes = new byte[rgReaderStates[0].cbAtr];
                    Array.Copy(rgReaderStates[0].rgbAtr, bytes, bytes.Length);
                    atr = new ATR(bytes);
                    return true;
                }
                return false;
            }
            if ((eventState & (uint)SCardState.PRESENT) == (uint)SCardState.PRESENT || (eventState & (uint)SCardState.ATRMATCH) == (uint)SCardState.ATRMATCH)
            {
                State = (eventState & (uint)SCardState.PRESENT) == (uint)SCardState.PRESENT ? SCardState.PRESENT : SCardState.ATRMATCH;
                byte[] bytes = new byte[rgReaderStates[0].cbAtr];
                Array.Copy(rgReaderStates[0].rgbAtr, bytes, bytes.Length);
                atr = new ATR(bytes);
                return true;
            }
            return false;
        }

        public bool waitForCardAbsent(uint timeout = 0)
        {
            if (!isCardPresent()) return true;
            requestDestroy = false;
            int procTimeout = timeout > 0 ? (int)timeout : int.MaxValue;
            Task<bool> t = Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                while (true)
                {
                    lock (lockObject)
                    {
                        if (!isCardPresent()) return true;
                        if (requestDestroy) return false;
                    }
                    TimeSpan end = DateTime.Now.Subtract(start);
                    if (end.Milliseconds >= procTimeout) break;
                }
                return false;
            });

            t.Wait(procTimeout);
            return !t.IsCompleted ? false : t.Result;
        }

        public bool waitForCardPresent(uint timeout = 0)
        {
            if (isCardPresent()) return true;
            requestDestroy = false;
            int procTimeout = timeout > 0 ? (int)timeout : int.MaxValue;
            Task<bool> t = Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                while (true)
                {
                    lock (lockObject)
                    {
                        if (isCardPresent()) return true;
                        if (requestDestroy) return false;
                    }
                    TimeSpan end = DateTime.Now.Subtract(start);
                    if (end.Milliseconds >= procTimeout) break;
                }
                return false;
            });
            t.Wait(procTimeout);
            return !t.IsCompleted ? false : t.Result;
        }

        internal void cleanUp() => requestDestroy = true;
    }
}
