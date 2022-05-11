using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.BannerPlugin.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.BannerPlugin.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class BannerPluginController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        public BannerPluginController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var MyBannerPluginSettings = await _settingService.LoadSettingAsync<BannerPluginSettings>(storeScope);
            var model = new ConfigurationModel
            {
                Message = MyBannerPluginSettings.Message,
                PictureId = MyBannerPluginSettings.PictureId,
            };
            if (storeScope > 0)
            {
                model.PictureId_OverrideForStore = await _settingService.SettingExistsAsync(MyBannerPluginSettings, x => x.PictureId, storeScope);
            }
            return View("~/Plugins/Widgets.BannerPlugin/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var MyBannerPluginSettings = await _settingService.LoadSettingAsync<BannerPluginSettings>(storeScope);

            var previousPictureIds = new[]
            {
                MyBannerPluginSettings.PictureId
            };

            MyBannerPluginSettings.Message = model.Message;
            MyBannerPluginSettings.PictureId = model.PictureId;


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingAsync(MyBannerPluginSettings, x => x.Message, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(MyBannerPluginSettings, x => x.PictureId, model.PictureId_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            var currentPictureIds = new[]
           {
                MyBannerPluginSettings.PictureId
           };

            foreach (var pictureId in previousPictureIds.Except(currentPictureIds))
            {
                var previousPicture = await _pictureService.GetPictureByIdAsync(pictureId);
                if (previousPicture != null)
                    await _pictureService.DeletePictureAsync(previousPicture);
            }

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
            return await Configure();
        }
    }
}