using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BannerPlugin.Models
{
    public record PublicInfoModel : BaseNopModel
    {
        public string Message { get; set; }
        public string PictureUrl { get; set; }
       
    }
}