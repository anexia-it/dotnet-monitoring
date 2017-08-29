using System.Collections.Generic;

namespace Anexia.Monitoring.Models
{
    /// <summary>
    /// Dto for the package json object returned by nuget
    /// </summary>
    internal class NugetPackageJson
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public List<NugetPackageVersionJson> Versions { get; set; }
    }    
}
