using System;
using Newtonsoft.Json;

namespace Resin.Api.Client.Domain
{
    public class ResinDevice
    {
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Actor")]
        public int Actor { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "device_type")]
        public string DeviceType { get; set; }

        [JsonProperty(PropertyName = "is_online")]
        public bool IsOnline { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "custom_latitude")]
        public double? CustomLatitude { get; set; }

        [JsonProperty(PropertyName = "custom_longitude")]
        public double? CustomLongitude { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "logs_channel")]
        public string LogsChannel { get; set; }

        [JsonProperty(PropertyName = "public_address")]
        public string PublicAddress { get; set; }

        [JsonProperty(PropertyName = "vpn_address")]
        public string VpnAddress { get; set; }

        [JsonProperty(PropertyName = "ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "os_version")]
        public string OsVersion { get; set; }

        [JsonProperty(PropertyName = "os_variant")]
        public string OsVariant { get; set; }

        [JsonProperty(PropertyName = "supervisor_version")]
        public string SupervisorVersion { get; set; }

        [JsonProperty(PropertyName = "provisioning_progress")]
        public object ProvisioningProgress { get; set; }

        [JsonProperty(PropertyName = "provisioning_state")]
        public string ProvisioningState { get; set; }

        [JsonProperty(PropertyName = "download_progress")]
        public object DownloadProgress { get; set; }

        [JsonProperty(PropertyName = "is_web_accessible")]
        public bool IsWebAcccessible { get; set; }

        /*
 
          "application": {
        "__deferred": {
          "uri": "/resin/application(569478)"
        },
        "__id": 569478
      },
      "user": {
        "__deferred": {
          "uri": "/resin/user(21071)"
        },
        "__id": 21071
      },
         */

        [JsonProperty(PropertyName = "lock_expiry_date")]
        public DateTime? LockExpiryDate { get; set; }

        [JsonProperty(PropertyName = "commit")]
        public string Commit { get; set; }

        [JsonProperty(PropertyName = "support_expiry_date")]
        public DateTime? SupportExpiryDate { get; set; }

        [JsonProperty(PropertyName = "supervisor_release")]
        public object SupervisorRelease { get; set; }

        /*
        "service_instance": {
        "__deferred": {
          "uri": "/resin/service_instance(15)"
        },
        "__id": 15
        },
        */
        
        [JsonProperty(PropertyName = "build")]
        public object Build { get; set; }
        
        [JsonProperty(PropertyName = "device")]
        public object Device { get; set; }
        
        [JsonProperty(PropertyName = "last_seen_time")]
        public DateTime? LastSeenTime { get; set; }
    }
}
