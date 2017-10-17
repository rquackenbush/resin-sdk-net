using Newtonsoft.Json;

namespace Resin.Api.Client
{
    internal class ODataResponse<TData>
    {
        [JsonProperty(PropertyName = "d")]
        public TData D { get; set; }
    }
}
