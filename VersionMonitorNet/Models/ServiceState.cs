namespace VersionMonitorNet.Models
{
    /// <summary>
    /// Dto for service state info
    /// </summary>
    public class ServiceState
    {
        public ServiceState(string serviceName, bool isRunning)
        {
            ServiceName = serviceName;
            IsRunning = isRunning;
        }

        /// <summary>
        /// Is service running or not
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// Name of the service
        /// </summary>
        public string ServiceName { get; private set; }
    }
}
