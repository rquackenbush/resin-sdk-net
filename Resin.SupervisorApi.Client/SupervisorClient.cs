namespace Resin.SupervisorApi.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal abstract class SupervisorClient : ISupervisorClient
    {
        protected HttpClient HttpClient { get; }

        protected SupervisorClient(HttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        protected abstract Task<HttpResponseMessage> MakeRequest(
            HttpMethod httpMethod, 
            string relativeUrl, 
            IDictionary<string, string> queryString = null,  
            object data = null,
            CancellationToken cancellationToken = new CancellationToken());

        private Exception CreateResponseException(HttpResponseMessage response)
        {
            return new SupervisorApiException($"Supervisor API returned: {response.StatusCode}: {response.ReasonPhrase}");
        }

        public async Task BlinkAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await MakeRequest(HttpMethod.Post, "/v1/blink", cancellationToken: cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
                throw CreateResponseException(response);
        }

        private async Task PostForceAsync(string relativeUrl, bool force, CancellationToken cancellationToken)
        {
            object data = null;

            if (force)
            {
                data = new
                {
                    force = true
                };
            }

            var response = await MakeRequest(HttpMethod.Post, relativeUrl, data: data, cancellationToken: cancellationToken);

            if (response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.OK)
                throw CreateResponseException(response);
        }

        public Task UpdateAsync(bool force = true, CancellationToken cancellationToken = new CancellationToken())
        {
            return PostForceAsync("/v1/update", force, cancellationToken);
        }

        public Task RebootAsync(bool force = false, CancellationToken cancellationToken = new CancellationToken())
        {
            return PostForceAsync("/v1/reboot", force, cancellationToken);
        }

        public Task ShutdownAsync(bool force = false, CancellationToken cancellationToken = new CancellationToken())
        {
            return PostForceAsync("/v1/shutdown", force, cancellationToken);
        }

        public async Task RestartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await MakeRequest(HttpMethod.Post, "/v1/restart", cancellationToken: cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
                throw CreateResponseException(response);
        }
    }
}