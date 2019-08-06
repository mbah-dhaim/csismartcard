namespace com.csi.smartcard
{
    internal enum SCardState : uint
    {
        /// <summary>
        /// Unware
        /// </summary>
        UNAWARE = 0x00000000,
        /// <summary>
        /// State Ignored
        /// </summary>
        IGNORE = 0x00000001,
        /// <summary>
        /// State changed
        /// </summary>
        CHANGED = 0x00000002,
        /// <summary>
        /// State Unknown
        /// </summary>
        UNKNOWN = 0x00000004,
        /// <summary>
        /// State Unavilable
        /// </summary>
        UNAVAILABLE = 0x00000008,
        /// <summary>
        /// State empty
        /// </summary>
        EMPTY = 0x00000010,
        /// <summary>
        /// Card Present
        /// </summary>
        PRESENT = 0x00000020,
        /// <summary>
        /// ATR mathched
        /// </summary>
        ATRMATCH = 0x00000040,
        /// <summary>
        /// In exclusive use
        /// </summary>
        EXCLUSIVE = 0x00000080,
        /// <summary>
        /// In use
        /// </summary>
        INUSE = 0x00000100,
        /// <summary>
        /// Mute State
        /// </summary>
        MUTE = 0x00000200,
        /// <summary>
        /// Card unpowered
        /// </summary>
        UNPOWERED = 0x00000400
    }
}
