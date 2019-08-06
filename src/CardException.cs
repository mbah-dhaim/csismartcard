using System;

namespace com.csi.smartcard
{

    [Serializable]
    public class CardException : Exception
    {
        public CardException() : base("Can not connect") { }
        public CardException(string message) : base(message) { }
        public CardException(string message, Exception inner) : base(message, inner) { }
        protected CardException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
