namespace Resin.Api.Client.Domain
{
    public class RegisterDeviceResult : ODataObject
    {
        public int Id => GetValue<int>("id");

        public string Uuid => GetValue<string>("uuid");

        public string ApiKey => GetValue<string>("api_key");
    }
}