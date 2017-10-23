using System.Threading;
using System.Threading.Tasks;

namespace Resin.Api.Client.Interfaces
{
    /// <summary>
    /// Provides tokens for the resin api.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Gets a token.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
       Task<string> GetTokenAsync(CancellationToken cancellationToken);
    }
}