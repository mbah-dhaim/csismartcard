using System;
namespace com.csi.smartcard
{
    /// <summary>
    /// Protocol Identifier enumeration
    /// </summary>
    [Flags]
    public enum SCardProtocolIdentifiers
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = -2147483648,
        /// <summary>
        /// Optimal or Undefined
        /// </summary>
        Optimal = 0,
        /// <summary>
        /// Raw
        /// </summary>
        Raw = 65536,
        /// <summary>
        /// T0
        /// </summary>
        T0 = 1,
        /// <summary>
        /// T1
        /// </summary>
        T1 = 2,
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0
    }
}
