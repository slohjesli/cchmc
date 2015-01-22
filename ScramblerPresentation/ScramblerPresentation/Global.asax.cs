using ExternalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ScramblerPresentation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Initializes the Scramblers for the application. 
            //Once initialized here, it will automatically scramble all data pulled through the WebAPI and through views and JSON.
            //It will ignore any attributes with the Ignore flag set or the Ignore attribute applied, and will use any other Scramble attributes applied instead of the default if any exist.
            //GlobalFilters.Filters.Add(new ScramblerMvcAttribute());
            //GlobalConfiguration.Configuration.Filters.Add(new ScramblerWebApiAttribute());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Registers external class member.
            //ScrambleRegister.Ignore<ExternalClass>(t => t.Initial);

            //Sets the global switch, so it can be permanently on in demos to prevent switching back.
            //ObfuscationSettings.IsActiveDefault = true;

            //Adds the Switch setting, which allows queries at the end of URIs to activate or deactivate the scrambler.
            //The variable in the query used for the setting is the string passed in, which here would make the query ?Scramble=true or ?Scramble=false
            //GlobalFilters.Filters.Add(new ScramblerMvcSessionSwitchAttribute("Scramble"));
            //GlobalConfiguration.Configuration.Filters.Add(new ScramblerWebApiSessionSwitchAttribute("Scramble"));
        }
    }
}
