using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using VersionMonitorNet.Models;

namespace VersionMonitorNet.Services
{
    /// <summary>
    /// Provides monitoring methods
    /// </summary>
    internal class MonitoringService
    {
        /// <summary>
        /// Checks if the services are running (database and optional custom services)
        /// </summary>
        /// <returns></returns>
        internal string GetServiceStates()
        {
            StringBuilder builder = new StringBuilder();

            if (VersionMonitor.CheckDatabaseFunction == null)
                builder.AppendLine("Database check is not configured!");
            else
                builder.AppendLine(String.Format("Database connection: {0}", VersionMonitor.CheckDatabaseFunction() ? "OK" : "NOK"));

            if (VersionMonitor.CheckCustomServicesFunction != null)
            {
                foreach (var result in VersionMonitor.CheckCustomServicesFunction())
                    builder.AppendLine(String.Format("{0}: {1}", result.ServiceName, result.IsRunning ? "OK" : "NOK"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the runtime and modules info
        /// </summary>
        /// <returns></returns>
        internal dynamic GetModulesInfo()
        {
            return new
            {
                runtime = GetRuntime(),
                modules = GetModules()
            };
        }

        #region Runtime helper

        /// <summary>
        /// get runtime info
        /// </summary>
        /// <returns></returns>
        private RuntimeInfo GetRuntime()
        {
            var framework = new FrameworkName(VersionMonitor.CallingAssembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName);
            return new RuntimeInfo
            {
                // fix value, this application is only used for .NET Framework
                Platform = "dotnet",
                PlatformVersion = framework.Version.ToString(),
                Framework = framework.Identifier,
                FrameworkInstalledVersion = framework.Version.ToString(),
                FrameworkNewestVersion = null  //todo: get newest version
            };
        }

        /// <summary>
        /// get info about modules
        /// </summary>
        /// <returns></returns>
        private List<ModuleInfo> GetModules()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>();
            string entryAssemblyName = VersionMonitor.CallingAssembly.GetName().Name.ToLower();
            var libraries = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var library in libraries)
            {
                // no need to display executing assembly
                if (library.GetName().Name == entryAssemblyName)
                    continue;

                modules.Add(new ModuleInfo()
                {
                    Name = library.GetName().Name,
                    InstalledVersion = library.GetName().Version.ToString(),
                    NewestVersion = null // todo: newest module version
                });
            }

            return modules;
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
    }
}
