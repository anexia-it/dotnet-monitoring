using Newtonsoft.Json;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Anexia.Monitoring.Models;

namespace Anexia.Monitoring.Services
{
    /// <summary>
    /// Provides monitoring methods
    /// </summary>
    internal class MonitoringService : IDisposable
    {
        /// <summary>
        /// Url for querying the nuget packages
        /// </summary>
        private const string NUGET_URL_PREFIX = "https://api-v2v3search-0.nuget.org/query?q=packageid:";
        /// <summary>
        /// Url for querying the netframework releases
        /// </summary>
        private const string NET_RELEASES_INDEX = "https://raw.githubusercontent.com/microsoft/dotnet/master/releases/README.md";
        /// <summary>
        /// Client for calling the nuget-API
        /// </summary>
        private HttpClient _client = new HttpClient();

        /// <summary>
        /// Checks if the services are running (database and optional custom services)
        /// </summary>
        /// <returns></returns>
        internal string GetServiceStates()
        {
            bool success = true;

            // check if database is running
            if (VersionMonitor.CheckDatabaseFunction != null && !VersionMonitor.CheckDatabaseFunction())
                success = false;

            // check if custom services are running
            if (VersionMonitor.CheckCustomServicesFunction != null)
            {
                foreach (var result in VersionMonitor.CheckCustomServicesFunction())
                {
                    if (!result.IsRunning)
                    {
                        success = false;
                        break;
                    }
                }
            }

            return (success ? "OK" : "NOK");
        }

        /// <summary>
        /// Gets the runtime and modules info
        /// </summary>
        /// <returns></returns>
        internal async Task<dynamic> GetModulesInfo()
        {
            return new
            {
                runtime = await GetRuntime(),
                modules = await GetModules()
            };
        }

        #region Runtime helper

        /// <summary>
        /// get runtime info
        /// </summary>
        /// <returns></returns>
        private async Task<RuntimeInfo> GetRuntime()
        {
            // get current framework and version
            var framework = new FrameworkName(VersionMonitor.CallingAssembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName);

            // try to convert to semantic version
            string frameworkVersion = framework.Version.ToString();
            SemanticVersion semanticVersion;
            SemanticVersion.TryParse(frameworkVersion, out semanticVersion);

            if (semanticVersion != null)
                frameworkVersion = semanticVersion.ToString();

            var content = await GetWebContentString(NET_RELEASES_INDEX);

            MatchCollection matches = Regex.Matches(content, @"(?<=- \[\.NET Framework )[0-9.]*");
            var latestVersion = matches.Count > 0 ? matches[0].Value : null;

            return new RuntimeInfo
            {
                // fix value, this application is only used for .NET Framework
                Platform = "dotnet",
                PlatformVersion = frameworkVersion,
                Framework = framework.Identifier,
                FrameworkInstalledVersion = frameworkVersion,
                FrameworkNewestVersion = latestVersion
            };
        }

        /// <summary>
        /// get info about modules
        /// </summary>
        /// <returns></returns>
        private async Task<List<ModuleInfo>> GetModules()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>();
            string entryAssemblyName = VersionMonitor.CallingAssembly.GetName().Name.ToLower();
            var libraries = AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName);

            foreach (var library in libraries)
            {
                AssemblyName assemblyName = library.GetName();
                // no need to display executing assembly
                if (assemblyName.Name == entryAssemblyName)
                    continue;
                if (!VersionMonitor.BlackList.Exists(x => Regex.Match(assemblyName.Name, x).Success)
                   && !VersionMonitor.AdditionalBlackList.Exists(x => Regex.Match(assemblyName.Name, x).Success))
                {
                    // try to convert to semantic version
                    string assemblyVersion = assemblyName.Version.ToString();
                    SemanticVersion semanticVersion;
                    SemanticVersion.TryParse(assemblyVersion, out semanticVersion);

                    if (semanticVersion != null)
                        assemblyVersion = semanticVersion.ToString();

                    modules.Add(new ModuleInfo()
                    {
                        Name = assemblyName.Name,
                        InstalledVersion = assemblyVersion,
                        // if module is a nuget package, get the newest version from nuget
                        NewestVersion = (library.GlobalAssemblyCache ? assemblyVersion : await GetNewestModuleVersion(assemblyName.Name, assemblyVersion)), // todo: get newest version for modules from global assembly cache
                        //if module is a nuget package, get License
                        Licenses = (library.GlobalAssemblyCache ? new List<string>() : await GetLicense(assemblyName.Name))
                    });
                }
            }
            return modules;
        }

        /// <summary>
        /// Query nuget with package name to get newest version
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="installedVersion"></param>
        /// <returns></returns>
        private async Task<string> GetNewestModuleVersion(string libraryName, string installedVersion)
        {
            var nugetJson = await GetWebContent<NugetJson>(NUGET_URL_PREFIX + libraryName);
            if (nugetJson != null && nugetJson.data.Count > 0)
            {
                var nugetVersions = nugetJson.data.First().Versions;
                // try to convert to semantic version
                SemanticVersion maxVersion = null;
                foreach (var nugetVersion in nugetVersions)
                {
                    // check if a newer version exists
                    SemanticVersion currentVersion;
                    if (SemanticVersion.TryParse(nugetVersion.version, out currentVersion))
                        maxVersion = (maxVersion == null || currentVersion > maxVersion) ? currentVersion : maxVersion;
                }

                if (maxVersion != null)
                    return maxVersion.ToString();
            }

            return installedVersion;
        }

        /// <summary>
        /// Query nuget with package name to get License
        /// </summary>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        private async Task<List<string>> GetLicense(string libraryName)
        {
            var nugetJson = await GetWebContent<NugetJson>(NUGET_URL_PREFIX + libraryName);
            return nugetJson != null ? nugetJson.data.Any() ? new List<string> { nugetJson.data[0].licenseUrl } : new List<string>() : new List<string>();
        }

        /// <summary>
        ///     Queries a webpage and returns it's content converted to the given format
        /// </summary>
        /// <param name="url">The url to call.</param>
        /// <returns>Task containing the webpage's deserialized content.</returns>
        private async Task<T> GetWebContent<T>(string url)
        {
            // read response content
            var stringResponse = await GetWebContentString(url);

            // convert json to dto
            return JsonConvert.DeserializeObject<T>(stringResponse);
        }

        /// <summary>
        ///     Queries a webpage and returns it's content converted as string
        /// </summary>
        /// <param name="url">The url to call.</param>
        /// <returns>Task containing the webpage's content.</returns>
        private async Task<string> GetWebContentString(string url)
        {
            var response = await _client.GetAsync(url);

            // status code verification
            response.EnsureSuccessStatusCode();

            // read response content
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        #region Token validation

        /// <summary>
        /// Check if access token has been provided in Initialize-method
        /// </summary>
        /// <returns></returns>
        public bool CheckTokenConfiguration()
        {
            return !String.IsNullOrWhiteSpace(VersionMonitor.AccessToken);
        }

        /// <summary>
        /// Check if the access token from the query matches the one from the configuration
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public bool ValidateToken(string accessToken)
        {
            return VersionMonitor.AccessToken == accessToken;
        }

        #endregion

        public void Dispose()
        {
            _client.Dispose();
            _client = null;
        }
    }
}
