using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using VersionMonitorNet.Models;

namespace VersionMonitorNet
{
    /// <summary>
    /// Startpoint for initializing the version monitoring:
    /// 1) set access token
    /// 2) register the needed monitoring routes before adding the default MVC routes
    /// </summary>
    public static class VersionMonitor
    {
        /// <summary>
        /// the major version number of this assembly - needed for creating the monitoring-routes
        /// </summary>
        private static string _assemblyVersion = null;

        /// <summary>
        /// token for accessing the apis
        /// </summary>
        internal static string AccessToken { get; private set; }
        /// <summary>
        /// the assembly that is accessing this library
        /// </summary>
        internal static Assembly CallingAssembly { get; private set; }
        /// <summary>
        /// function to check if the database is running - moved logic to client to keep version monitor platform independent
        /// </summary>
        internal static Func<bool> CheckDatabaseFunction { get; set; }
        /// <summary>
        /// function to check if custom services are running
        /// </summary>
        internal static Func<List<ServiceState>> CheckCustomServicesFunction { get; set; }

        /// <summary>
        /// Set the access token for the monitoring APIs
        /// </summary>
        /// <param name="accessToken"></param>
        public static void SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// Register route to call service state monitor - make sure this is called before adding mvc default routing
        /// </summary>
        /// <param name="app">IApplicationBuilder to map routes</param>
        /// <param name="checkDatabaseFunction">function to check if database is running</param>
        /// <param name="checkCustomServicesFunction">function to check if custom services are running</param>
        public static void RegisterServiceStateMonitor(RouteCollection routes, Func<bool> checkDatabaseFunction, Func<List<ServiceState>> checkCustomServicesFunction = null)
        {
            CheckCustomServicesFunction = checkCustomServicesFunction;
            CheckDatabaseFunction = checkDatabaseFunction;

            routes.MapRoute(
                 "anxservice", //route name
                  ("anxapi/v" + GetVersionNumber() + "/up"), // template
                  new { controller = "Monitoring", action = "GetServiceStates" }, // defaults
                  null, // constraints
                  new[] { "VersionMonitorNetCore.Controllers" } //namespaces
            );
        }

        /// <summary>
        /// Register route to call runtime & modules monitor - make sure this is called before adding mvc default routing
        /// </summary>
        /// <param name="app">IApplicationBuilder to map routes</param>
        public static void RegisterModulesInfoMonitor(RouteCollection routes, Assembly currentAssembly)
        {
            CallingAssembly = currentAssembly;

            routes.MapRoute(
                  "anxruntime",
                  ("anxapi/v" + GetVersionNumber() + "/modules"),
                  new { controller = "Monitoring", action = "GetModulesInfo" },
                  null,
                  new[] { "VersionMonitorNetCore.Controllers" }
            );
        }

        private static string GetVersionNumber()
        {
            if (String.IsNullOrWhiteSpace(_assemblyVersion))
            {
                // only major version number is needed
                _assemblyVersion = typeof(VersionMonitorNet.VersionMonitor).Assembly.GetName().Version.Major.ToString();
            }

            return _assemblyVersion;
        }
    }
}
