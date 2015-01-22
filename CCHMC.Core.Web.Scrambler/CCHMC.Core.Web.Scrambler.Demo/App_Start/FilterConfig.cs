using System.Web;
using System.Web.Mvc;

namespace CCHMC.Core.Web.Scrambler.Demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
