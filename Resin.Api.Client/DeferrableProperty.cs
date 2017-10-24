using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    internal class DeferrableProperty<T> : IDeferrableProperty<T>
        where T : class, IDeferrableObject, new()
        
    {
        private readonly ApiClientBase _client;
        
        private readonly string _deferredUri;

        private T _value;

        internal DeferrableProperty(ApiClientBase client, JToken token)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (token == null) throw new ArgumentNullException(nameof(token));

            _client = client;

            JToken deferred = token["__deferred"];

            if (deferred == null)
            {
                //We should have the real object
                _value = new T();

                _value.Initialize(client, token["d"]);
            }
            else
            {
                //This is a deffered object
                _deferredUri = deferred["uri"].Value<string>();
            }
        }

        public bool HasValue
        {
            get { return _value != null; }
        }

        public async Task<T> GetAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_value == null)
            {
                JToken token = await _client.GetAsync(_deferredUri, cancellationToken);

                _value = new T();

                _value.Initialize(_client, token["d"].Children().FirstOrDefault());
            }

            return _value;
        }
    }
}