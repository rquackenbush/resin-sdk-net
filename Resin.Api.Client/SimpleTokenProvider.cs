using System.Threading;
using System.Threading.Tasks;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    /// <summary>
    /// Simple token provider. Doesn't handle refreshing the token.
    /// </summary>
    public class SimpleTokenProvider : ITokenProvider
    {
        private readonly string _token;

        public SimpleTokenProvider(string token)
        {
            _token = token;
        }

        public Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_token);
        }
    }
}