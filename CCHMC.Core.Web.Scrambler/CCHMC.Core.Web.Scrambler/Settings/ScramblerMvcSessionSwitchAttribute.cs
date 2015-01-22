using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CCHMC.Core.Web.Scrambler.Settings
{
    /// <summary>
    /// The attribute used to mark MVC controllers and functions which should scramble their results. It can also be registered for all controllers in the Global.asax.
    /// </summary>
    public class ScramblerMvcSessionSwitchAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The name used in the query string to toggle obfuscation.
        /// </summary>
        internal string SwitchName { get; set; }

        /// <summary>
        /// Enables the query to toggle obfuscation for the MVC controller(s).
        /// </summary>
        /// <param name="switchName">The string which can be put in the query and set to "true" or "false" to enable or disable obfuscation.</param>
        public ScramblerMvcSessionSwitchAttribute (string switchName)
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
        public override void OnActionExecuting (ActionExecutingContext filterContext)
        {
            var donotscramble = filterContext.HttpContext.Request.QueryString.Get(SwitchName);
            if (donotscramble != null)
            {
                ObfuscationSettings.SetCookie(donotscramble);
            }
        }
    }
}
