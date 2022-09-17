namespace TreeEdit
{
    using System;
    using System.Runtime.Serialization;

    public class InvalidCommandException : ApplicationException
    {
        public InvalidCommandException()
            : base()
        { }

        public InvalidCommandException(string message) 
            : base(message)
        { }

        public InvalidCommandException(string message, Exception innerException) 
            : base(message, innerException)
        { }

        protected InvalidCommandException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        { }
    }
}