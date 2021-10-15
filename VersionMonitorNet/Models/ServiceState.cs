namespace Anexia.Monitoring.Models
{
    /// <summary>
    ///     Dto for service state info
    /// </summary>
    public class ServiceState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceState"/> class.
        /// </summary>
        /// <param name="serviceName">The service's name</param>
        /// <param name="isRunning">Indicator whether the service is currently running</param>
        public ServiceState(string serviceName, bool isRunning)
        {
            ServiceName = serviceName;
            IsRunning = isRunning;
        }

        /// <summary>
        ///     Gets a value indicating whether the service is running or not
        /// </summary>
        public bool IsRunning { get; }

        /// <summary>
        ///    Gets the service's name
        /// </summary>
        public string ServiceName { get; }
    }
}