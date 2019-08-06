using System;
using System.Runtime.InteropServices;
using System.Text;
namespace com.csi.smartcard
{
    internal sealed class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public class SCARD_IO_REQUEST
        {
            public uint dwProtocol;
            public uint cbPciLength;
            public SCARD_IO_REQUEST()
            {
                dwProtocol = 0u;
                cbPciLength = 0u;
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SCARD_READERSTATE
        {
            public string szReader;
            public IntPtr pvUserData;
            public uint dwCurrentState;
            public uint dwEventState;
            public uint cbAtr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.U1)]
            public byte[] rgbAtr;
        }
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardBeginTransaction(IntPtr hCard);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardCancel(IntPtr hContext);
        [DllImport("WINSCARD.DLL", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern uint SCardConnect(IntPtr hContext, string szReader, uint dwShareMode, uint dwPreferredProtocols, out IntPtr phCard, out uint pdwActiveProtocol);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardControl(IntPtr hCard, uint dwControlCode, [In] byte[] lpInBuffer, uint nInBufferSize, [In] [Out] byte[] lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardDisconnect(IntPtr hCard, uint dwDisposition);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardEndTransaction(IntPtr hCard, uint dwDisposition);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardFreeMemory(IntPtr hContext, IntPtr pvMem);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardGetAttrib(IntPtr hCard, uint dwAttrId, ref IntPtr pbAttr, ref uint pcbAttrLen);
        [DllImport("WINSCARD.DLL", CharSet = CharSet.Auto)]
        internal static extern uint SCardGetStatusChange(IntPtr hContext, uint dwTimeout, [In] [Out] SCARD_READERSTATE[] rgReaderStates, uint cReaders);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardIsValidContext(IntPtr hContext);
        [DllImport("WINSCARD.DLL", CharSet = CharSet.Auto)]
        internal static extern uint SCardListReaderGroups(IntPtr hContext, ref IntPtr pmszGroups, ref uint pcchGroups);
        [DllImport("WINSCARD.DLL", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern uint SCardListReaders(IntPtr hContext, string mszGroups, ref IntPtr pmszReaders, ref uint pcchReaders);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardReconnect(IntPtr hCard, uint dwShareMode, uint dwPreferredProtocols, uint dwInitialization, out uint pdwActiveProtocol);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardReleaseContext(IntPtr hContext);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardSetAttrib(IntPtr hCard, uint dwAttrId, [In] byte[] pbAttr, uint cbAttrLen);
        [DllImport("WINSCARD.DLL", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern uint SCardStatus(IntPtr hCard, StringBuilder szReaderName, ref uint pcchReaderLen, out uint pdwState, out uint pdwProtocol, [Out] byte[] pbAtr, ref uint pcbAtrLen);
        [DllImport("WINSCARD.DLL")]
        internal static extern uint SCardTransmit(IntPtr hCard, IntPtr pioSendPci, [In] byte[] pbSendBuffer, uint cbSendLength, [In] [Out] SCARD_IO_REQUEST pioRecvPci, [In] [Out] byte[] pbRecvBuffer, ref uint pcbRecvLength);
        [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr LoadLibrary(string fileName);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr handle);
        [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr GetProcAddress(IntPtr handle, string procName);
        public static IntPtr GetPciT0()
        {
            IntPtr handle = LoadLibrary("Winscard.dll");
            IntPtr procAddress = GetProcAddress(handle, "g_rgSCardT0Pci");
            FreeLibrary(handle);
            return procAddress;
        }
        public static IntPtr GetPciT1()
        {
            IntPtr handle = LoadLibrary("Winscard.dll");
            IntPtr procAddress = GetProcAddress(handle, "g_rgSCardT1Pci");
            FreeLibrary(handle);
            return procAddress;
        }
        public static IntPtr GetPciRaw()
        {
            IntPtr handle = LoadLibrary("Winscard.dll");
            IntPtr procAddress = GetProcAddress(handle, "g_rgSCardRawPci");
            FreeLibrary(handle);
            return procAddress;
        }
    }
}
