namespace Resin.Api.Client.Domain
{
    public class DeviceEnvironmentVariable : ODataObject
    {
        public int Id => GetValue<int>("id");

        public string EnvironmentVariableName => GetValue<string>("env_var_name");

        public string Value => GetValue<string>("value");
    }
}