using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    ///     Dto for the package json object returned by nuget
    /// </summary>
    internal class NugetPackageJson
    {
        /// <summary>
        ///     Gets or sets the id of the nuget package
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the nuget package's current version
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        ///     Gets or sets the nuget package's license URL
        /// </summary>
        [JsonProperty(PropertyName = "licenseUrl")]
        public string LicenseUrl { get; set; }

        /// <summary>
        ///     Gets or sets the list of all versioned nuget packages
        /// </summary>
        [JsonProperty(PropertyName = "versions")]
        public List<NugetPackageVersionJson> Versions { get; set; }
    }
}