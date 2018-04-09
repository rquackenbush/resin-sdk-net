namespace Resin.SupervisorApi.Client.Domain
{
    using Newtonsoft.Json;

    public class GetDeviceResponse
    {
        [JsonProperty("api_port")]
        public int ApiPort { get; set; }

        [JsonProperty("commit")]
        public string Commit { get; set; }

        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty("download_progress")]
        public int? DownloadProgress { get; set; }

        [JsonProperty("os_version")]
        public string OsVersion { get; set; }

        [JsonProperty("supervisor_version")]
        public string SupervisorVersion { get; set; }

        [JsonProperty("update_pending")]
        public bool UpdatePending { get; set; }

        [JsonProperty("update_failed")]
        public bool UpdateFailed { get; set; }
    }
}