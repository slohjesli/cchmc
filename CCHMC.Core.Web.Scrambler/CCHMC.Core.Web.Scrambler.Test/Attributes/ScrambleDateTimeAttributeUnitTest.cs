using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Attributes
{
    [TestClass]
    public class ScrambleDateTimeAttributeUnitTest
    {
        
        [TestMethod]
        public void DefaultObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute();
            Assert.IsNotNull(scr.Obfuscate(DateTime.MinValue) as DateTime?, "Did not obfuscate as DateTime!");
        }
        
        [TestMethod]
        public void Strict_InvalidObjectObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(true);

            var NotADateTime = 152;
            scr.Obfuscate(NotADateTime);
            //Should just generate a random DateTime.
            Assert.IsNotNull(scr.Obfuscate(NotADateTime) as DateTime?);
        }

        [TestMethod]
        public void Strict_ValidStringObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(true);
            
            DateTime time = new DateTime(2010, 3, 1, 6, 15, 30);
            var val = time.ToString();
            
            DateTime tmp;
            Assert.IsTrue(scr.Obfuscate(val) is DateTime || DateTime.TryParse((string)scr.Obfuscate(val), out tmp));
        }
        
        [TestMethod]
        public void Strict_NoDuplicableInformationObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(true);
            
            DateTime time = new DateTime(13, 4, 6, 7, 2, 1);
            Assert.IsNotNull(scr.Obfuscate(time) as DateTime?, "Did not generate a DateTime for Strict settings!");
        }
        
        [TestMethod]
        public void Strict_ReplicableDataObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(true);
            DateTime time = new DateTime(2010, 3, 1, 6, 15, 30);
            
            var dt = (DateTime)scr.Obfuscate(time);
            Assert.AreEqual(0, dt.Second % time.Second);
            Assert.AreEqual(0, dt.Minute % time.Minute);
            Assert.AreEqual(0, dt.Hour % time.Hour);
            Assert.AreEqual(0, dt.Day % time.Day);
            Assert.AreEqual(0, dt.Month % time.Month);
            Assert.AreEqual(0, dt.Year % (time.Year % 100));
        }
        
        [TestMethod]
        public void NotStrictObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(false);
            Assert.IsNotNull(scr.Obfuscate(DateTime.Now) as DateTime?, "Did not obfuscate as DateTime!");
        }
        
        [TestMethod]
        public void ZeroStepObfuscation ()
        {
            DateTime now = DateTime.Now;
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(TimeSpan.Zero);
            Assert.IsNotNull(scr.Obfuscate(DateTime.Now) as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue(scr.Obfuscate(DateTime.Now) as DateTime? >= now && scr.Obfuscate(DateTime.Now) as DateTime? <= DateTime.Now);
        }
        
        [TestMethod]
        public void OnlyTimeStepObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(new TimeSpan(1, 30, 0));
            Assert.IsNotNull(scr.Obfuscate(DateTime.Now) as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate(DateTime.Now) as DateTime?).Value.Ticks % new TimeSpan(1, 30, 0).Ticks == 0, "Obfuscation is not a multiple of the TimeSpan!");
        }
        
        [TestMethod]
        public void OnlyDateStepObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(new TimeSpan(5, 0, 0, 0));
            Assert.IsNotNull(scr.Obfuscate(DateTime.Now) as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate(DateTime.Now) as DateTime?).Value.Ticks % new TimeSpan(1, 30, 0).Ticks == 0, "Obfuscation is not a multiple of the TimeSpan!");
        }
        
        [TestMethod]
        public void FullDateTimeStepObfuscation ()
        {
            ScrambleDateTimeAttribute scr = new ScrambleDateTimeAttribute(new TimeSpan(5, 1, 30, 0));
            Assert.IsNotNull(scr.Obfuscate(DateTime.MinValue) as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate(DateTime.MinValue) as DateTime?).Value.Ticks % new TimeSpan(1, 30, 0).Ticks == 0, "Obfuscation is not a multiple of the TimeSpan!");
        }
    }
}
