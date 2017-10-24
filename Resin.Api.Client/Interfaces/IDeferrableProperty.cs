using System.Threading;
using System.Threading.Tasks;

namespace Resin.Api.Client.Interfaces
{
    public interface IDeferrableProperty<TDataObject>
    {
        bool HasValue { get; }

        Task<TDataObject> GetAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}