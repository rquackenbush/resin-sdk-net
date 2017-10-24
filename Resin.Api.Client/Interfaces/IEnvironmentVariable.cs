namespace Resin.Api.Client.Interfaces
{
    public interface IEnvironmentVariable
    {
        int Id { get; }

        string Name { get; }

        string Value { get; }
    }
}