namespace Resin.SupervisorApi.Client
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;
    using Newtonsoft.Json;

    public class LocalSupervisorClient : SupervisorClient
    {
        private readonly string _supervisorAddress;
        private readonly string _supervisorApiKey;

        public LocalSupervisorClient(string supervisorAddress, string supervisorApiKey)
        {
            _supervisorAddress = supervisorAddress;
            _supervisorApiKey = supervisorApiKey;
        }

        protected override Task<HttpResponseMessage> MakeRequest(HttpMethod httpMethod, string relativeUrl, IDictionary<string, string> queryString = null, object data = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var requestQueryString = new Dictionary<string, string>()
            {
                { "apiKey", _supervisorApiKey }
            };

            if (queryString != null && queryString.Any())
            {
                foreach (var pair in queryString)
                {
                    requestQueryString[pair.Key] = pair.Value;
                }
            }

            string url = $"{_supervisorAddress}{relativeUrl}";

            url = QueryHelpers.AddQueryString(url, requestQueryString);

            var requestMessage = new HttpRequestMessage(httpMethod, url);

            if (data != null)
            {
                string json = JsonConvert.SerializeObject(data);

                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return HttpClient.SendAsync(requestMessage, cancellationToken);
        }
    }
}