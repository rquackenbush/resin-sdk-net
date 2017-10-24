using System;
using System.Runtime.Serialization;

namespace Resin.Api.Client.Exceptions
{
    public class ResinApiClientException : ApplicationException
    {
        public ResinApiClientException()
        {
        }

        public ResinApiClientException(string message) : base(message)
        {
        }

        public ResinApiClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResinApiClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}