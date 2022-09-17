namespace Graphs
{
    using System;
    using System.Runtime.Serialization;

    public class CycleException : ApplicationException
    {
        public CycleException()
        {
        }

        public CycleException(string message) : base(message)
        {
        }

        public CycleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CycleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}