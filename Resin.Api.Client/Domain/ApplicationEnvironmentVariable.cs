using Newtonsoft.Json;
namespace Resin.Api.Client.Domain
{
    public class ApplicationEnvironmentVariable : ODataObject
    {
        public int Id => GetValue<int>("id");

        public string Name => GetValue<string>("name");

        public string Value => GetValue<string>("value");
    }
}