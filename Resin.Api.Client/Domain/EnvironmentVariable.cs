using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client.Domain
{
    public abstract class EnvironmentVariable : ODataObject
    {
        public abstract string Name { get; }

        public int Id => GetValue<int>("id");

        public string Value => GetValue<string>("value");
    }
}