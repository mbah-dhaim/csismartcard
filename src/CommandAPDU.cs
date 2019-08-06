using System;

namespace com.csi.smartcard
{
    public class CommandAPDU
    {
        private byte[] bytes;
        private int cla;
        private int ins;
        private int p1;
        private int p2;
        private int nc;
        private byte[] data;
        private int ne;
        public CommandAPDU(byte[] apdu) => bytes = apdu;
        public CommandAPDU(byte[] apdu, int apduOffset, int apduLength)
        {
            bytes = new byte[apduLength];
            Array.Copy(apdu, apduOffset, bytes, 0, apduLength);
        }

        public CommandAPDU(int cla, int ins, int p1, int p2)
        {
            this.cla = cla;
            this.ins = ins;
            this.p1 = p1;
            this.p2 = p2;
        }
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data) : this(cla, ins, p1, p2) => this.data = data;
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data, int ne) : this(cla, ins, p1, p2, data) => this.ne = ne;
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data, int dataOffset, int dataLength) : this(cla, ins, p1, p2)
        {
            this.data = new byte[dataLength];
            Array.Copy(data, dataOffset, this.data, 0, dataLength);
        }
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data, int dataOffset, int dataLength, int ne) : this(cla, ins, p1, p2, data, dataOffset, dataLength) => this.ne = ne;
        public CommandAPDU(int cla, int ins, int p1, int p2, int ne) : this(cla, ins, p1, p2) => this.ne = ne;

        private void buildBytes()
        {
            int length = ushort.MaxValue + 10;
            byte[] buffer = new byte[length];
            //case 1S
            buffer[0] = (byte)cla;
            buffer[1] = (byte)ins;
            buffer[2] = (byte)p1;
            buffer[3] = (byte)p2;
            length = 4;
            nc = (null == data || data.Length == 0) ? 0 : data.Length;
            if (ne > 0)
            {
                if (nc > 0)
                {
                    if (ne < 256 && nc < 256)
                    {
                        //case 4S
                        buffer[length] = (byte)nc;
                        length += 1;
                        Array.Copy(data, 0, buffer, length, nc);
                        length += nc;
                        buffer[length] = (byte)ne;
                        length += 1;
                    }
                    else
                    {
                        //case 4E
                        buffer[length + 1] = (byte)(nc >> 8);
                        buffer[length + 2] = (byte)(nc);
                        length += 3;
                        Array.Copy(data, 0, buffer, length, nc);
                        length += nc;
                        buffer[length + 1] = (byte)(ne >> 8);
                        buffer[length + 2] = (byte)(ne);
                        length += 3;
                    }
                }
                else
                {
                    if (ne < 256)
                    {
                        //case 2S
                        buffer[length] = (byte)ne;
                        length += 1;
                    }
                    else
                    {
                        //case 2E
                        buffer[length + 1] = (byte)(ne >> 8);
                        buffer[length + 2] = (byte)(ne);
                        length += 3;
                    }
                }
            }
            else if (nc > 0)
            {
                if (nc < 256)
                {
                    //case 3S
                    buffer[length] = (byte)nc;
                    length += 1;
                    Array.Copy(data, 0, buffer, length, nc);
                    length += nc;
                }
                else
                {
                    //case 3E
                    buffer[length + 1] = (byte)(nc >> 8);
                    buffer[length + 2] = (byte)(nc);
                    length += 3;
                    Array.Copy(data, 0, buffer, length, nc);
                    length += nc;
                }
            }
            bytes = new byte[length];
            Array.Copy(buffer, bytes, length);
        }

        public byte[] getBytes()
        {
            if (null == bytes || bytes.Length == 0)
            {
                buildBytes();
            }
            return bytes;
        }

        public int getCLA() => cla;

        public byte[] getData() => data;

        public int getINS() => ins;

        public int getNc() => (null == data || data.Length == 0) ? 0 : data.Length;

        public int getNe() => ne;

        public int getP1() => p1;

        public int getP2() => p2;
    }
}

