using System;

namespace com.csi.smartcard
{
    /// <summary>
    /// A factory for CardTerminal objects.
    /// </summary>
    public class TerminalFactory : IDisposable
    {
        /// <summary>
        /// disposed flag
        /// </summary>
        protected bool disposed;

        private IntPtr handle = IntPtr.Zero;
        /// <summary>
        /// <see cref="CardTerminals"/> object holder
        /// </summary>
        protected CardTerminals cardTerminals;
        /// <summary>
        /// Constructor
        /// </summary>
        protected TerminalFactory()
        {
            uint rv = NativeMethods.SCardEstablishContext((uint)SCardContextScope.System, IntPtr.Zero, IntPtr.Zero, out handle);
            if (rv != 0) throw new ApplicationException("Smartcard Service not found or started");
            cardTerminals = CardTerminals.of();
        }

        private static TerminalFactory instance = null;
        /// <summary>
        /// Returns the default TerminalFactory instance.
        /// </summary>
        /// <returns></returns>
        public static TerminalFactory getDefault()
        {
            instance = instance ?? new TerminalFactory();
            return instance;
        }
        /// <summary>
        /// Returns a new CardTerminals object encapsulating the terminals supported by this factory.
        /// </summary>
        /// <returns></returns>
        public CardTerminals terminals() => cardTerminals;

        internal IntPtr getHandle() => handle;

        internal bool isEstablished() => handle != IntPtr.Zero;
        /// <summary>
        /// Internal release from context
        /// </summary>
        protected void release()
        {
            if (!isEstablished()) return;
            NativeMethods.SCardReleaseContext(handle);
            handle = IntPtr.Zero;
        }
        /// <summary>
        /// Dispose object
        /// </summary>
        /// <param name="disposing"></param>
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
        /// <summary>
        /// Finalizer
        /// </summary>
        ~TerminalFactory()
        {
            Dispose(false);
        }
        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
