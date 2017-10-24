using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    public abstract class ApiClientBase
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly string _baseAddress;

        protected const string ContentTypeJson = "application/json";

        protected ApiClientBase(ITokenProvider tokenProvider, string baseAddress)
        {
            if (tokenProvider == null) throw new ArgumentNullException(nameof(tokenProvider));
            _tokenProvider = tokenProvider;
            _baseAddress = baseAddress;
        }

        protected async Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            string token = await _tokenProvider.GetTokenAsync(cancellationToken);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentTypeJson));

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
            //Check to see if it was successful.
            if (!response.IsSuccessStatusCode)
            {
                string content = null;

                try
                {
                    //Try to get the string data.
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {   
                }

                throw new ApiClientException($"{response.StatusCode}: {response.ReasonPhrase} {content}");
            }
        }

        /// <summary>
        /// Perform a get and return a deserialized response.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected internal async Task<JToken> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken))
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
                return JToken.Parse(json);
            }
        }

        /// <summary>
        /// Post a json request and return the deserialized result.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected internal async Task<JToken> PostAsync(string requestUri, object request, CancellationToken cancellationToken)
        {
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken))
            {
                string requestJson = JsonConvert.SerializeObject(request);

                StringContent requestContent = new StringContent(requestJson, Encoding.UTF8, ContentTypeJson);

                //Perform the get
                HttpResponseMessage response = await client.PostAsync(requestUri, requestContent, cancellationToken);

                //Check for error
                await ThrowOnErrorAsync(response);

                //get the response
                string json = await response.Content.ReadAsStringAsync();

                //Log the response
                await LogResponseAsync(json);

                return JToken.Parse(json);
            }
        }

        /// <summary>
        /// Posts an empty request and returns the string result.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected internal async Task<string> PostRawAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken))
            {
                StringContent requestContent = new StringContent("");

                //Perform the get
                HttpResponseMessage response = await client.PostAsync(requestUri, requestContent, cancellationToken);

                //Check for error
                await ThrowOnErrorAsync(response);

                //get the response
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// PATCH
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected internal async Task PatchAsync(
            string requestUri, 
            object request, 
            CancellationToken cancellationToken = new CancellationToken())
        {
            string requestJson = JsonConvert.SerializeObject(request);

            StringContent requestContent = new StringContent(requestJson, Encoding.UTF8, ContentTypeJson);

            using (HttpClient client = await CreateHttpClientAsync(cancellationToken))
            {
                // https://stackoverflow.com/a/29772349/232566
                var method = new HttpMethod("PATCH");

                //Create the request message
                var requestMessage = new HttpRequestMessage(method, requestUri)
                {
                    Content = requestContent
                };

                //Send it!
                await client.SendAsync(requestMessage, cancellationToken);
            }
        }
    }
}