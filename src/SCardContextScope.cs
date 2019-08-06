namespace com.csi.smartcard
{
    /// <summary>
    /// Context scope enumeration
    /// </summary>
    public enum SCardContextScope : uint
    {
        /// <summary>
        /// System scope
        /// </summary>
        System = 2,
        /// <summary>
        /// Terminal scope
        /// </summary>
        Terminal = 1,
        /// <summary>
        /// User scope
        /// </summary>
        User = 0
    }
}
