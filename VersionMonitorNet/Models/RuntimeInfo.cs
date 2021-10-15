using Newtonsoft.Json;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    ///     Dto for the runtime info
    /// </summary>
    [JsonObject(Title = "runtime")]
    public class RuntimeInfo
    {
        /// <summary>
        ///     Gets or sets platform
        /// </summary>
        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        /// <summary>
        ///     Gets or sets platform version
        /// </summary>
        [JsonProperty(PropertyName = "platform_version")]
        public string PlatformVersion { get; set; }

        /// <summary>
        ///     Gets or sets framework
        /// </summary>
        [JsonProperty(PropertyName = "framework")]
        public string Framework { get; set; }

        /// <summary>
        ///     Gets or sets installed framework version
        /// </summary>
        [JsonProperty(PropertyName = "framework_installed_version")]
        public string FrameworkInstalledVersion { get; set; }

        /// <summary>
        ///     Gets or sets newest framework version
        /// </summary>
        [JsonProperty(PropertyName = "framework_newest_version")]
        public string FrameworkNewestVersion { get; set; }
    }
}