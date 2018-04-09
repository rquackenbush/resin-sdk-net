namespace Resin.SupervisorApi.Client
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;
    using Newtonsoft.Json;

    internal class ProxySupervisorClient : SupervisorClient 
    {
        private readonly string _baseUrl;
        private readonly string _uuid;
        private readonly string _token;

        public ProxySupervisorClient(HttpClient httpClient, string uuid, string token, string baseUrl = "https://api.resin.io/supervisor") 
            : base(httpClient)
        {
            _baseUrl = baseUrl;
            _uuid = uuid;
            _token = token;
        }

        protected override Task<HttpResponseMessage> MakeRequest(
            HttpMethod httpMethod, 
            string relativeUrl, 
            IDictionary<string, string> queryString = null, 
            object data = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (httpMethod == HttpMethod.Get)
            {
                //GET operations have to be POST for the proxy as we're always sending a request object.
                httpMethod = HttpMethod.Post;
            }

            object requestData;

            if (data == null)
            {
                requestData = new
                {
                    uuid = _uuid,
                };
            }
            else
            {
                requestData = new
                {
                    uuid = _uuid,
                    data = data
                };
            }

            string requestJson = JsonConvert.SerializeObject(requestData);

            string url = $"{_baseUrl}{relativeUrl}";

            if (queryString != null && queryString.Any())
            {
                url = QueryHelpers.AddQueryString(url, queryString);
            }

            var requestMessage = new HttpRequestMessage(httpMethod, url)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            return HttpClient.SendAsync(requestMessage, cancellationToken);
        }
    }
}