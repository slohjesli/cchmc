using ScramblerPresentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScramblerPresentation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(InternalHelper.AllThePeople.First());
        }

        public ActionResult Summary()
        {
            return View(InternalHelper.Index);
        }
    }
}