using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CCHMC.Core.Web.Scrambler.Demo.Models;
using CCHMC.Core.Web.Scrambler.Settings;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Test.TestModels;

namespace CCHMC.Core.Web.Scrambler.Demo.Controllers
{
    public class HomeController : Controller
    {
        //[Scrambler]
        public ActionResult Index ()
        {
            return View();
        }

        public ActionResult About ()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        
        public ActionResult Contact ()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SpeedTest (SpeedResults result)
        {
            return View(result);
        }
        
        public ActionResult SimpleSpeedTest ()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            ResultsController rc = new ResultsController();
            
            ObfuscationSettings.ScrambleActiveCookie = true;
            DateTime start = DateTime.Now;
            for (int i=0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            ObfuscationSettings.ScrambleActiveCookie = false;
            start = DateTime.Now;
            for (int i=0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);

            return View(results);
        }
        
        public ActionResult ComplexSpeedTest ()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            ClassController cc = new ClassController();
            
            ObfuscationSettings.ScrambleActiveCookie = true;
            DateTime start = DateTime.Now;
            for (int i=0; i < results.NumberOfExecutions; i++)
            {
                cc.GetClass();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            ObfuscationSettings.ScrambleActiveCookie = false;
            start = DateTime.Now;
            for (int i=0; i < results.NumberOfExecutions; i++)
            {
                cc.GetClass();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);


            return View(results);
        }
        
        public ActionResult NestedSpeedTest ()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            RecursiveController rc = new RecursiveController();
            
            ObfuscationSettings.ScrambleActiveCookie = true;
            DateTime start = DateTime.Now;
            for (int i=0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            ObfuscationSettings.ScrambleActiveCookie = false;
            start = DateTime.Now;
            for (int i=0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);

            return View(results);
        }

        /* Speed tests for the old version of the Attribute Register.
         * Todo: Update for the current version.
        public ActionResult SimpleRegisterSpeedTest()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            BasicController rc = new BasicController();

            ObfuscationSettings.ScrambleActiveCookie = true;

            DateTime start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);

            Type t = typeof(BasicClass);
            for (int i = 0; i < 1000; i++)
            {
                ScrambleRegister.Register(t);
                ScrambleRegister.Register(t, i.ToString());
                t = typeof(List<>).MakeGenericType(t);
            }

            start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            return View(results);
        }

        public ActionResult LargeRegisterSpeedTest()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            BasicController rc = new BasicController();

            ObfuscationSettings.ScrambleActiveCookie = true;

            DateTime start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);

            Type t = typeof(BasicClass);
            for (int i = 0; i < 1000000; i++)
            {
                ScrambleRegister.RegisterType(t);
                ScrambleRegister.RegisterMember(t, i.ToString());
                t = new FakeType(i);
            }

            start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            ObfuscationSettings.ScrambleActiveCookie = false;

            return View(results);
        }

        public ActionResult SimpleRegisterMemberSpeedTest()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            BasicController rc = new BasicController();

            ObfuscationSettings.ScrambleActiveCookie = true;

            DateTime start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);

            Type t = typeof(BasicClass);
            ScrambleRegister.RegisterMember(t, "Name");
            for (int i = 0; i < 1000; i++)
            {
                t = new FakeType(i);
                ScrambleRegister.RegisterType(t);
                ScrambleRegister.RegisterMember(t, i.ToString());
            }

            start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            return View(results);
        }

        public ActionResult LargeRegisterMemberSpeedTest()
        {
            SpeedResults results = new SpeedResults();
            results.NumberOfExecutions = 1000000000;
            BasicController rc = new BasicController();

            ObfuscationSettings.ScrambleActiveCookie = true;

            DateTime start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.UnobfuscatedTime = DateTime.Now.Subtract(start);

            Type t = typeof(BasicClass);
            ScrambleRegister.RegisterMember(t, "Name");
            for (int i = 0; i < 1000000; i++)
            {
                t = new FakeType(i);
                ScrambleRegister.RegisterType(t);
                ScrambleRegister.RegisterMember(t, i.ToString());
            }

            start = DateTime.Now;
            for (int i = 0; i < results.NumberOfExecutions; i++)
            {
                rc.Get();
            }
            results.ObfuscatedTime = DateTime.Now.Subtract(start);

            ObfuscationSettings.ScrambleActiveCookie = false;

            return View(results);
        }
        */
        //[MvcScrambler]
        public ActionResult Demo ()
        {
            var test = new DisplayAttributeTest() { 
                thing1 = "Cat", thing2 = "Hat",
                temporal = new DateTime(1992, 1, 27, 5, 55, 15)
            };

            var a = test.temporal;

            var props = typeof(DisplayAttributeTest).GetProperties();

            var p = typeof(DisplayAttributeTest).GetProperty("temporal").GetCustomAttribute(typeof(DisplayFormatAttribute)) as DisplayFormatAttribute;

            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    DisplayFormatAttribute dfa = attr as DisplayFormatAttribute;
                }
            }

            return View(test);
        }

    }
}