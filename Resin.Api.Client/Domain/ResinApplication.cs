using System;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client.Domain
{
    public class ResinApplication : ODataObject
    {
        private DeferrableProperty<ResinUser> _user;

        protected override void Initialize()
        {
            _user = new DeferrableProperty<ResinUser>(Client, Token["user"]);
        }

        public string AppName => GetValue<string>("app_name");

        public int Id => GetValue<int>("id");

        public int Actor => GetValue<int>("actor");

        public string DeviceType => GetValue<string>("device_type");

        public string GitRepository => GetValue<string>("git_repository");

        public string Commit => GetValue<string>("commit");

        public int Version => GetValue<int>("version");

        public bool ShouldTrackLatestRelease => GetValue<bool>("should_track_latest_release");

        public DateTime? SupportExpiryDate => GetValue<DateTime?>("support_expiry_date");

        public object Application => GetValue<object>("application");

        public IDeferrableProperty<ResinUser> User
        {
            get
            {
                CheckInitialized();
                return _user;
            }
        }
    }
}