using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client.Domain
{
    internal class DeviceEnvironmentVariable : EnvironmentVariable
    {
        public override string Name => GetValue<string>("env_var_name");
    }
}