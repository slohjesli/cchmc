using System;
using System.IO;
using System.Web;
using CCHMC.Core.Web.Scrambler.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Settings
{
    [TestClass]
    public class ObfuscationSettingsUnitTest
    {
        [TestInitialize]
        public void Initialize ()
        {
            HttpRequest req = new HttpRequest("test", "http://www.example.com/", "");
            HttpResponse resp = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(req, resp);
        }

        [TestMethod]
        public void MultiSessionCookieInteraction()
        {
            HttpContext context1 = HttpContext.Current;

            HttpRequest req = new HttpRequest("test", "http://www.example.com/", "");
            HttpResponse resp = new HttpResponse(new StringWriter());
            HttpContext context2 = new HttpContext(req, resp);

            ObfuscationSettings.ScrambleActiveCookie = true;

            HttpContext.Current = context2;
            ObfuscationSettings.ScrambleActiveCookie = false;

            HttpContext.Current = context1;
            Assert.IsTrue((bool)ObfuscationSettings.ScrambleActiveCookie);

            HttpContext.Current = context2;
            Assert.IsFalse((bool)ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void NullHttpContext_SetCookie()
        {
            HttpContext.Current = null;
            ObfuscationSettings.ScrambleActiveCookie = true;

            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void NullHttpContext_GetCookie ()
        {
            HttpContext.Current = null;
            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void DefaultCookie_IsNull ()
        {
            Assert.IsNull(ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void SetCookieToTrue_ImmediateTruthiness ()
        {
            ObfuscationSettings.ScrambleActiveCookie = true;
            Assert.IsTrue((bool)ObfuscationSettings.ScrambleActiveCookie);
        }
        
        [TestMethod]
        public void SetCookieToTrue_ResponseTruthiness ()
        {
            ObfuscationSettings.ScrambleActiveCookie = true;
            Assert.AreEqual("true", (string)HttpContext.Current.Response.Cookies.Get(ObfuscationSettings.ActiveSession).Value);
        }

        [TestMethod]
        public void SetCookieToFalse_ImmediateTruthiness ()
        {
            ObfuscationSettings.ScrambleActiveCookie = false;
            Assert.IsFalse((bool)ObfuscationSettings.ScrambleActiveCookie);
        }

        [TestMethod]
        public void SetCookieToFalse_ResponseTruthiness ()
        {
            ObfuscationSettings.ScrambleActiveCookie = false;
            Assert.AreEqual("false", (string)HttpContext.Current.Response.Cookies.Get(ObfuscationSettings.ActiveSession).Value);
        }
    }
}
