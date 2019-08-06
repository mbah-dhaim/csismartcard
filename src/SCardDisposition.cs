namespace com.csi.smartcard
{
    /// <summary>
    /// Disposition enumeration
    /// </summary>
    public enum SCardDisposition
    {
        /// <summary>
        /// Confiscate
        /// </summary>
        Confiscate = 4,
        /// <summary>
        /// Eject
        /// </summary>
        EjectCard = 3,
        /// <summary>
        /// Leave
        /// </summary>
        LeaveCard = 0,
        /// <summary>
        /// Reset card
        /// </summary>
        ResetCard,
        /// <summary>
        /// Unpower Card
        /// </summary>
        UnpowerCard
    }
}
