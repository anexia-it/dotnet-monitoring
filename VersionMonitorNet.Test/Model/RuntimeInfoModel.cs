using System.Collections.Generic;
using Anexia.Monitoring.Models;

namespace VersionMonitorNet.Test.Model
{
#pragma warning disable SA1313
    public record RuntimeInfoModel(RuntimeInfo Runtime, List<ModuleInfo> Modules);
#pragma warning restore SA1313
}