using com.csi.smartcard.Extensions;
using System;
using System.Text;

namespace com.csi.smartcard
{
    /// <summary>
    /// Helper Object
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Convert hexadecimal to byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToByteArray(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }
            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }
            int length = hex.Length / 2;
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = byte.Parse(hex.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return result;
        }
        /// <summary>
        /// Convert byte array to hexadecimal
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToHex(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                result.AppendFormat("{0:X2}", data[i]);
            }
            return result.ToString();
        }
        /// <summary>
        /// Convert hexadecimal to int
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToNumber(string hex) => int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        /// <summary>
        /// Convert integer to hexadecimal
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NumberToHex(int number, int length)
        {
            string result = number.ToString("X");
            return result.PadLeft(length, '0');
        }
        /// <summary>
        /// check if APDU response is valid
        /// </summary>
        /// <param name="rApdu"></param>
        /// <returns></returns>
        /// <remarks>deprecated</remarks>
        public static bool IsValidResponse(string rApdu)
        {
            if (rApdu.Substring(rApdu.Length - 4, 4) == "9000" || rApdu.Substring(rApdu.Length - 4, 2) == "61" || rApdu.Substring(rApdu.Length - 4, 2) == "6C")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get <see cref="short"/> value from byte array
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static short GetShort(byte[] data, int index) => (short)(data[index] * 256 + data[index + 1]);
        /// <summary>
        /// Set byte array with <see cref="short"/> value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <param name="index"></param>
        public static void SetShort(short value, byte[] data, int index)
        {
            data[index] = (byte)(value >> 8);
            data[index + 1] = (byte)(value);
        }
        /// <summary>
        /// Set byte array with <see cref="short"/> value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        public static void SetShort(short value, byte[] data) => SetShort(value, data, 0);
        /// <summary>
        /// Get <see cref="short"/> value from hexadecimal
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static short GetShort(string data, int index)
        {
            if (index % 2 != 0)
            {
                throw new InvalidOperationException("Index must even");
            }
            byte[] bData = HexToByteArray(data);
            index /= 2;
            return GetShort(bData, index);
        }
        /// <summary>
        /// Get <see cref="short"/> value from byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static short GetShort(byte[] data) => GetShort(data, 0);
        /// <summary>
        /// Get <see cref="short"/> value from byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static short GetShort(string data) => GetShort(data, 0);
        /// <summary>
        /// Custom convert byte array to <see cref="float"/>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static float BytesToFloat(byte[] data, int index)
        {
            byte[] tmp = new byte[4];
            int length = 4;
            if (data.Length - index < 4) length = data.Length - index;
            Array.Copy(data, index, tmp, 4 - length, length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(tmp);
            }
            return BitConverter.ToSingle(tmp, 0);
        }
        /// <summary>
        /// Custom convert hexadecimal to <see cref="float"/>
        /// </summary>
        /// <returns></returns>
        public static float HexToFloat(string hex)
        {
            byte[] data = HexToByteArray(hex);
            return BytesToFloat(data, 0);
        }
        /// <summary>
        /// Custom convert <see cref="float"/> to byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] FloatToBytes(float data)
        {
            byte[] result = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }
            return result;
        }
        /// <summary>
        /// Custom convert <see cref="float"/> to hexadecimal
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FloatToHex(float data) => ByteArrayToHex(FloatToBytes(data));
        /// <summary>
        /// Convert byte array to integer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int BytesToInt(byte[] data, int index)
        {
            byte[] tmp = new byte[4];
            int length = 4;
            if (data.Length - index < 4) length = data.Length - index;
            Array.Copy(data, index, tmp, 4 - length, length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(tmp);
            }
            return BitConverter.ToInt32(tmp, 0);
        }
        /// <summary>
        /// Convert hexadecimal to integer
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToInt(string hex)
        {
            byte[] data = HexToByteArray(hex);
            return BytesToInt(data, 0);
        }
        /// <summary>
        /// Convert <see cref="int"/> to byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] IntToBytes(int value)
        {
            byte[] result = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }
            return result;
        }
        /// <summary>
        /// Convert <see cref="int"/> to hexadecimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntToHex(int value)
        {
            byte[] result = IntToBytes(value);
            return ByteArrayToHex(result);
        }
        /// <summary>
        ///Convert byte array to <see cref="short"/>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static short BytesToShort(byte[] data, int index)
        {
            byte[] tmp = new byte[2];
            int length = 2;
            if (data.Length - index < 2) length = data.Length - index;
            Array.Copy(data, index, tmp, 2 - length, length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(tmp);
            }
            return BitConverter.ToInt16(tmp, 0);
        }
        /// <summary>
        /// Convert hexadecimal to <see cref="short"/>
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static short HexToShort(string hex) => BytesToShort(HexToByteArray(hex), 0);
        /// <summary>
        /// Convert  <see cref="short"/> to byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ShortToBytes(short value)
        {
            byte[] result = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }
            return result;
        }
        /// <summary>
        /// Convert  <see cref="short"/> to hexadecimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShortToHex(short value) => ByteArrayToHex(ShortToBytes(value));
        /// <summary>
        /// Convert byte array to hex with space
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PrettyHex(byte[] data)
        {
            if (null == data || data.Length == 0) return string.Empty;
            StringBuilder result = new StringBuilder();
            data.ForEach(b => result.AppendFormat("{0:X2} ", b));
            return result.ToString();
        }
    }
}
