using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Anexia.Monitoring.Models;

namespace Anexia.Monitoring
{
    /// <summary>
    ///     Start point for initializing the version monitoring:
    ///     1) set access token (-> SetAccessToken function)
    ///     2) register the needed monitoring routes before adding the default MVC routes (RegisterServiceStateMonitor and/or
    ///     RegisterModulesInfoMonitor function)
    /// </summary>
    public static class VersionMonitor
    {
        /// <summary>
        ///     the major version number of this assembly - needed for creating the monitoring-routes
        /// </summary>
        private static string _assemblyVersion;

        /// <summary>
        ///     Gets the token for accessing the apis
        /// </summary>
        internal static string AccessToken { get; private set; }

        /// <summary>
        ///     Gets the blacklist for modules starts with not should been loaded
        /// </summary>
        internal static List<string> AdditionalBlackList { get; private set; } =
            new List<string>();

        /// <summary>
        ///     Gets the blacklist for modules starts with not should been loaded
        /// </summary>
        internal static List<string> BlackList { get; private set; } =
            new List<string>
            {
                "^[App_Web]",
                "^[CompiledRazorTemplates]",
                "^[System.]"
            };

        /// <summary>
        ///     Gets the assembly that is accessing this library
        /// </summary>
        internal static Assembly CallingAssembly
        {
            get => _callingAssembly == null ? Assembly.GetEntryAssembly() : _callingAssembly;
            private set => _callingAssembly = value;
        }

        /// <summary>
        ///     Gets or sets the assembly that is accessing this library
        /// </summary>
        private static Assembly _callingAssembly;

        /// <summary>
        ///     Gets or sets function to check if the database is running - moved logic to client to keep version
        ///     monitor platform independent
        /// </summary>
        internal static Func<bool> CheckDatabaseFunction { get; set; }

        /// <summary>
        ///     Gets or sets function to check if custom services are running
        /// </summary>
        internal static Func<List<ServiceState>> CheckCustomServicesFunction { get; set; }

        /// <summary>
        ///     Set the access token for the monitoring APIs
        /// </summary>
        /// <param name="accessToken">
        ///     the token to allow access to the monitoring routes - must be send as query-param with each
        ///     api-call
        /// </param>
        public static void SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        ///     Sets the blacklist for the monitoring APIs
        /// </summary>
        /// <param name="blackList">the list of module-prefixes not allowed to load in list</param>
        public static void SetBlackList(List<string> blackList)
        {
            BlackList = blackList ?? new List<string>();
        }

        /// <summary>
        ///     Sets the additionalBlackList the monitoring APIs
        /// </summary>
        /// <param name="additionalBlackList">the list of module-prefixes on Top not allowed to load in list</param>
        public static void SetAdditionalBlackList(List<string> additionalBlackList)
        {
            AdditionalBlackList = additionalBlackList ?? new List<string>();
        }

        /// <summary>
        ///     Registers route to call service state monitor - make sure this is called before adding mvc default routing
        ///     route: "/anxapi/v[VERSIONMONITOR-VERSION]/up?access_token=[TOKEN]"
        /// </summary>
        /// <param name="routes">The application's route collection.</param>
        /// <param name="checkDatabaseFunction">function to check if database is running</param>
        /// <param name="checkCustomServicesFunction">function to check if custom services are running</param>
        public static void RegisterServiceStateMonitor(
            RouteCollection routes,
            Func<bool> checkDatabaseFunction = null,
            Func<List<ServiceState>> checkCustomServicesFunction = null)
        {
            CheckCustomServicesFunction = checkCustomServicesFunction;
            CheckDatabaseFunction = checkDatabaseFunction;

            routes.MapRoute(
                "anxservice", // route name
                "anxapi/v" + GetVersionNumber() + "/up", // template
                new { controller = "Monitoring", action = "GetServiceStates" }, // defaults
                null, // constraints
                new[] { "VersionMonitorNetCore.Controllers" }); // namespaces
        }

        /// <summary>
        ///     Registers route to call runtime & modules monitor - make sure this is called before adding mvc default routing
        ///     route: "/anxapi/v[VERSIONMONITOR-VERSION]/modules?access_token=[TOKEN]"
        /// </summary>
        /// <param name="routes">The application's route collection.</param>
        /// <param name="currentAssembly">The current assembly.</param>
        public static void RegisterModulesInfoMonitor(RouteCollection routes, Assembly currentAssembly)
        {
            CallingAssembly = currentAssembly;

            routes.MapRoute(
                "anxruntime", // route name
                "anxapi/v" + GetVersionNumber() + "/modules", // template
                new { controller = "Monitoring", action = "GetModulesInfo" }, // defaults
                null, // constraints
                new[] { "VersionMonitorNetCore.Controllers" }); // namespaces
        }

        /// <summary>
        ///     Get the version of the current dll (VersionMonitor)
        /// </summary>
        /// <returns>The assembly's version number.</returns>
        private static string GetVersionNumber()
        {
            if (string.IsNullOrWhiteSpace(_assemblyVersion))
            {
                // only major version number is needed
                _assemblyVersion = typeof(VersionMonitor).Assembly.GetName().Version.Major.ToString();
            }

            return _assemblyVersion;
        }
    }
}