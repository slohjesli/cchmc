using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Settings;

namespace CCHMC.Core.Web.Scrambler
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ScramblerWebApiAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Used to decorate fields and properties to generate replacement information for that field or property to conceal 
        /// sensitive data when previewing applications which may view the data.
        /// </summary>
        public ScramblerWebApiAttribute ()
            : base()
        {

        }

        public override void OnActionExecuted (HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null && actionExecutedContext.Response != null)
            {
                var obj = actionExecutedContext.Response.Content as ObjectContent;
                if (ObfuscationSettings.IsActive && obj != null)
                {
                    obj.Value = obj.Value.Obfuscate();
                    actionExecutedContext.Response.Content = obj;
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        public override void OnActionExecuting (System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
}
