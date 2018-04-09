namespace Resin.SupervisorApi.Client
{
    using System;
    using System.Runtime.Serialization;

    public class SupervisorApiException : Exception
    {
        public SupervisorApiException()
        {
        }

        public SupervisorApiException(string message) : base(message)
        {
        }

        public SupervisorApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SupervisorApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}