using System.Collections.Generic;
using Anexia.Monitoring.Models;

namespace VersionMonitorNet.Test.Model
{
    public record RuntimeInfoModel(RuntimeInfo Runtime, List<ModuleInfo> Modules);
}