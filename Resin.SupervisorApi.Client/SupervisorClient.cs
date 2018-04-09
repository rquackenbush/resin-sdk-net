namespace Resin.SupervisorApi.Client
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class SupervisorClient : ISupervisorClient
    {
        protected HttpClient HttpClient { get; } = new HttpClient();

        protected abstract Task<HttpResponseMessage> MakeRequest(
            HttpMethod httpMethod, 
            string relativeUrl, 
            IDictionary<string, string> queryString = null,  
            object data = null,
            CancellationToken cancellationToken = new CancellationToken());

        public async Task BlinkAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await MakeRequest(HttpMethod.Post, "/v1/blink", cancellationToken: cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new SupervisorApiException($"Supervisor API returned: {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
    }
}