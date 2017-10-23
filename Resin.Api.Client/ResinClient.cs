using System;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public async Task<ResinUser> GetUserAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var users = await GetAsync<ResinUser[]>("v1/user", cancellationToken);

            return users.FirstOrDefault();
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

                //Save an updated veresion of the token
                BearerToken = await response.Content.ReadAsStringAsync();

                return BearerToken;
            }
        }

        #region Applications

        /// <summary>
        /// Gets all applications.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ResinApplication[]> GetApplicationsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return GetAsync<ResinApplication[]>("v1/application", cancellationToken);
        }

        /// <summary>
        /// Get the environment variables for a given environment
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApplicationEnvironmentVariable[]> GetApplicationEnvironmentVariablesAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            return await GetAsync<ApplicationEnvironmentVariable[]>($"v1/environment_variable?$filter=application eq {applicationId}", cancellationToken);
        }

        public async Task<ResinApplication> GetApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ResinApplication[] applications = await GetAsync<ResinApplication[]>($"v1/application({id})", cancellationToken);

            return applications.FirstOrDefault();
        }

        public async Task<ResinApplication> GetApplicationAsync(string name,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ResinApplication[] applications = await GetAsync<ResinApplication[]>($"v1/application?$filter=app_name eq '{name}'", cancellationToken);

            return applications.FirstOrDefault();
        }

        public async Task<ResinApplication> CreateApplicationAsync(
            string name, 
            string deviceType,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var data = new 
            {
                app_name = name,
                device_type = deviceType
            };

            return await PostAsync<ResinApplication>("v1/application", data, cancellationToken);
        }

        public Task<ResinApplication> DeleteApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetProvisioningKeyAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            string raw = await PostRawAsync($"api-key/application/{applicationId}/provisioning", cancellationToken);

            if (!string.IsNullOrWhiteSpace(raw))
            {
                if (raw.StartsWith("\""))
                {
                    return raw.Substring(1, raw.Length - 2);
                }
            }

            return raw;
        }

        #endregion

        #region Devices

     
        /// <summary>
        /// Registers a new device.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="uuid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RegisterDeviceResult> RegisterDeviceAsync(int applicationId, string uuid, CancellationToken cancellationToken = new CancellationToken())
        {
            //Get the application
            ResinApplication application = await GetApplicationAsync(applicationId, cancellationToken);

            if (application == null)
                throw new ApplicationException($"Application with id {applicationId} was not found.");

            ResinUser user = await GetUserAsync(cancellationToken);

            if (user == null)
                throw new ApplicationException($"User not found.");

            var data = new
            {
                user = user.Id,
                application = application.Id,
                device_type = application.DeviceType,
                uuid
            };

            //Get the api key
            string apiKey = await GetProvisioningKeyAsync(applicationId, cancellationToken);

            using (var client = CreateHttpClient())
            {
                //Serialize it
                string json = JsonConvert.SerializeObject(data);

                //Create the request content
                var content = new StringContent(json, Encoding.UTF8, ContentTypeJson);

                //Make the request
                HttpResponseMessage response = await client.PostAsync($"device/register?apikey={apiKey}", content, cancellationToken);

                //Check the response
                await ThrowOnErrorAsync(response);

                //Get the response json
                string responseJson = await response.Content.ReadAsStringAsync();

                //Log it
                await LogResponseAsync(responseJson);

                //Get the result.
                return JsonConvert.DeserializeObject<RegisterDeviceResult>(responseJson);
            }
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

        public Task<ResinDevice[]> GetDevicesAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ResinDevice> GetDeviceAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            return GetAsync<ResinDevice>($"v1/device({id})", cancellationToken);
        }

        public Task<ResinDevice> GetDeviceAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            return GetAsync<ResinDevice>($"v1/device?$filter=name eq '{name}'", cancellationToken);
        }

        /// <summary>
        /// Add a note to a specific device.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddNoteAsync(int id, string note, CancellationToken cancellationToken = new CancellationToken())
        {
            return PatchAsync($"v1/device({id})", new {note}, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetStatusAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            ResinDevice[] resinDevices = await GetAsync<ResinDevice[]>($"v1/device({id})?$select=status", cancellationToken);

            return resinDevices.FirstOrDefault()?.Status;
        }


        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DeviceEnvironmentVariable[]> GetDeviceEnvironmentalVariablesAsync(int deviceId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await GetAsync<DeviceEnvironmentVariable[]>($"v1/device_environment_variable?$filter=device eq {deviceId}", cancellationToken);
        }

        public Task CreateDeviceEnvironmentVariableAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task UpdateDeviceEnvironmentVariable(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteDeviceApplicationVariableAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Restarts the application container on a given device. 
        /// </summary>
        /// <param name="deviceId">The id </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RestartDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeviceCommandAsync(deviceId, "restart", cancellationToken);
        }

        /// <summary>
        /// Reboots the device.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RebootDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeviceCommandAsync(deviceId, "reboot", cancellationToken);
        }

        /// <summary>
        /// Dangerous. Shuts down the device.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ShutdownDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeviceCommandAsync(deviceId, "shutdown", cancellationToken);
        }

        /// <summary>
        /// Sends a command to a device.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task DeviceCommandAsync(int deviceId, string command, CancellationToken cancellationToken)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.PostAsync($"device/{deviceId}/{command}", new ByteArrayContent(new byte[] { }), cancellationToken);

                await ThrowOnErrorAsync(response);
            }
        }

        public Task MoveDeviceAsync(int deviceId, int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
