using Newtonsoft.Json.Linq;

namespace Resin.Api.Client.Interfaces
{
    public interface IDeferrableObject
    {
        void Initialize(ApiClientBase client, JToken token);
    }
}