using Newtonsoft.Json;

namespace Resin.Api.Client.Domain
{
    public class DeviceEnvironmentVariable
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "env_var_name")]
        public string EnvironmentVariableName { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}