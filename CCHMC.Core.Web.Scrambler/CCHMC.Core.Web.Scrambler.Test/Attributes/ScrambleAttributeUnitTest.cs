using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Attributes
{
    [TestClass]
    public class ScrambleAttributeUnitTest
    {
        [TestMethod]
        public void DefaultInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute();
            Assert.IsNull(scr.Obfuscate(null) as object);
        }

        [TestMethod]
        public void DefaultInitialization_StringPropertyObfsucation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute();
            Assert.IsNull(scr.Obfuscate("text") as string);
        }
        
        [TestMethod]
        public void NullInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(null);
            Assert.IsNull(scr.Obfuscate(null) as object);
        }
        
        [TestMethod]
        public void NullInitialization_BasicPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(null);
            Assert.IsNull(scr.Obfuscate("Test") as string);
        }
        
        [TestMethod]
        public void NullInitialization_ClassPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(null);
            Assert.IsNull(scr.Obfuscate(new SimpleObject(){ Name="Test" }));
        }
        
        [TestMethod]
        public void StringInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute("Test");
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void StringInitialization_StringPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute("Test");
            Assert.AreEqual("Test", scr.Obfuscate("Second"));
        }
        
        [TestMethod]
        public void StringInitialization_NonStringPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute("Test");
            Assert.AreEqual("Test", scr.Obfuscate(5));
        }
        
        [TestMethod]
        public void BooleanInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(true);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void BooleanInitialization_BooleanPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(true);
            Assert.AreEqual(true, scr.Obfuscate(false));
        }
        
        [TestMethod]
        public void BooleanInitialization_NonBooleanPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(true);
            Assert.AreEqual(true, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void ByteInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute((byte)5);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void ByteInitialization_BytePropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute((byte)5);
            Assert.AreEqual((byte)5, scr.Obfuscate((byte)5));
        }
        
        [TestMethod]
        public void ByteInitialization_NonBytePropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute((byte)5);
            Assert.AreEqual((byte)5, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void ObjectInitialization_NullPropertyObfuscation ()
        {
            var obj = new object();
            ScrambleAttribute scr = new ScrambleAttribute(obj);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void ObjectInitialization_ObjectPropertyObfuscation ()
        {
            var obj = new object();
            ScrambleAttribute scr = new ScrambleAttribute(obj);
            Assert.AreEqual(obj, scr.Obfuscate(new Object()));
        }
        
        [TestMethod]
        public void IntInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(5);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void IntInitialization_IntPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(5);
            Assert.AreEqual(5, scr.Obfuscate(27));
        }
        
        [TestMethod]
        public void IntInitialization_NonIntPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(5);
            Assert.AreEqual(5, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void FloatInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5F);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void FloatInitialization_FloatPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5F);
            Assert.AreEqual(3.5F, scr.Obfuscate(7.9F));
        }
        
        [TestMethod]
        public void FloatInitialization_NonFloatPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5F);
            Assert.AreEqual(3.5F, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void DoubleInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5D);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void DoubleInitialization_DoublePropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5D);
            Assert.AreEqual(3.5D, scr.Obfuscate(7.5D));
        }
        
        [TestMethod]
        public void DoubleInitialization_NonDoublePropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5D);
            Assert.AreEqual(3.5D, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void DecimalInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5m);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void DecimalInitialization_DecimalPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5m);
            Assert.AreEqual(3.5m, scr.Obfuscate(5.2m));
        }
        
        [TestMethod]
        public void DecimalInitialization_NonDecimalPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(3.5m);
            Assert.AreEqual(3.5m, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void LongInitialization_NullPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(5555555555L);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void LongInitialization_LongPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(5555555555L);
            Assert.AreEqual(5555555555L, scr.Obfuscate(9999999999L));
        }
        
        [TestMethod]
        public void LongInitialization_NonLongPropertyObfuscation ()
        {
            ScrambleAttribute scr = new ScrambleAttribute(5555555555L);
            Assert.AreEqual(5555555555L, scr.Obfuscate("Test"));
        }
        
        [TestMethod]
        public void TestClassInitialization_NullPropertyObfuscation ()
        {
            var test = new SimpleObject() { Name="Schmohn Schmacob Schmingleheimer Jidt" };
            ScrambleAttribute scr = new ScrambleAttribute(test);
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void TestClassInitialization_TestClassPropertyObfuscation ()
        {
            var test = new SimpleObject() { Name="Schmohn Schmacob Schmingleheimer Jidt" };
            ScrambleAttribute scr = new ScrambleAttribute(test);
            Assert.AreEqual(test, scr.Obfuscate(new SimpleObject() { Name="John Jacob Jingleheimer Schmidt" }));
        }
        
        [TestMethod]
        public void TestClassInitialization_NonTestClassPropertyObfuscation ()
        {
            var test = new SimpleObject() { Name="Schmohn Schmacob Schmingleheimer Jidt" };
            ScrambleAttribute scr = new ScrambleAttribute(test);
            Assert.AreEqual(test, scr.Obfuscate("Test"));
        }
    }
}
