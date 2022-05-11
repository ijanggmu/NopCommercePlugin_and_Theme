using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BannerPlugin.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BannerPlugin.Message")]
        [UIHint("Message")]
        public string Message { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BannerPlugin.Picture")]
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public bool PictureId_OverrideForStore { get; set; }
        
    }
}