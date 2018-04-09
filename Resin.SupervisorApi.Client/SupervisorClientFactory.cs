namespace Resin.SupervisorApi.Client
{
    using System.Net.Http;

    public class SupervisorClientFactory : ISupervisorClientFactory
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public ISupervisorClient CreateLocalClient(string supervisorAddress, string supervisorApiKey)
        {
            return new LocalSupervisorClient(_httpClient, supervisorAddress, supervisorApiKey);
        }

        public ISupervisorClient CreateProxyClient(string uuid, string token, string baseUrl = SupervisorConstants.DefaultProxyUrl)
        {
            return new ProxySupervisorClient(_httpClient, uuid, token, baseUrl);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    
}