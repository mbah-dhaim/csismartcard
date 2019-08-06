using System;

namespace com.csi.smartcard
{
    /// <summary>
    /// A command APDU following the structure defined in ISO/IEC 7816-4.
    /// </summary>
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
        /// <summary>
        /// Constructs a CommandAPDU from a byte array containing the complete APDU contents (header and body).
        /// </summary>
        /// <param name="apdu"></param>
        public CommandAPDU(byte[] apdu) => bytes = apdu;
        /// <summary>
        /// Constructs a CommandAPDU from a byte array containing the complete APDU contents (header and body).
        /// </summary>
        /// <param name="apdu"></param>
        /// <param name="apduOffset"></param>
        /// <param name="apduLength"></param>
        public CommandAPDU(byte[] apdu, int apduOffset, int apduLength)
        {
            bytes = new byte[apduLength];
            Array.Copy(apdu, apduOffset, bytes, 0, apduLength);
        }
        /// <summary>
        /// Constructs a CommandAPDU from the four header bytes.
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="ins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public CommandAPDU(int cla, int ins, int p1, int p2)
        {
            this.cla = cla;
            this.ins = ins;
            this.p1 = p1;
            this.p2 = p2;
        }
        /// <summary>
        /// Constructs a CommandAPDU from the four header bytes and command data.
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="ins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="data"></param>
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data) : this(cla, ins, p1, p2) => this.data = data;
        /// <summary>
        /// Constructs a CommandAPDU from the four header bytes, command data, and expected response data length.
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="ins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="data"></param>
        /// <param name="ne"></param>
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data, int ne) : this(cla, ins, p1, p2, data) => this.ne = ne;
        /// <summary>
        /// Constructs a CommandAPDU from the four header bytes and command data.
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="ins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <param name="dataLength"></param>
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data, int dataOffset, int dataLength) : this(cla, ins, p1, p2)
        {
            this.data = new byte[dataLength];
            Array.Copy(data, dataOffset, this.data, 0, dataLength);
        }
        /// <summary>
        /// Constructs a CommandAPDU from the four header bytes, command data, and expected response data length.
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="ins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <param name="dataLength"></param>
        /// <param name="ne"></param>
        public CommandAPDU(int cla, int ins, int p1, int p2, byte[] data, int dataOffset, int dataLength, int ne) : this(cla, ins, p1, p2, data, dataOffset, dataLength) => this.ne = ne;
        /// <summary>
        /// Constructs a CommandAPDU from the four header bytes and the expected response data length.
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="ins"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="ne"></param>
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
        /// <summary>
        /// Returns a copy of the bytes in this APDU.
        /// </summary>
        /// <returns></returns>
        public byte[] getBytes()
        {
            if (null == bytes || bytes.Length == 0)
            {
                buildBytes();
            }
            return bytes;
        }
        /// <summary>
        /// Returns the value of the class byte CLA.
        /// </summary>
        /// <returns></returns>
        public int getCLA() => cla;
        /// <summary>
        /// Returns a copy of the data bytes in the command body.
        /// </summary>
        /// <returns></returns>
        public byte[] getData() => data;
        /// <summary>
        /// Returns the value of the instruction byte INS.
        /// </summary>
        /// <returns></returns>
        public int getINS() => ins;
        /// <summary>
        /// Returns the number of data bytes in the command body (Nc) or 0 if this APDU has no body.
        /// </summary>
        /// <returns></returns>
        public int getNc() => (null == data || data.Length == 0) ? 0 : data.Length;
        /// <summary>
        /// Returns the maximum number of expected data bytes in a response APDU (Ne).
        /// </summary>
        /// <returns></returns>
        public int getNe() => ne;
        /// <summary>
        /// Returns the value of the parameter byte P1.
        /// </summary>
        /// <returns></returns>
        public int getP1() => p1;
        /// <summary>
        /// Returns the value of the parameter byte P2.
        /// </summary>
        /// <returns></returns>
        public int getP2() => p2;
    }
}

