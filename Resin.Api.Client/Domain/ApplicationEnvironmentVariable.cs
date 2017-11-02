namespace Resin.Api.Client.Domain
{
    public class ApplicationEnvironmentVariable : EnvironmentVariable
    {
        public override string Name => GetValue<string>("name");
    }
}