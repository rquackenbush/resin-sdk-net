using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Resin.Api.Client.Domain
{
    public class ResinUser
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "actor")]
        public int Actor { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
}