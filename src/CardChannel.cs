using System;

namespace com.csi.smartcard
{
    /// <summary>
    /// A logical channel connection to a Smart Card.
    /// </summary>
    public class CardChannel
    {
        private readonly IntPtr SCARD_PCI_T0;

        private readonly IntPtr SCARD_PCI_T1;

        private readonly IntPtr SCARD_PCI_RAW;

        private Card card;
        /// <summary>
        /// Constructor
        /// </summary>
        protected CardChannel()
        {
            SCARD_PCI_T0 = NativeMethods.GetPciT0();
            SCARD_PCI_T1 = NativeMethods.GetPciT1();
            SCARD_PCI_RAW = NativeMethods.GetPciRaw();
        }
        internal static CardChannel of() => new CardChannel();

        internal CardChannel setCard(Card card)
        {
            this.card = card;
            return this;
        }
        /// <summary>
        /// Close connection
        /// <see cref="Card.disconnect(bool)"/>
        /// </summary>
        public virtual void close() => card.disconnect();
        /// <summary>
        /// Transmits the command APDU stored in the command byte array and receives the reponse APDU in the response byte array
        /// </summary>
        /// <param name="command"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public virtual int transmit(byte[] command, out byte[] response)
        {
            response = new byte[0];
            uint length = ushort.MaxValue;
            byte[] buffer = new byte[length];
            uint rv = uint.MaxValue;
            switch (card.getProtocol())
            {
                case SCardProtocolIdentifiers.Raw:
                    rv = NativeMethods.SCardTransmit(card.getHandle(), SCARD_PCI_RAW, command, (uint)command.GetLength(0), null, buffer, ref length);
                    break;
                case SCardProtocolIdentifiers.T0:
                    rv = NativeMethods.SCardTransmit(card.getHandle(), SCARD_PCI_T0, command, (uint)command.GetLength(0), null, buffer, ref length);
                    break;
                case SCardProtocolIdentifiers.T1:
                    rv = NativeMethods.SCardTransmit(card.getHandle(), SCARD_PCI_T1, command, (uint)command.GetLength(0), null, buffer, ref length);
                    break;
                default:
                    break;
            }
            if (rv != 0u)
            {
                throw new CardException("Transmit failed");
            }
            response = new byte[length];
            Array.Copy(buffer, response, length);
            return (int)length;
        }
        /// <summary>
        /// Transmits the specified command APDU to the Smart Card and returns the response APDU.
        /// <see cref="CommandAPDU"/>
        /// <see cref="ResponseAPDU"/>
        /// </summary>
        /// <param name="apdu"></param>
        /// <returns></returns>
        public ResponseAPDU transmit(CommandAPDU apdu)
        {
            int length = transmit(apdu.getBytes(), out byte[] response);
            if (length > 0) return new ResponseAPDU(response);
            return null;
        }
    }
}
