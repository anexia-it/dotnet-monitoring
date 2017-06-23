using System.Collections.Generic;

namespace VersionMonitorNet.Models
{
    internal class NugetPackageJson
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public List<NugetPackageVersionJson> Versions { get; set; }
    }    
}
