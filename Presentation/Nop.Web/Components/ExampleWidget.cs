using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    [ViewComponent(Name = "HelloWorldWidget")]
    public class ExampleWidgetViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            return Content("Hello World");
        }
    }
}
