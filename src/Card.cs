using System;

namespace com.csi.smartcard
{
    public class Card
    {
        private IntPtr handle;

        protected SCardProtocolIdentifiers protocol;

        protected ATR atr;

        protected Card() { }

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

        public ATR getATR() => atr;

        public void disconnect(bool reset = false) => NativeMethods.SCardDisconnect(handle, reset ? (uint)SCardDisposition.ResetCard : (uint)SCardDisposition.UnpowerCard);

        public CardChannel getBasicChannel() => CardChannel.of().setCard(this);

    }
}
