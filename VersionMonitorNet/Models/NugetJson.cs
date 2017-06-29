using System.Collections.Generic;

namespace VersionMonitorNet.Models
{
    /// <summary>
    /// Dto for the json object returned by nuget
    /// </summary>
    internal class NugetJson
    {
        public List<NugetPackageJson> data { get; set; }
    }
}
