using System;

namespace com.csi.smartcard
{
    internal class StatusChangeContainer
    {
        public IntPtr Context { get; set; }
        public NativeMethods.SCARD_READERSTATE[] ReaderStates { get; set; }        
        public uint Count { get; set; }
        public uint Timeout { get; set; }
        public bool IsCompleted { get; set; }
    }
}
