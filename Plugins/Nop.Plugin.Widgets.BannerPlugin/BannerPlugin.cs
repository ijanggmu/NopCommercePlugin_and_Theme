using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.BannerPlugin;
using Nop.Plugin.Widgets.BannerPlugin.Components;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BannerPlugin
{
    public class BannerPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly INopFileProvider _fileProvider;
        public BannerPlugin(ISettingService settingService, IWebHelper webHelper, ILocalizationService localizationService, IPictureService pictureService, INopFileProvider fileProvider)
        {
            _webHelper = webHelper;
            _settingService = settingService;
            _localizationService = localizationService;
            _pictureService = pictureService;
            _fileProvider = fileProvider;
        }
        public bool HideInWidgetList => false;
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/BannerPlugin/Configure";
        }
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> {   PublicWidgetZones.HomepageBeforeNews
                });
        }
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsBannerPlugin";
        }
        public override async Task InstallAsync()
        {
            //pictures
            var sampleImagesPath = _fileProvider.MapPath("~/Plugins/Widgets.BannerPlugin/Content/images/");
            var settings = new BannerPluginSettings()
            {
                Message = "Type your caption here",
                PictureId = (await _pictureService.InsertPictureAsync(await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "banner.jpg")), MimeTypes.ImagePJpeg, "banner_1")).Id,
            };
            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Widgets.BannerPlugin.Message", "Message");
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.BannerPlugin.Picture"] = "Picture",
                ["Plugins.Widgets.BannerPlugin.Picture.Hint"] = "Upload picture.",
            });
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _localizationService.DeleteLocaleResourceAsync("Plugins.Widgets.BannerPlugin.Message");
            await _localizationService.DeleteLocaleResourceAsync("Plugins.Widgets.BannerPlugin.Picture");

            await base.UninstallAsync();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "BannerPlugin",
                Title = "BannerPlugin Title",
                ControllerName = "BannerPlugin",
                ActionName = "Configure",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }
    }
}