using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web;
using System.Web.Caching;

namespace CCHMC.Core.Web.Scrambler.Settings
{
    /// <summary>
    /// The attribute used to mark Web API controllers which should scramble their GET results. It can also be registered for all controllers in the Global.asax.
    /// </summary>
    public class ScramblerWebApiSessionSwitchAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The name used in the query string to toggle obfuscation.
        /// </summary>
        internal string SwitchName { get; set; }
        
        /// <summary>
        /// Enables the query to toggle obfuscation for the WebApi controller(s).
        /// </summary>
        /// <param name="switchName">The string which can be put in the query and set to "true" or "false" to enable or disable obfuscation.</param>
        public ScramblerWebApiSessionSwitchAttribute (string switchName)
        {
            switchName = switchName.Trim();
            if (String.IsNullOrWhiteSpace(switchName) || switchName.IndexOfAny(@":/?#[]@!$&'()*+,;=".ToCharArray()) >= 0)
            {
                throw new ArgumentException("Must be a string which could be read in a query! (Switch name cannot be an empty string or contain any of the following characters: \":/?#[]@!$&'()*+,;=\")");
            }
            SwitchName = switchName;
        }
        
        /// <summary>
        /// Processes the request and activates or deactivates obfuscation as appropriate.
        /// </summary>
        /// <param name="filterContext">The current action context.</param>
        public override void OnActionExecuting (HttpActionContext actionExecutedContext)
        {
            var vals = actionExecutedContext.Request.GetQueryNameValuePairs().ToDictionary(t => t.Key, t => t.Value);
            if (vals.ContainsKey(SwitchName))
            {
                ObfuscationSettings.SetCookie(vals[SwitchName]);
            }
        }
    }
}
