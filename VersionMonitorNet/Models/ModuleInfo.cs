using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    ///     Dto for the module info
    /// </summary>
    [JsonObject(Title = "modules")]
    public class ModuleInfo
    {
        /// <summary>
        ///     Gets or sets module name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
 
        /// <summary>
        ///     Gets or sets installed version of the module
        /// </summary>
        [JsonProperty(PropertyName = "installed_version")]
        public string InstalledVersion { get; set; }

        /// <summary>
        ///     Gets or sets newest version of the module
        /// </summary>
        [JsonProperty(PropertyName = "newest_version")]
        public string NewestVersion { get; set; }

        /// <summary>
        ///     Gets or sets list of Licenses
        /// </summary>
        [JsonProperty(PropertyName = "licenses")]
        public List<string> Licenses { get; set; }
    }
}