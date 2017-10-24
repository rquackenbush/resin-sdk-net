using System;
using Newtonsoft.Json.Linq;
using Resin.Api.Client.Exceptions;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    public abstract class ODataObject : IDeferrableObject
    {
        private ApiClientBase _client;
        private JToken _token;

        protected ApiClientBase Client
        {
            get
            {
                CheckInitialized();

                return _client;
            }
        }

        protected void CheckInitialized()
        {
            if (_client == null)
                throw new NotInitializedException("Initialize has not been called yet.");
        }

        public void Initialize(ApiClientBase client, JToken token)
        {
            if (_client != null)
                throw new InvalidOperationException("Initialize has already been called.");

            if (client == null) throw new ArgumentNullException(nameof(client));
            if (token == null) throw new ArgumentNullException(nameof(token));

            _client = client;
            _token = token;

            Initialize();
        }

        protected virtual void Initialize()
        {
            
        }

        protected JToken Token
        {
            get
            {
                CheckInitialized();

                return _token;
            }
        }

        protected T GetValue<T>(string propertyName)
        {
            return Token[propertyName].Value<T>();
        }
    }
}