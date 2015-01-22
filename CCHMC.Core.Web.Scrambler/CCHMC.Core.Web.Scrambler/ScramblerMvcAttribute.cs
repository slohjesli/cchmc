using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CCHMC.Core.Web.Scrambler.Settings;
using CCHMC.Core.Web.Scrambler.Helpers;
using System.Web;

namespace CCHMC.Core.Web.Scrambler
{
    public class ScramblerMvcAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted (ActionExecutedContext filterContext)
        {            
            if (ObfuscationSettings.IsActive && filterContext != null)//(HttpContext.Current.Session[ObfuscationSettings.ActiveSession] as bool? ?? false))
            {
                var json = filterContext.Result as JsonResult;
                var view = filterContext.Result as ViewResultBase;

                if (json != null)
                {
                    json.Data = json.Data.Obfuscate(); //Data should be obfuscated in this case.
                } else if (view != null)
                {
                    view.ViewData.Model = view.Model.Obfuscate(); //Model should be obfuscated
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
