using System;
using System.Runtime.Serialization;

namespace Resin.Api.Client.Exceptions
{
    public class NotInitializedException : ResinApiClientException
    {
        public NotInitializedException()
        {
        }

        public NotInitializedException(string message) : base(message)
        {
        }

        public NotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}