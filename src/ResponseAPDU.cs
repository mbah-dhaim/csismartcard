using System;

namespace com.csi.smartcard
{
    /// <summary>
    /// A response APDU as defined in ISO/IEC 7816-4.
    /// </summary>
    public class ResponseAPDU
    {
        private byte[] bytes;

        private readonly int nr;

        private readonly int sw;

        private readonly int sw1;

        private readonly int sw2;

        private byte[] data;
        /// <summary>
        /// Constructs a ResponseAPDU from a byte array containing the complete APDU contents (conditional body and trailed).
        /// </summary>
        /// <param name="bytes"></param>
        public ResponseAPDU(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 2) throw new ArgumentException();
            this.bytes = bytes;
            sw1 = bytes[bytes.Length - 2];
            sw2 = bytes[bytes.Length - 1];
            sw = sw1 << 8 + sw2;
            if (bytes.Length > 2)
            {
                data = new byte[bytes.Length - 2];
                Array.Copy(bytes, data, data.Length);
                nr = data.Length;
            }
        }
        /// <summary>
        /// Returns a copy of the bytes in this APDU.
        /// </summary>
        /// <returns></returns>
        public byte[] getBytes() => (byte[])bytes.Clone();
        /// <summary>
        /// Returns a copy of the data bytes in the response body.
        /// </summary>
        /// <returns></returns>
        public byte[] getData() => null != data ? (byte[])data.Clone() : null;
        /// <summary>
        /// Returns the number of data bytes in the response body (Nr) or 0 if this APDU has no body.
        /// </summary>
        /// <returns></returns>
        public int getNr() => nr;
        /// <summary>
        /// Returns the value of the status bytes SW1 and SW2 as a single status word SW.
        /// </summary>
        /// <returns></returns>
        public int getSW() => sw;
        /// <summary>
        /// Returns the value of the status byte SW1 as a value between 0 and 255.
        /// </summary>
        /// <returns></returns>
        public int getSW1() => sw1;
        /// <summary>
        /// Returns the value of the status byte SW2 as a value between 0 and 255.
        /// </summary>
        /// <returns></returns>
        public int getSW2() => sw2;
    }
}
