using Newtonsoft.Json;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    /// Dto for the runtime info
    /// </summary>
    [JsonObject(Title = "runtime")]
    public class RuntimeInfo
    {
        /// <summary>
        /// Platform
        /// </summary>
        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }
        /// <summary>
        /// Platform version
        /// </summary>
        [JsonProperty(PropertyName = "platform_version")]
        public string PlatformVersion { get; set; }
        /// <summary>
        /// Framework
        /// </summary>
        [JsonProperty(PropertyName = "framework")]
        public string Framework { get; set; }
        /// <summary>
        /// Installed framework version
        /// </summary>
        [JsonProperty(PropertyName = "framework_installed_version")]
        public string FrameworkInstalledVersion { get; set; }
        /// <summary>
        /// Newest framework version
        /// </summary>
        [JsonProperty(PropertyName = "framework_newest_version")]
        public string FrameworkNewestVersion { get; set; }
    }
}
