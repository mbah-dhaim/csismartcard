using System;

namespace com.csi.smartcard
{
    /// <summary>
    /// A Smart Card with which a connection has been established.
    /// </summary>
    public class Card
    {
        private IntPtr handle;
        /// <summary>
        /// Connection protocol
        /// <see cref="SCardProtocolIdentifiers"/>
        /// </summary>
        protected SCardProtocolIdentifiers protocol;
        /// <summary>
        /// Card ATR
        /// </summary>
        protected ATR atr;
        /// <summary>
        /// Constructor
        /// </summary>
        protected Card() { }
        /// <summary>
        /// Generic create instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static T of<T>() where T : Card, new() => new T();

        internal Card setHandle(IntPtr handle)
        {
            this.handle = handle;
            return this;
        }

        internal IntPtr getHandle() => handle;

        internal Card setProtocol(SCardProtocolIdentifiers protocol)
        {
            this.protocol = protocol;
            return this;
        }

        internal SCardProtocolIdentifiers getProtocol() => protocol;

        internal Card setATR(ATR atr)
        {
            this.atr = atr;
            return this;
        }
        /// <summary>
        /// Get Card ATR
        /// </summary>
        /// <returns></returns>
        public ATR getATR() => atr;
        /// <summary>
        /// Disconnect
        /// </summary>
        /// <param name="reset"></param>
        public void disconnect(bool reset = false) => NativeMethods.SCardDisconnect(handle, reset ? (uint)SCardDisposition.ResetCard : (uint)SCardDisposition.UnpowerCard);
        /// <summary>
        /// Get transmission channel
        /// </summary>
        /// <returns></returns>
        public CardChannel getBasicChannel() => CardChannel.of().setCard(this);
        /// <summary>
        /// Requests exclusive access to this card.
        /// </summary>
        public void beginExclusive()
        {
            uint rv = NativeMethods.SCardBeginTransaction(handle);
            if (rv != 0)
            {
                throw new CardException("Failed to get exclusive access");
            }
        }
        /// <summary>
        /// Releases the exclusive access previously established using beginExclusive.
        /// </summary>
        public void endExclusive()
        {
            uint rv = NativeMethods.SCardEndTransaction(handle, (uint)SCardDisposition.LeaveCard);
            if (rv != 0)
            {
                throw new CardException("Failed to stop exclusive access");
            }
        }
    }
}
