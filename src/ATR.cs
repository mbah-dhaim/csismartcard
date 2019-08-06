namespace com.csi.smartcard
{
    public class ATR
    {
        private readonly byte[] bytes;
        public ATR(byte[] bytes) => this.bytes = bytes;
        public byte[] getBytes() => bytes;
    }
}
