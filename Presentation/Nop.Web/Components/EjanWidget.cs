using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Components
{
    [ViewComponent(Name = "EjanPlugin")]
    public class EjanWidgetViewComponent:NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            return Content("Ejan Shrestha Plugin");
        }
    }
}
