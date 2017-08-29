using System.Collections.Generic;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    /// Dto for the json object returned by nuget
    /// </summary>
    internal class NugetJson
    {
        public List<NugetPackageJson> data { get; set; }
    }
}
