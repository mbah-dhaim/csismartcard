using System;

namespace com.csi.smartcard
{
    /// <summary>
    /// Exception for errors that occur during communication with the Smart Card stack or the card itself.
    /// </summary>
    [Serializable]
    public class CardException : Exception
    {
        /// <summary>
        /// <see cref="Exception()"/>
        /// </summary>
        public CardException() : base("Can not connect") { }
        /// <summary>
        /// <see cref="Exception(string)"/>
        /// </summary>
        /// <param name="message"></param>
        public CardException(string message) : base(message) { }
        /// <summary>
        /// <see cref="Exception(string,Exception)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CardException(string message, Exception inner) : base(message, inner) { }
        /// <summary>        
        /// <see cref="Exception(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CardException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
