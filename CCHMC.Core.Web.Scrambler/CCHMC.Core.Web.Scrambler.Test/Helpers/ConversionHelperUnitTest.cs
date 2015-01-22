using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Test.TestModels;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class ConversionHelperUnitTest
    {
        Type to = typeof(int);

        [TestMethod]
        public void TryConvertToNullObject()
        {
            Assert.IsNull(ConversionHelper.TryConvertTo(null, typeof(object)));
        }

        [TestMethod]
        public void TryConvertToValidConversion()
        {
            long obj = 55L;
            Assert.AreEqual(to, ConversionHelper.TryConvertTo(obj, to).GetType());
        }

        [TestMethod]
        public void TryConvertToInvalidConversion()
        {
            string obj = "Abra kadabra alakazam";
            Assert.IsNull(ConversionHelper.TryConvertTo(obj, to));
        }

        [TestMethod]
        public void ConvertTypeIsAssignable()
        {
            string obj = "Test";
            var res = ConversionHelper.ConvertType(obj, typeof(object));
            Assert.AreEqual(obj, res);
            Assert.AreEqual(typeof(string), res.GetType());
            Assert.IsNotNull(res as string);
        }

        [TestMethod]
        public void ConvertTypeNull()
        {
            Assert.IsNull(ConversionHelper.ConvertType(null, to));
        }

        [TestMethod]
        public void ConvertTypeCanConvert()
        {
            string obj = "128";
            var res = ConversionHelper.ConvertType(obj, to);
            Assert.AreEqual(128, res);
            Assert.AreEqual(to, res.GetType());
        }


        [TestMethod]
        public void ConvertTypeCannotConvert()
        {
            var obj = new { a=1, b=2 };
            bool fail = false;
            try
            {
                ConversionHelper.ConvertType(obj, to);
            }
            catch (InvalidCastException ice)
            {
                fail = true;
            }
            Assert.IsTrue(fail, "ConversionHelper did not throw InvalidCastException when converting to an invalid type!");
        }
    }
}
