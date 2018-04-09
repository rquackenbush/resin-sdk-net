namespace Resin.SupervisorApi.Client
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;

    public interface ISupervisorClient
    {
        ///// <summary>
        ///// Responds with a simple "OK", signaling that the supervisor is alive and well.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task PingAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Starts a blink pattern on a LED for 15 seconds, if your device has one. Responds with an empty 200 response. It implements the "identify device" feature from the dashboard.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BlinkAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Triggers an update check on the supervisor. Optionally, forces an update when updates are locked.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(bool force = false, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Reboots the device. This will first try to stop applications, and fail if there is an update lock. An optional "force" parameter in the body overrides the lock when true (and the lock can also be overridden from the dashboard).
        /// </summary>
        /// <param name="force">if set to true will cause the update lock to be overridden</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RebootAsync(bool force = false, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Dangerous. Shuts down the device. This will first try to stop applications, and fail if there is an update lock. An optional "force" parameter in the body overrides the lock when true (and the lock can also be overridden from the dashboard).
        /// </summary>
        /// <param name="force"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ShutdownAsync(bool force = false, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// Clears the user application's /data folder.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task PurgeAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Restarts a user application container
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RestartAsync(CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// When the device's connection to the Resin VPN is down, by default the device performs a TCP ping heartbeat to check for connectivity. This endpoint enables such TCP ping in case it has been disabled (see DELETE /v1/tcp-ping).
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task TcpPingAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// When the device's connection to the Resin VPN is down, by default the device performs a TCP ping heartbeat to check for connectivity. This endpoint disables such TCP ping.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task DisableTcpPingAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// Invalidates the current RESIN_SUPERVISOR_API_KEY and generates a new one. Responds with the new API key, but the application will be restarted on the next update cycle to update the API key environment variable.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task RegenerateApiKeyAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// ntroduced in supervisor v1.6. Returns the current device state, as reported to the Resin API and with some extra fields added to allow control over pending/locked updates.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<GetDeviceResponse> GetDeviceAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// emporarily stops a user application container. A reboot or supervisor restart will cause the container to start again. The container is not removed with this endpoint.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="appId"></param>
        ///// <param name="force"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task StopAppAsync(ISupervisorCallContext context, string appId, bool force = false, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// Starts a user application container, usually after it has been stopped with StopAppAsync.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="appId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task StartAppAsync(ISupervisorCallContext context, string appId, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// Returns the application running on the device 
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="appId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<GetAppResponse> GetAppAsync(ISupervisorCallContext context, string appId, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// Used internally to check whether the supervisor is running correctly, according to some heuristics that help determine whether the internal components, application updates and reporting to the Resin API are functioning.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task HealthCheckAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// This endpoint allows setting some configuration values for the host OS. Currently it supports proxy and hostname configuration.
        ///// 
        ///// For proxy configuration, resinOS 2.0.7 and higher provides a transparent proxy redirector (redsocks) that makes all connections be routed to a SOCKS or HTTP proxy. This endpoint allows user applications to modify these proxy settings at runtime.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task UpdateHostConfigAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());

        ///// <summary>
        ///// This endpoint allows reading some configuration values for the host OS, previously set with PATCH /v1/device/host-config. Currently it supports proxy and hostname configuration.
        ///// 
        ///// Added in supervisor v6.6.0.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task GetHostConfigAsync(ISupervisorCallContext context, CancellationToken cancellationToken = new CancellationToken());
    }
}