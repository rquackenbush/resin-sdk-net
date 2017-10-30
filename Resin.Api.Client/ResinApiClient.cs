using Newtonsoft.Json.Linq;
using Resin.Api.Client.Domain;
using Resin.Api.Client.Exceptions;
using Resin.Api.Client.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Resin.Api.Client
{
    public class ResinApiClient : ApiClientBase
    {
        public const string DefaultApiAddress = "https://api.resin.io/v1/";

        /// <summary>
        /// resin.io Data API Service client. https://docs.resin.io/runtime/data-api/
        /// </summary>
        /// <param name="tokenProvider"></param>
        /// <param name="baseAddress"></param>
        public ResinApiClient(ITokenProvider tokenProvider, string baseAddress = DefaultApiAddress)
            : base(tokenProvider,
            baseAddress)
        {
        }

        #region User

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
        /// Get a particular user by id.
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

        #endregion

        #region Applications

        /// <summary>
        /// Gets all applications.
        /// https://docs.resin.io/runtime/data-api/#get-all-applications
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
        /// https://docs.resin.io/runtime/data-api/#get-all-application-variables
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EnvironmentVariable[]> GetApplicationEnvironmentVariablesAsync(int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"environment_variable?$filter=application eq {applicationId}", cancellationToken);

            return token.ToDataObjectArray<ApplicationEnvironmentVariable>(this)
                .Cast<EnvironmentVariable>()
                .ToArray();
        }

        /// <summary>
        /// Get an application by id.
        /// https://docs.resin.io/runtime/data-api/#get-application
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
        /// 
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

        /// <summary>
        /// https://docs.resin.io/runtime/data-api/#delete-application
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return DeleteAsync($"application({id})", cancellationToken);
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
                throw new ObjectNotFoundException($"Application with id {applicationId} was not found.");

            //Get the user
            ResinUser user = await GetUserAsync(cancellationToken);

            //uuid = Guid.NewGuid().ToString("N");
            //Create the data for the request
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
            var token = await PostAsync($"device/register?apikey={apiKey}", Guid.NewGuid().ToString("N"), cancellationToken);

            //Woot - we're done
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
        /// https://docs.resin.io/runtime/data-api/#get-device-by-id
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
        /// https://docs.resin.io/runtime/data-api/#get-device-by-name
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
        /// Delete a specific device.
        /// https://docs.resin.io/runtime/data-api/#delete-device
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteDeviceAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeleteAsync($"device({id})", cancellationToken);
        }

        /// <summary>
        /// Add a note to a specific device.
        /// https://docs.resin.io/runtime/data-api/#add-note
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddNoteAsync(int id, string note, CancellationToken cancellationToken = new CancellationToken())
        {
            return PatchAsync($"device({id})", new { note }, cancellationToken);
        }

        /// <summary>
        /// Get the current status of the specified device
        /// https://docs.resin.io/runtime/data-api/#get-status
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
        /// Get all environment variables of the device specified by the given id.
        /// https://docs.resin.io/runtime/data-api/#create-device-variable
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EnvironmentVariable[]> GetDeviceEnvironmentalVariablesAsync(int deviceId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            JToken token = await GetAsync($"device_environment_variable?$filter=device eq {deviceId}", cancellationToken);

            return token.ToDataObjectArray<DeviceEnvironmentVariable>(this)
                .Cast<EnvironmentVariable>()
                .ToArray();
        }

        /// <summary>
        /// Create new environment variable with a given name and value, for the given device
        /// https://docs.resin.io/runtime/data-api/#create-device-variable
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CreateDeviceEnvironmentVariableAsync(int deviceId, string name, string value, CancellationToken cancellationToken = new CancellationToken())
        {
            var data = new
            {
                device = deviceId,
                env_var_name = name,
                value
            };

            return PostAsync("device_environment_variable", data, cancellationToken);
        }

        /// <summary>
        /// Update a device environment variable with a new value, given the ID of the variable
        /// https://docs.resin.io/runtime/data-api/#update-device-variable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpdateDeviceEnvironmentVariable(int id, string value, CancellationToken cancellationToken = new CancellationToken())
        {
            var data = new
            {
                value
            };

            return PatchAsync($"device_environment_variable({id})", data, cancellationToken);
        }

        /// <summary>
        /// Update an application environment variable with a new value, given the ID of the variable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name">Application variable name</param>
        /// <param name="newValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpdateApplicationVariableAsync(int id, string name, string newValue, CancellationToken cancellationToken = new CancellationToken())
        {
            var data = new { name = newValue };

            return PatchAsync($"environment_variable({id})", data, cancellationToken);
        }

        /// <summary>
        /// Remove a device environment variable with a specified ID
        /// https://docs.resin.io/runtime/data-api/#delete-device-variable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteDeviceEnvironmentVariableAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeleteAsync($"device_environment_variable({id})", cancellationToken);
        }

        /// <summary>
        /// Starts a blink pattern on a LED for 15 seconds, if your device has one. Responds with an empty 200 response. It implements the "identify device" feature from the dashboard.
        /// https://docs.resin.io/runtime/supervisor-api/#post-v1-blink
        /// </summary>
        /// <param name="deviceId">The id </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task BlinkDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeviceCommandAsync(deviceId, "blink", cancellationToken);
        }

        public Task RenameDeviceAsync(int deviceId, string newName, CancellationToken cancellationToken = new CancellationToken())
        {
            var data = new { name = newName };

            return PatchAsync($"device({deviceId})", data, cancellationToken);
        }

        public Task RenameApplicationAsync(int deviceId, string newName, CancellationToken cancellationToken = new CancellationToken())
        {
            var data = new { name = newName };

            return PatchAsync($"application({deviceId})", data, cancellationToken);
        }

        /// <summary>
        /// Restarts the application container on a given device. 
        /// https://docs.resin.io/runtime/supervisor-api/#post-v1-restart
        /// </summary>
        /// <param name="deviceId">The id of the device to be restarted. </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RestartDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            return DeviceCommandAsync(deviceId, "restart", cancellationToken);
        }

        /// <summary>
        /// Reboots the device.
        /// https://docs.resin.io/runtime/supervisor-api/#post-v1-reboot
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
        /// https://docs.resin.io/runtime/supervisor-api/#post-v1-shutdown
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
        /// Move the specified device to another application, given the application ID.
        /// https://docs.resin.io/runtime/data-api/#move-device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="applicationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task MoveDeviceAsync(int deviceId, int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            var request = new
            {
                application = applicationId
            };

            return PatchAsync($"device({deviceId})", request, cancellationToken);
        }

        #endregion
    }
}
