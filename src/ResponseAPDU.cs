using System;

namespace com.csi.smartcard
{
    public class ResponseAPDU
    {
        private byte[] bytes;
        private int nr;
        private int sw;
        private int sw1;
        private int sw2;
        private byte[] data;
        public ResponseAPDU(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 2) throw new System.ArgumentException();
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

        public byte[] getBytes() => bytes;
        public byte[] getData() => data;
        public int getNr() => nr;
        public int getSW() => sw;
        public int getSW1() => sw1;
        public int getSW2() => sw2;
    }
}
