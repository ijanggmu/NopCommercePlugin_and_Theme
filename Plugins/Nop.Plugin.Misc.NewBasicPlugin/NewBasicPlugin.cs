using Nop.Services.Plugins;
using System;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.NewBasicPlugin
{
    public class NewBasicPlugin:BasePlugin
    {
        public override Task InstallAsync()
        {
            return base.InstallAsync();
        }
        public override Task UninstallAsync()
        {
            return base.UninstallAsync();
        }
    }
}
