using System;

namespace com.csi.smartcard
{
    public class TerminalFactory : IDisposable
    {
        protected bool disposed;

        private IntPtr handle = IntPtr.Zero;

        protected CardTerminals cardTerminals;

        protected TerminalFactory()
        {
            uint rv = NativeMethods.SCardEstablishContext((uint)SCardContextScope.System, IntPtr.Zero, IntPtr.Zero, out handle);
            if (rv != 0) throw new ApplicationException("Smartcard Service not found or started");
            cardTerminals = CardTerminals.of();
        }

        private static TerminalFactory instance = null;

        public static TerminalFactory getDefault()
        {
            instance = instance ?? new TerminalFactory();
            return instance;
        }

        public CardTerminals terminals() => cardTerminals;

        internal IntPtr getHandle() => handle;

        internal bool isEstablished() => handle != IntPtr.Zero;

        protected void release()
        {
            if (!isEstablished()) return;
            NativeMethods.SCardReleaseContext(handle);
            handle = IntPtr.Zero;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                cardTerminals.cleanUp();
                release();
            }
            disposed = true;
        }

        ~TerminalFactory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
