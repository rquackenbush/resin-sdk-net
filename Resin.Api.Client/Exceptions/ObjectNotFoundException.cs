﻿using System;
using System.Runtime.Serialization;

namespace Resin.Api.Client.Exceptions
{
    public class ObjectNotFoundException : ResinApiClientException
    {
        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message) : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}