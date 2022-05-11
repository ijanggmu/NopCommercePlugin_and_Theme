using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.BannerPlugin
{
    public class BannerPluginSettings : ISettings
    {
        public string Message { get; set; }
        public int PictureId { get; set; }

    }
}