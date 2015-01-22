using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CCHMC.Core.Web.Scrambler.Settings;
using CCHMC.Core.Web.Scrambler.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test
{
    [TestClass]
    public class ScramblerWebApiAttributeUnitTest
    {
        [TestMethod]
        public void NullContext()
        {
            ScramblerWebApiAttribute scram = new ScramblerWebApiAttribute();
            ObfuscationSettings.ScrambleActiveCookie = true;
            HttpActionExecutedContext aec = null;
            scram.OnActionExecuted(aec);

            Assert.IsNull(aec);
        }

        [TestMethod]
        public void NullResponse()
        {
            ScramblerWebApiAttribute scram = new ScramblerWebApiAttribute();
            HttpActionContext context = new HttpActionContext();

            context.Response = null;
            HttpActionExecutedContext aec = new HttpActionExecutedContext(context, new Exception());

            ObfuscationSettings.ScrambleActiveCookie = true;
            
            scram.OnActionExecuted(aec);
            
            Assert.IsNull(aec.Response);
        }

        [TestMethod]
        public void ValidTrueTest ()
        {
            ScramblerWebApiAttribute scram = new ScramblerWebApiAttribute();
            HttpActionContext context = new HttpActionContext();
            
            var test = new Subobject() {
                intField = 100,
                intProp = 2000,
                strField = "This sentence is false",
                strProp = "I am lying right now.",
            };
            ObjectContent result = new ObjectContent(typeof(Subobject), test.Clone(), new JsonMediaTypeFormatter());
            context.Response = new HttpResponseMessage(HttpStatusCode.OK) { Content = result };
            HttpActionExecutedContext aec = new HttpActionExecutedContext(context, new Exception());

            ObfuscationSettings.ScrambleActiveCookie = true;

            scram.OnActionExecuted(aec);

            test = (Subobject)((ObjectContent)aec.Response.Content).Value;
            
            Assert.AreNotEqual(100, test.intField, "intField was not obfuscated! (Chance of obfuscation matching: 0.1%)");
            Assert.AreNotEqual(2000, test.intProp, "intProp was not obfuscated! (Chance of obfuscation matching: 0.01%)");
            Assert.AreNotEqual("This sentence is false", test.strField, "strField was not obfuscated!");
            Assert.AreNotEqual("I am lying right now.", test.strProp, "strProp was not obfuscated!");
        }
        
        [TestMethod]
        public void ValidFalseTest ()
        {
            ScramblerWebApiAttribute scram = new ScramblerWebApiAttribute();
            HttpActionContext context = new HttpActionContext();
            
            var test = new Subobject() {
                intField = 100,
                intProp = 2000,
                strField = "This sentence is false",
                strProp =  "I am lying right now.",
            };
            ObjectContent result = new ObjectContent(typeof(Subobject), test.Clone(), new JsonMediaTypeFormatter());
            context.Response = new HttpResponseMessage(HttpStatusCode.OK) { Content = result };
            HttpActionExecutedContext aec = new HttpActionExecutedContext(context, new Exception());

            ObfuscationSettings.IsActiveDefault = false;

            scram.OnActionExecuted(aec);

            //Assert.AreEqual(test, ((ObjectContent)aec.Response.Content).Value);

            test = (Subobject)((ObjectContent)aec.Response.Content).Value;

            Assert.AreEqual(100, test.intField, "intField was obfuscated!");
            Assert.AreEqual(2000, test.intProp, "intProp was obfuscated!");
            Assert.AreEqual("This sentence is false", test.strField, "strField was obfuscated!");
            Assert.AreEqual("I am lying right now.", test.strProp, "strProp was obfuscated!");
        }
        
        [TestMethod]
        public void NullTest ()
        {
            ScramblerWebApiAttribute scram = new ScramblerWebApiAttribute();
            HttpActionContext context = new HttpActionContext();
            context.Response = new HttpResponseMessage(HttpStatusCode.OK);
            HttpActionExecutedContext aec = new HttpActionExecutedContext(context, new Exception());

            scram.OnActionExecuted(aec);

            Assert.IsNull(aec.Response.Content);
        }
    }
}
