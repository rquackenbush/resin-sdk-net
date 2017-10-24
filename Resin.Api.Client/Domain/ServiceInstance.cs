namespace Resin.Api.Client.Domain
{
    public class ServiceInstance : ODataObject
    {
        public int Id => GetValue<int>("id");
    }
}