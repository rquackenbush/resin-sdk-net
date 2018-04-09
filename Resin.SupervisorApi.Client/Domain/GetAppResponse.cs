namespace Resin.SupervisorApi.Client.Domain
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class GetAppResponse
    {
        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("commit")]
        public string Commit { get; set; }

        [JsonProperty("imageId")]
        public string ImageId { get; set; }

        [JsonProperty("containerId")]
        public string ContainerId { get; set; }

        [JsonProperty("env")]
        public Dictionary<string, string> Env { get; set; }
    }
}