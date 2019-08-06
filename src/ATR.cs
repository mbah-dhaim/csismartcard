namespace com.csi.smartcard
{
    /// <summary>
    /// A Smart Card's answer-to-reset bytes.
    /// </summary>
    public class ATR
    {
        private readonly byte[] bytes;
        /// <summary>
        /// Constructs an ATR from a byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public ATR(byte[] bytes) => this.bytes = bytes;
        /// <summary>
        /// Get ATR byte array
        /// </summary>
        /// <returns></returns>
        public byte[] getBytes() => (byte[])bytes.Clone();
    }
}
