using System;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Settings;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Web;

namespace CCHMC.Core.Web.Scrambler.Test.Settings
{
    [TestClass]
    public class ScramblerWebApiSessionSwitchAttributeUnitTest
    {
        [TestMethod]
        public void ConstructorEmptyString()
        {
            bool ae = false;
            try
            {
                new ScramblerWebApiSessionSwitchAttribute(String.Empty);
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
                    new ScramblerWebApiSessionSwitchAttribute("inval" + c + "d");
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
            var attr = new ScramblerWebApiSessionSwitchAttribute("valid");
            Assert.AreEqual(attr.SwitchName, "valid");
        }

        [TestMethod]
        public void OnActionExecutingNoSwitch()
        {
            var attr = new ScramblerWebApiSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?", null), new HttpResponse(null));
            var context = new HttpControllerContext() { Request = new HttpRequestMessage(new HttpMethod("test"), "http://example.com?") };
            var hac = new HttpActionContext() { ControllerContext = context };
            attr.OnActionExecuting(hac);
            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void OnActionExecutingInvalidSwitch()
        {
            var attr = new ScramblerWebApiSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?valid=kjfng", "valid=kjfng"), new HttpResponse(null));
            var context = new HttpControllerContext() { Request = new HttpRequestMessage(new HttpMethod("test"), "http://example.com?valid=kjfng") };
            var hac = new HttpActionContext() { ControllerContext = context };
            attr.OnActionExecuting(hac);
            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void OnActionExecutingTrueSwitch()
        {
            var attr = new ScramblerWebApiSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?valid=true", "valid=true"), new HttpResponse(null));
            var context = new HttpControllerContext() { Request=new HttpRequestMessage(new HttpMethod("test"), "http://example.com?valid=true") };
            var hac = new HttpActionContext() { ControllerContext = context };
            attr.OnActionExecuting(hac);
            Assert.IsTrue((bool)ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void OnActionExecutingFalseSwitch()
        {
            var attr = new ScramblerWebApiSessionSwitchAttribute("valid");
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://example.com?valid=false", "valid=false"), new HttpResponse(null));
            var context = new HttpControllerContext() { Request = new HttpRequestMessage(new HttpMethod("test"), "http://example.com?valid=false") };
            var hac = new HttpActionContext() { ControllerContext = context };
            attr.OnActionExecuting(hac);
            Assert.IsFalse((bool)ObfuscationSettings.ScrambleActiveCookie);
        }
    }
}
