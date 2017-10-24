using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Resin.Api.Client.Domain
{
    public class ResinUser : ODataObject
    {
        public int Id => GetValue<int>("id");

        public int Actor => GetValue<int>("actor");

        public string Username => GetValue<string>("username");
    }
}