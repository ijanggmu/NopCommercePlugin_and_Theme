using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.BannerPlugin.Infrastructure.Cache;
using Nop.Plugin.Widgets.BannerPlugin.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BannerPlugin.Components
{
    [ViewComponent(Name = "WidgetsBannerPlugin")]
    public class WidgetsBannerPluginViewComponent : NopViewComponent
    {
        public static string ViewComponentName => "WidgetsBannerPlugin";
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWebHelper _webHelper;

        public WidgetsBannerPluginViewComponent(IStoreContext storeContext, 
            ISettingService settingService, 
            IPictureService pictureService,
            IStaticCacheManager staticCacheManager,
            IWebHelper webHelper)
        {
            _storeContext = storeContext;
            _settingService = settingService;
            _pictureService = pictureService;
            _staticCacheManager = staticCacheManager;
            _webHelper = webHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var BannerPluginSettings = await _settingService.LoadSettingAsync<BannerPluginSettings>( _storeContext.GetCurrentStoreAsync().Id);

            var model = new PublicInfoModel
            {
                Message = BannerPluginSettings.Message,
                PictureUrl = await GetPictureUrlAsync(BannerPluginSettings.PictureId),
            };

            if (string.IsNullOrEmpty(model.Message) && string.IsNullOrEmpty(model.PictureUrl))
                return Content("");

            return View("~/Plugins/Widgets.BannerPlugin/Views/PublicInfo.cshtml", model);
        }
        protected async Task<string> GetPictureUrlAsync(int pictureId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY,
                pictureId, _webHelper.IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                //little hack here. nulls aren't cacheable so set it to ""
                var url = await _pictureService.GetPictureUrlAsync(pictureId, showDefaultPicture: false) ?? "";
                return url;
            });
        }
    }
}
