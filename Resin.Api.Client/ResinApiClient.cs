using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Resin.Api.Client.Domain;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    public class ResinApiClient : ApiClientBase
    {
        /// <summary>
        /// resin.io Data API Service client. https://docs.resin.io/runtime/data-api/
        /// </summary>
        /// <param name="tokenProvider"></param>
        /// <param name="baseAddress"></param>
        public ResinApiClient(ITokenProvider tokenProvider, string baseAddress = "https://api.resin.io/v1/") 
            : base(tokenProvider,
            baseAddress)
        {
        }

        /// <summary>
        /// Get the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinUser> GetUserAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync("user", cancellationToken);

            ResinUser[] users = token.ToDataObjectArray<ResinUser>(this);

            var user = users?.FirstOrDefault();

            if (user == null)
                throw new ObjectNotFoundException("Unable to find user.");

            return user;
        }

        /// <summary>
        /// Get the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinUser> GetUserAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"user({id})", cancellationToken);

            ResinUser[] users = token.ToDataObjectArray<ResinUser>(this);

            var user = users?.FirstOrDefault();

            if (user == null)
                throw new ObjectNotFoundException($"Unable to find user with id {id}.");

            return user;
        }

        /// <summary>
        /// https://docs.resin.io/runtime/data-api/#resource-whoami
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> WhoamiAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken))
            {
                HttpResponseMessage response = await client.GetAsync("/whoami", cancellationToken);

                await ThrowOnErrorAsync(response);

                //Save an updated veresion of the token
                return await response.Content.ReadAsStringAsync();               
            }
        }

        #region Applications

        /// <summary>
        /// Gets all applications.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinApplication[]> GetApplicationsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync("application", cancellationToken);

            return token.ToDataObjectArray<ResinApplication>(this);
        }

        /// <summary>
        /// Get the environment variables for a given environment
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApplicationEnvironmentVariable[]> GetApplicationEnvironmentVariablesAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"environment_variable?$filter=application eq {applicationId}", cancellationToken);

            return token.ToDataObjectArray<ApplicationEnvironmentVariable>(this);
        }

        /// <summary>
        /// Get an application by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinApplication> GetApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"application({id})", cancellationToken);

            ResinApplication[] applications = token.ToDataObjectArray<ResinApplication>(this);

            var application = applications.FirstOrDefault();

            if (application == null)
                throw new ObjectNotFoundException($"Application with id {id} not found.");

            return application;
        }

        /// <summary>
        /// Get an application by name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinApplication> GetApplicationAsync(string name,
            CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"application?$filter=app_name eq '{name}'", cancellationToken);

            var application = token.ToDataObjectArray<ResinApplication>(this).FirstOrDefault();

            if (application == null)
                throw new ObjectNotFoundException($"Application with name '{name}' not found.");

            return application;
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

            JToken token = await PostAsync("application", data, cancellationToken);

            return token.ToDataObjectDirect<ResinApplication>(this);
        }

        public Task<ResinApplication> DeleteApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetProvisioningKeyAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            string raw = await PostRawAsync($"/api-key/application/{applicationId}/provisioning", cancellationToken);

            if (raw.Length >= 3)
            { 
                return raw.Substring(1, raw.Length - 2);
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

            //Get the user
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

            //Do it
            var token = await PostAsync($"device/register?apikey={apiKey}", data, cancellationToken);

            return token.ToDataObjectDirect<RegisterDeviceResult>(this);
        }

        
        /// <summary>
        /// https://docs.resin.io/runtime/data-api/#get-all-devices
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinDevice[]> GetDevicesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync("device", cancellationToken);

            return token.ToDataObjectArray<ResinDevice>(this);
        }

        /// <summary>
        /// Gets the devices for a given application.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ResinDevice[]> GetDevicesAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a single device by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinDevice> GetDeviceAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"v1/device({id})", cancellationToken);

            return token.ToDataObject<ResinDevice>(this);
        }

        /// <summary>
        /// Get a single device by name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResinDevice> GetDeviceAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"device?$filter=name eq '{name}'", cancellationToken);

            ResinDevice[] devices = token.ToDataObjectArray<ResinDevice>(this);

            var device = devices.FirstOrDefault();

            if (device == null)
                throw new ObjectNotFoundException($"Unable to find device with name '{name}'.");

            return device;

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
            return PatchAsync($"device({id})", new {note}, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetStatusAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"device({id})?$select=status", cancellationToken);

            ResinDevice[] devices = token.ToDataObjectArray<ResinDevice>(this);

            var device = devices.FirstOrDefault();

            if (device == null)
                throw new ObjectNotFoundException($"Unable to find device with id {id}.");

            return device.Status;
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
            JToken token = await GetAsync($"device_environment_variable?$filter=device eq {deviceId}", cancellationToken);

            return token.ToDataObjectArray<DeviceEnvironmentVariable>(this);
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
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken))
            {
                var response = await client.PostAsync($"device/{deviceId}/{command}", new ByteArrayContent(new byte[] { }), cancellationToken);

                await ThrowOnErrorAsync(response);
            }
        }

        /// <summary>
        ///  Move a device to another application.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="applicationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task MoveDeviceAsync(int deviceId, int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
