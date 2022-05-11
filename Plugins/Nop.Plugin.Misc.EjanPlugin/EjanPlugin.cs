using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.EjanPlugin
{
    public class EjanPlugin:BasePlugin,IWidgetPlugin
    {
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "EjanPlugin";
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { "home_page_before_categories" });
        }

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
