namespace Resin.SupervisorApi.Client
{
    using System;
    public interface ISupervisorClientFactory : IDisposable
    {
        ISupervisorClient CreateLocalClient(string supervisorAddress, string supervisorApiKey);

        ISupervisorClient CreateProxyClient(string uuid, string token, string baseUrl = SupervisorConstants.DefaultProxyUrl);
    }
}