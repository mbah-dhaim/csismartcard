namespace com.csi.smartcard
{
    public partial class CardTerminals
    {
        /// <summary>
        /// Enumeration of attributes of a CardTerminal.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// All CardTerminals.
            /// </summary>
            ALL,
            /// <summary>
            /// CardTerminals in which a card is present.
            /// </summary>
            CARD_PRESENT,
            /// <summary>
            /// CardTerminals in which a card is not present.
            /// </summary>
            CARD_ABSENT,
            /// <summary>
            /// CardTerminals for which a card insertion was detected during the latest call to waitForChange() call.
            /// </summary>
            CARD_INSERTION,
            /// <summary>
            /// CardTerminals for which a card removal was detected during the latest call to waitForChange() call.
            /// </summary>
            CARD_REMOVAL
        }
    }
}
