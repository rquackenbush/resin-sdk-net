using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    internal class DeferrableArrayProperty<T> : IDeferrableProperty<T[]>
        where T : class, IDeferrableObject, new()
    {
        private readonly ApiClientBase _client;
        
        private readonly string _deferredUri;

        private T[] _value;

        internal DeferrableArrayProperty(ApiClientBase client, JToken token)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _client = client;

            JToken deferred = token["__deferred"];

            if (deferred == null)
            {
                //We should have the real object
                _value = GetValue(token);
            }
            else
            {
                //This is a deffered object
                _deferredUri = deferred["uri"].Value<string>();
            }
        }

        private T[] GetValue(JToken token)
        {
            T[] result = token["d"].Children().Select(t =>
            {
                T value = new T();

                value.Initialize(_client, t);

                return value;

            }).ToArray();

            return result;
        }

        public bool HasValue
        {
            get { return _value != null; }
        }

        public async Task<T[]> GetAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_value == null)
            {
                JToken token = await _client.GetAsync(_deferredUri, cancellationToken);

                _value = GetValue(token);
            }

            return _value;
        }
    }
}