using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anexia.Monitoring;
using Anexia.Monitoring.Controllers;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using VersionMonitorNet.Test.Model;
using Xunit;

namespace VersionMonitorNet.Test.UnitTests.Controllers
{
    /// <summary>
    ///     Test class for monitoring controller tests
    /// </summary>
    public class MonitoringControllerTests
    {
        private readonly string _accessToken;
        private readonly MonitoringController _monitoringController;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MonitoringControllerTests"/> class.
        ///     This constructor will be called each time before any test method.
        /// </summary>
        public MonitoringControllerTests()
        {
            _monitoringController = new MonitoringController();
            _accessToken = Guid.NewGuid().ToString();
            VersionMonitor.SetAccessToken(_accessToken);
        }

        /// <summary>
        ///     Tests getting service states
        /// </summary>
        [Fact]
        public void GetServiceStatesTest()
        {
            var result = (object)_monitoringController.GetServiceStates(_accessToken);
            if (result is ContentResult okObjectResult)
            {
                Assert.True(okObjectResult.Content == "OK");
            }
            else
            {
                Assert.True(false, "Not a OkObjectResult");
            }
        }

        /// <summary>
        ///     Tests getting modules info
        /// </summary>
        /// <returns>Task containing the test.</returns>
        [Fact]
        public async Task GetModulesInfoTest()
        {
            var result = await _monitoringController.GetModulesInfo(_accessToken);
            if (result is ContentResult okObjectResult)
            {
                var runtimeInfoModel = JsonConvert.DeserializeObject<RuntimeInfoModel>(okObjectResult.Content);
                if (runtimeInfoModel?.Runtime != null)
                {
                    var frameWorkVersion = PlatformServices.Default.Application.RuntimeFramework;
                    Assert.True(
                        frameWorkVersion.Version.ToString() == runtimeInfoModel.Runtime.FrameworkInstalledVersion,
                        "Not a Actual version");
                }
                else
                {
                    Assert.True(false, "RuntimeInfo not existing");
                }

                if (runtimeInfoModel?.Modules != null)
                {
                    Assert.NotEmpty(runtimeInfoModel.Modules);
                }
                else
                {
                    Assert.True(false, "List module info is not existing");
                }
            }
            else
            {
                Assert.True(false, "Not a OkObjectResult or Value not DynamicObject");
            }
        }
    }
}