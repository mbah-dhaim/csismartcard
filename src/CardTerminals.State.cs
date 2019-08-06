namespace com.csi.smartcard
{
    public partial class CardTerminals
    {
        public enum State
        {
            ALL,
            CARD_PRESENT,
            CARD_ABSENT,
            CARD_INSERTION,
            CARD_REMOVAL
        }
    }
}
