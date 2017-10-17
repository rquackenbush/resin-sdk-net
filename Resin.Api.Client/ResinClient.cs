using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Resin.Api.Client.Domain;

namespace Resin.Api.Client
{
    public class ResinClient : ApiClientBase
    {
        public ResinClient(string bearerToken, string baseAddress = "https://api.resin.io/v1") : base(bearerToken,
            baseAddress)
        {
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
                await LogResponse(json);

                //Deserialize the response
                ODataResponse<TResponse> resinResponse = JsonConvert.DeserializeObject<ODataResponse<TResponse>>(json);

                //And return the deserialized object(s)
                return resinResponse?.D;
            }
        }

        /// <summary>
        /// https://docs.resin.io/runtime/data-api/#resource-whoami
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> WhoamiAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using (var client = CreateHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("/whoami", cancellationToken);

                await ThrowOnErrorAsync(response);

                BearerToken = await response.Content.ReadAsStringAsync();

                return BearerToken;
            }
        }

        /// <summary>
        /// Gets all applications
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ResinApplication[]> GetApplicationsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return GetAsync<ResinApplication[]>("v1/application", cancellationToken);
        }

        /// <summary>
        /// https://docs.resin.io/runtime/data-api/#get-all-devices
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ResinDevice[]> GetDevicesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return GetAsync<ResinDevice[]>("v1/device", cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetMetadataAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using (var client = CreateHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("v1/$metadata", cancellationToken);

                await ThrowOnErrorAsync(response);

                string json = await response.Content.ReadAsStringAsync();

                return json.FormatJson();
            }
        }

        /// <summary>
        /// Restart the 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="appId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RestartAsync(int deviceId, int appId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.PostAsync($"device/{deviceId}/restart", new ByteArrayContent(new byte[]{}), cancellationToken);

                await ThrowOnErrorAsync(response);
            }
        }
    }
}
