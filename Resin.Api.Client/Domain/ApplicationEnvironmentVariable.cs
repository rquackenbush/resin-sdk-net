namespace Resin.Api.Client.Domain
{
    internal class ApplicationEnvironmentVariable : EnvironmentVariable
    {
        public override string Name => GetValue<string>("name");
    }
}