using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    ///     Dto for the json object returned by nuget
    /// </summary>
    internal class NugetJson
    {
        /// <summary>
        ///     Gets or sets the data of the nuget package
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public List<NugetPackageJson> Data { get; set; }
    }
}