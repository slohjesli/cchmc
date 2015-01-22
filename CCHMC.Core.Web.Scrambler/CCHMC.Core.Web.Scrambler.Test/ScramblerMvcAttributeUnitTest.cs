using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CCHMC.Core.Web.Scrambler.Settings;
using CCHMC.Core.Web.Scrambler.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace CCHMC.Core.Web.Scrambler.Test
{
    [TestClass]
    public class ScramblerMvcAttributeUnitTest
    {
        [TestInitialize]
        public void Initialize()
        {
            ObfuscationSettings.IsActiveDefault = true;
        }

        [TestMethod]
        public void NullContext()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ObfuscationSettings.ScrambleActiveCookie = true;
            ActionExecutedContext aec = null;
            scram.OnActionExecuted(aec);

            Assert.IsNull(aec);
        }

        [TestMethod]
        public void InvalidTypeTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            JsonResult result = new JsonResult();
            
            var test = new SimpleObject() { Name="test" };

            ObfuscationSettings.ScrambleActiveCookie = true;

            result.Data = test;
            aec.Result = result;

            scram.OnActionExecuted(aec);

            Assert.AreNotEqual("test", ((SimpleObject)result.Data).Name);
        }
        
        [TestMethod]
        public void EmptyJsonTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            JsonResult result = new JsonResult();
            
            ObfuscationSettings.ScrambleActiveCookie = true;
            aec.Result = result;

            scram.OnActionExecuted(aec);

            Assert.IsNull(result.Data);
        }
        
        [TestMethod]
        public void JsonObfuscateTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            JsonResult result = new JsonResult();

            var test = new SimpleObject() { Name = "test" };

            ObfuscationSettings.ScrambleActiveCookie = true;

            result.Data = test;
            aec.Result = result;

            scram.OnActionExecuted(aec);
            
            test = (SimpleObject)((JsonResult)aec.Result).Data;
            Assert.AreNotEqual("test", test.Name);
        }
        
        [TestMethod]
        public void JsonDoNotObfuscateTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            JsonResult result = new JsonResult();

            var test = new SimpleObject() { Name = "test" };

            ObfuscationSettings.IsActiveDefault = false;

            result.Data = test;
            aec.Result = result;

            scram.OnActionExecuted(aec);
            
            test = (SimpleObject)((JsonResult)aec.Result).Data;
            Assert.AreEqual("test", test.Name);
        }

        [TestMethod]
        public void ViewObfuscateTest()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();

            var result = new ViewResult();
            var test = new SimpleObject() { Name = "test" };
            result.ViewData.Model = test;
            aec.Result = result;

            ObfuscationSettings.ScrambleActiveCookie = true;

            scram.OnActionExecuted(aec);

            test = (SimpleObject)((ViewResult)aec.Result).Model;

            Assert.AreNotEqual("test", test.Name);
        }

        [TestMethod]
        public void ViewObfuscateEnumerableTest()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();

            var result = new ViewResult();
            var expected = new List<string> { "test", "bob", "jabberwocky", "snicker-snacker" }.Select(t => t);
            result.ViewData.Model = expected;
            aec.Result = result;

            ObfuscationSettings.ScrambleActiveCookie = true;

            scram.OnActionExecuted(aec);

            var test = (IEnumerable<string>)((ViewResult)aec.Result).Model;

            var tEnum = test.GetEnumerator();
            var exEnum = expected.GetEnumerator();

            while (tEnum.MoveNext() && exEnum.MoveNext())
            {
                //Cycle through so more information can be given if it fails.
                Assert.AreNotEqual(exEnum.Current, tEnum.Current);
            }
            //Since all of the elements analyzed so far match, if this fails, they have different lengths.
            Assert.AreEqual(expected.Count(), test.Count(), "Result was not of the expected length!");
        }
        
        [TestMethod]
        public void ViewDoNotObfuscateTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            
            var result = new ViewResult();
            var test = new SimpleObject() { Name = "test" };
            result.ViewData.Model = test;
            aec.Result = result;

            ObfuscationSettings.IsActiveDefault = false;
            
            scram.OnActionExecuted(aec);
            
            test = (SimpleObject)((ViewResult)aec.Result).Model;

            Assert.AreEqual("test", test.Name);
        }

        [TestMethod]
        public void OnActionExecuting_IgnoreFlagTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            
            var result = new ViewResult();
            AttributedObject test = new AttributedObject() { donotscramble_flagged="Unscrambled" };
            
            result.ViewData.Model = test;
            aec.Result = result;

            ObfuscationSettings.ScrambleActiveCookie = true;
            
            scram.OnActionExecuted(aec);
            
            test = (AttributedObject)((ViewResult)aec.Result).Model;

            Assert.AreEqual("Unscrambled", test.donotscramble_flagged);
        }

        [TestMethod]
        public void OnActionExecuting_IgnoreAttributeTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            
            var result = new ViewResult();
            AttributedObject test = new AttributedObject() { donotscramble_attr="Unscrambled" };
            
            result.ViewData.Model = test;
            aec.Result = result;

            ObfuscationSettings.ScrambleActiveCookie = true;
            
            scram.OnActionExecuted(aec);
            
            test = (AttributedObject)((ViewResult)aec.Result).Model;

            Assert.AreEqual("Unscrambled", test.donotscramble_attr);
        }

        [TestMethod]
        public void OnActionExecuting_ScrambleAttributeTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            
            var result = new ViewResult();
            AttributedObject test = new AttributedObject() { scramblearg="Unscrambled" };
            
            result.ViewData.Model = test;
            aec.Result = result;

            ObfuscationSettings.ScrambleActiveCookie = true;
            
            scram.OnActionExecuted(aec);
            
            test = (AttributedObject)((ViewResult)aec.Result).Model;

            Assert.AreEqual("This is the scrambled version of scramblearg.", test.scramblearg);
        }

        [TestMethod]
        public void OnActionExecuting_AlternativeattributeTest ()
        {
            ScramblerMvcAttribute scram = new ScramblerMvcAttribute();
            ActionExecutedContext aec = new ActionExecutedContext();
            
            var result = new ViewResult();
            AttributedObject test = new AttributedObject() { intProp=192 };
            
            result.ViewData.Model = test;
            aec.Result = result;

            ObfuscationSettings.ScrambleActiveCookie = true;
            
            scram.OnActionExecuted(aec);
            
            test = (AttributedObject)((ViewResult)aec.Result).Model;

            Assert.AreNotEqual(192, test.intProp);
        }
    }
}
