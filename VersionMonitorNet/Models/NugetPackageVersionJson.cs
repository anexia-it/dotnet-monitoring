using Newtonsoft.Json;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    ///     Dto for the package-version json object returned by nuget
    /// </summary>
    internal class NugetPackageVersionJson
    {
        /// <summary>
        ///     Gets or sets the version of the nuget package
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}