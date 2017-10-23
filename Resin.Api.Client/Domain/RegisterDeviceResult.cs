using Newtonsoft.Json;

namespace Resin.Api.Client.Domain
{
    public class RegisterDeviceResult
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        [JsonProperty(PropertyName = "api_key")]
        public string ApiKey { get; set; }
    }
}