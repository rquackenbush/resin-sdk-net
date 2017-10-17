using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Resin.Api.Client;

namespace Resin.Api.Client
{
    public abstract class ApiClientBase
    {
        private readonly string _baseAddress;

        protected ApiClientBase(string bearerToken, string baseAddress = "https://api.resin.io/v1")
        {
            BearerToken = bearerToken;
            _baseAddress = baseAddress;
        }

        protected string BearerToken { get; set; }

        protected HttpClient CreateHttpClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.BaseAddress = new Uri(_baseAddress);

            return client;
        }

        protected virtual Task LogResponse(string response)
        {
            Console.WriteLine(response.FormatJson());

            return Task.CompletedTask;
        }


        /// <summary>
        /// Throws an exception if the 
        /// </summary>
        /// <param name="response"></param>
        protected virtual async Task ThrowOnErrorAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string content = null;

                try
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {   
                }

                throw new ApiClientException($"{response.StatusCode}: {response.ReasonPhrase} {content}");
            }
        }
    }
}