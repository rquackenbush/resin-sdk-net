using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Resin.Api.Client
{
    public abstract class ApiClientBase
    {
        private readonly string _baseAddress;

        protected ApiClientBase(string bearerToken, string baseAddress)
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

        protected virtual Task LogResponseAsync(string response)
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

        protected async Task<TResponse> GetAsync<TResponse>(string requestUri, CancellationToken cancellationToken)
            where TResponse : class
        {
            using (var client = CreateHttpClient())
            {
                //Perform the get
                HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);

                //Check for error
                await ThrowOnErrorAsync(response);

                //get the response
                string json = await response.Content.ReadAsStringAsync();

                //Log the response
                await LogResponseAsync(json);

                //Deserialize the response
                ODataResponse<TResponse> resinResponse = JsonConvert.DeserializeObject<ODataResponse<TResponse>>(json);

                //And return the deserialized object(s)
                return resinResponse?.D;
            }
        }

        /// <summary>
        /// PATCH
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var client = CreateHttpClient())
            {
                // https://stackoverflow.com/a/29772349/232566

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = content
                };

                return await client.SendAsync(request, cancellationToken);
            }
        }

        protected Task PatchAsync(
            string requestUri, 
            object request, 
            CancellationToken cancellationToken = new CancellationToken())
        {
            string json = JsonConvert.SerializeObject(request);

            StringContent requestContent = new StringContent(json);

            return PatchAsync(requestUri, requestContent, cancellationToken);
        }
    }
}