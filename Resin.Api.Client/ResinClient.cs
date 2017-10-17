using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Resin.Api.Client.Domain;

namespace Resin.Api.Client
{
    public class ResinClient : ApiClientBase
    {
        public ResinClient(string bearerToken, string baseAddress = "https://api.resin.io/v1") : base(bearerToken,
            baseAddress)
        {
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

        public Task<ResinApplication> GetApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ResinApplication> GetApplicationAsync(string name,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ResinApplication> CreateApplicationAsync(
            string name, 
            string deviceType,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ResinApplication> DeleteApplicationAsync(int id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Devices

        /// <summary>
        /// https://docs.resin.io/runtime/data-api/#get-all-devices
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ResinDevice[]> GetDevicesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return GetAsync<ResinDevice[]>("v1/device", cancellationToken);
        }

        public Task<ResinDevice[]> GetDevicesAsync(int applicationId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ResinDevice> GetDeviceAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ResinDevice> GetDeviceAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task AddDeviceNoteAsync(int id, string note, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
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
        public async Task RestartDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.PostAsync($"device/{deviceId}/restart", new ByteArrayContent(new byte[]{}), cancellationToken);

                await ThrowOnErrorAsync(response);
            }
        }

        public Task RebootDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task ShutdownDeviceAsync(int deviceId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task MoveDeviceAsync(int deviceId, int applicationId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
