using System;
using Newtonsoft.Json;

namespace Resin.Api.Client.Domain
{
    public class ResinApplication
    {
        [JsonProperty(PropertyName = "app_name")]
        public string AppName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "actor")]
        public int Actor { get; set; }

        [JsonProperty(PropertyName = "device_type")]
        public string DeviceType { get; set; }

        [JsonProperty(PropertyName = "git_repository")]
        public string GitRepository { get; set; }

        [JsonProperty(PropertyName = "commit")]
        public string Commit { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "should_track_latest_release")]
        public bool ShouldTrackLatestRelease { get; set; }

        [JsonProperty(PropertyName = "support_expiry_date")]
        public DateTime? SupportExpiryDate { get; set; }
        
        [JsonProperty(PropertyName = "application")]
        public object Application { get; set; }
        
    }
}
