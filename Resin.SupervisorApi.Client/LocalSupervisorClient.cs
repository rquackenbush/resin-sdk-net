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

    internal class LocalSupervisorClient : SupervisorClient
    {
        private readonly string _supervisorAddress;
        private readonly string _supervisorApiKey;

        public LocalSupervisorClient(HttpClient httpClient, string supervisorAddress, string supervisorApiKey) 
            : base(httpClient)
        {
            _supervisorAddress = supervisorAddress;
            _supervisorApiKey = supervisorApiKey;
        }

        protected override Task<HttpResponseMessage> MakeRequest(HttpMethod httpMethod, string relativeUrl, IDictionary<string, string> queryString = null, object data = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            //We always add the api key to the request
            var requestQueryString = new Dictionary<string, string>()
            {
                { "apiKey", _supervisorApiKey }
            };

            //CHeck to see if we need to add additional query string parameters
            if (queryString != null && queryString.Any())
            {
                foreach (var pair in queryString)
                {
                    requestQueryString[pair.Key] = pair.Value;
                }
            }

            //Format the url
            string url = QueryHelpers.AddQueryString($"{_supervisorAddress}{relativeUrl}", requestQueryString);

            //Create the request
            var requestMessage = new HttpRequestMessage(httpMethod, url);

            //Check to see if there is a body
            if (data != null)
            {
                string json = JsonConvert.SerializeObject(data);

                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            //Send it now!!!
            return HttpClient.SendAsync(requestMessage, cancellationToken);
        }
    }
}