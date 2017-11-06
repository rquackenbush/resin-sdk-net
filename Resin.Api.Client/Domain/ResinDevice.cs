using Resin.Api.Client.Interfaces;
using System;

namespace Resin.Api.Client.Domain
{
    public class ResinDevice : ODataObject
    {
        private DeferrableProperty<ResinApplication> _application;
        private DeferrableProperty<ResinUser> _user;
        // private DeferrableProperty<ServiceInstance> _serviceInstance;

        protected override void Initialize()
        {
            _application = new DeferrableProperty<ResinApplication>(Client, Token["application"]);
            _user = new DeferrableProperty<ResinUser>(Client, Token["user"]);
            //_serviceInstance = new DeferrableProperty<ServiceInstance>(Client, Token["service_instance"]);
        }

        public DateTime CreatedAt => GetValue<DateTime>("created_at");

        public int Id => GetValue<int>("id");

        public int Actor => GetValue<int>("actor");

        public string Uuid => GetValue<string>("uuid");

        public string LocalId => GetValue<string>("local_id");

        public string Name => GetValue<string>("name");

        public string Note => GetValue<string>("note");

        public string DeviceType => GetValue<string>("device_type");

        public bool IsOnline => GetValue<bool>("is_online");

        public double? Latitude => GetValue<double?>("latitude");

        public double? Longitude => GetValue<double?>("longitude");

        public double? CustomLatitude => GetValue<double?>("custom_latitude");

        public double? CustomLongitude => GetValue<double?>("custom_longitude");

        public string Location => GetValue<string>("location");

        public string LogsChannel => GetValue<string>("logs_channel");

        public string PublicAddress => GetValue<string>("public_address");

        public string VpnAddress => GetValue<string>("vpn_address");

        public string IpAddress => GetValue<string>("ip_address");

        public string Status => GetValue<string>("status");

        public string OsVersion => GetValue<string>("os_version");

        public string OsVariant => GetValue<string>("os_variant");

        public string SupervisorVersion => GetValue<string>("supervisor_version");

        public object ProvisioningProgress => GetValue<object>("provisioning_progress");

        public string ProvisioningState => GetValue<string>("provisioning_state");

        public object DownloadProgress => GetValue<object>("download_progress");

        public bool IsWebAcccessible => GetValue<bool>("is_web_accessible");

        public IDeferrableProperty<ResinApplication> Application
        {
            get
            {
                CheckInitialized();
                return _application;
            }
        }

        public IDeferrableProperty<ResinUser> User
        {
            get
            {
                CheckInitialized();
                return _user;
            }
        }

        //public IDeferrableProperty<ServiceInstance> ServiceInstance
        //{
        //    get
        //    {
        //        CheckInitialized();
        //        return _serviceInstance;
        //    }
        //}

        public DateTime? LockExpiryDate => GetValue<DateTime?>("lock_expiry_date");

        public string Commit => GetValue<string>("commit");

        public DateTime? SupportExpiryDate => GetValue<DateTime?>("support_expiry_date");

        public object SupervisorRelease => GetValue<object>("supervisor_release");

        public object Build => GetValue<object>("build");

        public object Device => GetValue<object>("device");

        public DateTime? LastSeenTime => GetValue<DateTime?>("last_seen_time");
    }
}