using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace CCHMC.Core.Web.Scrambler.Settings
{
    /// <summary>
    /// Stores settings for the website.
    /// </summary>
    public static class ObfuscationSettings
    {
        //This is needed for obfuscation to be usable outside of web applications; notably, it is needed for unit tests, though console and other applications may also require its use.
        /// <summary>
        /// The value used to determine whether or not the Scrambler should be on if the other settings are not set.
        /// </summary>
        public static bool IsActiveDefault = false;

        /// <summary>
        /// Key for the session which determines whether or not obfuscation is active for the site for this user session.
        /// </summary>
        public static readonly string ActiveSession = "IsScramblerActive";

        /// <summary>
        /// Accesses or modifies the cookie which sets whether or not the obfuscation should be active.
        /// </summary>
        //todo: This should probably be reviewed to make sure it's not bad design...
        public static bool? ScrambleActiveCookie
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var scramCookie = HttpContext.Current.Request.Cookies.Get(ActiveSession);
                    return scramCookie == null ? null : (bool?)(scramCookie.Value == "true");
                } else
                {
                    return null;
                }
            }
            set
            {
                //Should this check the context? 
                //It might be better if it throws an exception if it can't set the value to avoid it looking like it sets even when it doesn't.
                if (HttpContext.Current != null)
                {
                    //Create the cookie
                    HttpCookie cookie = (value == null) ? null : new HttpCookie(ActiveSession, (bool)value ? "true" : "false");
                    //If the cookie exists, set it in the request and response (so it applies immediately and later); otherwise, append to both.
                    if (HttpContext.Current.Request.Cookies.Get(ActiveSession) == null)
                    {
                        HttpContext.Current.Response.AppendCookie(cookie);
                        HttpContext.Current.Request.Cookies.Add(cookie);
                    } else
                    {
                        HttpContext.Current.Response.SetCookie(cookie);
                        HttpContext.Current.Request.Cookies.Set(cookie);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the cookie value to true or false, or does not change it, depending on the string.
        /// </summary>
        /// <param name="val">The string for setting the cookie; will set the cookie to True if the string is "true"; false if it is "false"; and will not change it otherwise.</param>
        /// <returns>The value of the cookie after modifying the value if necessary.</returns>
        internal static bool? SetCookie(string val)
        {
            if (val.ToLower() == "true")
            {
                ScrambleActiveCookie = true;
            } else if (val.ToLower() == "false")
            {
                ScrambleActiveCookie = false;
            }
            return ScrambleActiveCookie;
        }

        /// <summary>
        /// Determines whether or not obfuscation is active for the site or user session.
        /// </summary>
        public static bool IsActive
        {
            get
            {
                return ScrambleActiveCookie ?? IsActiveDefault;
            }
        }
    }
}
