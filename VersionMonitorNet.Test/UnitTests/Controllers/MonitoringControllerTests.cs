using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anexia.Monitoring;
using Anexia.Monitoring.Controllers;
using Anexia.Monitoring.Models;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using VersionMonitorNet.Test.Model;
using Xunit;

namespace VersionMonitorNet.Test.UnitTests.Controllers
{
    public class MonitoringControllerTests
    {
        private readonly string _accestoken;
        private readonly MonitoringController _monitoringController;

        public MonitoringControllerTests()
        {
            _monitoringController = new MonitoringController();
            _accestoken = Guid.NewGuid().ToString();
            VersionMonitor.SetAccessToken(_accestoken);
        }

        [Fact]
        public void GetServiceStatesTest()
        {
            var result = (object)_monitoringController.GetServiceStates(_accestoken);
            if (result is ContentResult okObjectResult)
                Assert.True((string)okObjectResult.Content == "OK");
            else
                Assert.True(false, "Not a OkObjectResult");
        }

        [Fact]
        public async Task GetModulesInfoTest()
        {
            var result = await _monitoringController.GetModulesInfo(_accestoken);
            if (result is ContentResult okObjectResult)
            {
                var runtimeInfoModel = JsonConvert.DeserializeObject<RuntimeInfoModel>(okObjectResult.Content);
                if (runtimeInfoModel?.Runtime != null)
                {
                    var frameWorkVersion = PlatformServices.Default.Application.RuntimeFramework;
                    Assert.True(frameWorkVersion.Version.ToString() == runtimeInfoModel.Runtime.FrameworkInstalledVersion,
                        "Not a Actual version");
                }
                else
                {
                    Assert.True(false, "RuntimeInfo not existing");
                }
                
                if (runtimeInfoModel?.Modules != null)
                    Assert.NotEmpty(runtimeInfoModel.Modules);
                else
                    Assert.True(false, "List module info is not existing");
            }
            else
            {
                Assert.True(false, "Not a OkObjectResult or Value not DynamicObject");
            }
        }
    }
}