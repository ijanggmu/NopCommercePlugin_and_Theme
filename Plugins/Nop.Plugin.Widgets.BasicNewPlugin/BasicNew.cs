using Nop.Services.Plugins;
using System;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BasicNewPlugin
{
    public class BasicNew:BasePlugin
    {
        public override Task InstallAsync()
        {
            //Logic during installation goes here...

            return base.InstallAsync();
        }

        public override Task UninstallAsync()
        {
            //Logic during uninstallation goes here...

            return base.UninstallAsync();
        }

    }
}
