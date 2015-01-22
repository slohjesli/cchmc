using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Settings;
using System.Web;

namespace CCHMC.Core.Web.Scrambler.Test.Settings
{
    [TestClass]
    public class ScramblerMvcSessionSwitchAttributeUnitTest
    {
        [TestMethod]
        public void ConstructorEmptyString()
        {
            bool ae = false;
            try
            {
                new ScramblerMvcSessionSwitchAttribute(String.Empty);
            }
            catch (ArgumentException e)
            {
                ae = true;
            }
            Assert.IsTrue(ae, "Empty string passed to constructor did not cause an ArgumentException!");
        }

        [TestMethod]
        public void ConstructorInvalidString()
        {
            bool ae = false;
            char[] invalidchars = ":/?#[]@!$&'()*+,;=".ToCharArray();
            foreach (char c in invalidchars)
            {
                try
                {
                    new ScramblerMvcSessionSwitchAttribute("inval" + c + "d");
                }
                catch (ArgumentException e)
                {
                    ae = true;
                }
                Assert.IsTrue(ae, "Invalid string passed to constructor did not cause an ArgumentException! Invalid char: " + c);
            }
        }

        [TestMethod]
        public void ConstructorValidString()
        {
            var attr = new ScramblerMvcSessionSwitchAttribute("valid");
            Assert.AreEqual(attr.SwitchName, "valid");
        }

        [TestMethod]
        public void OnActionExecutingNoSwitch()
        {
            var attr = new ScramblerMvcSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?", null), new HttpResponse(null));
            var context = new HttpContextWrapper(HttpContext.Current);
            var aec = new ActionExecutingContext() { HttpContext = context as HttpContextBase };
            attr.OnActionExecuting(aec);
            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void OnActionExecutingInvalidSwitch()
        {
            var attr = new ScramblerMvcSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?valid=sdfm", "valid=sdfm"), new HttpResponse(null));
            var context = new HttpContextWrapper(HttpContext.Current);
            var aec = new ActionExecutingContext() { HttpContext = context as HttpContextBase };
            attr.OnActionExecuting(aec);
            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void OnActionExecutingTrueSwitch()
        {
            var attr = new ScramblerMvcSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?valid=true", "valid=true"), new HttpResponse(null));
            var context = new HttpContextWrapper(HttpContext.Current);
            var aec = new ActionExecutingContext() { HttpContext = context as HttpContextBase };
            attr.OnActionExecuting(aec);
            Assert.IsTrue((bool)ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void OnActionExecutingFalseSwitch()
        {
            var attr = new ScramblerMvcSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?valid=false", "valid=false"), new HttpResponse(null));
            var context = new HttpContextWrapper(HttpContext.Current);
            var aec = new ActionExecutingContext() { HttpContext = context as HttpContextBase };
            attr.OnActionExecuting(aec);
            Assert.IsFalse((bool)ObfuscationSettings.ScrambleActiveCookie);
        }
    }
}
