namespace VersionMonitorNet.Models
{
    /// <summary>
    /// Dto for the package-version json object returned by nuget
    /// </summary>
    internal class NugetPackageVersionJson
    {
        public string version { get; set; }
    }
}
