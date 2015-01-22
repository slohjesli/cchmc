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
    public class ScrambleDateAttributeUnitTest
    {
        [TestMethod]
        public void DefaultObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute();
            Assert.IsNotNull(scr.Obfuscate(DateTime.MinValue) as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate(DateTime.MinValue) as DateTime?).Value.Ticks % TimeSpan.TicksPerDay == 0, "Obfuscation is not a number of days!");
            Assert.AreEqual(scr.Obfuscate(DateTime.MinValue), scr.Obfuscate(DateTime.Now));
        }
        
        [TestMethod]
        public void Strict_InvalidObjectObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(true);

            var NotADateTime = 152;
            //Should just generate a random DateTime.
            Assert.IsNotNull(scr.Obfuscate(NotADateTime) as DateTime?);
        }
        
        [TestMethod]
        public void Strict_ValidStringObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(true);
            
            DateTime time = new DateTime(2010, 3, 1, 6, 15, 30);
            DateTime tmp;
            Assert.IsTrue(scr.Obfuscate(time.ToString()) is DateTime 
                || DateTime.TryParse((string)scr.Obfuscate(time.ToString()), out tmp));
        }
        
        [TestMethod]
        public void Strict_NoDuplicableInformationObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(true);
            
            DateTime time = new DateTime(13, 4, 6, 7, 2, 1);
            Assert.IsNotNull(scr.Obfuscate(time) as DateTime?, "Did not generate a DateTime for Strict settings!");
        }
        
        [TestMethod]
        public void Strict_ReplicableDataObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(true);
            
            DateTime time = new DateTime(2010, 3, 1, 6, 15, 30);

            var dt = (DateTime)scr.Obfuscate(time);
            Assert.AreEqual(0, dt.Day % time.Day);
            Assert.AreEqual(0, dt.Month % time.Month);
            Assert.AreEqual(0, dt.Year % (time.Year % 100));
        }
        
        [TestMethod]
        public void NotStrictObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(false);            
            Assert.IsTrue((scr.Obfuscate(DateTime.Now) as DateTime?).Value.Ticks % TimeSpan.TicksPerDay == 0, "Obfuscation is not a number of days!");
            Assert.AreEqual(scr.Obfuscate(DateTime.Now), scr.Obfuscate(DateTime.Now));
        }

        [TestMethod]
        public void ZeroStepObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(TimeSpan.Zero);
            Assert.AreEqual(DateTime.Today, (scr.Obfuscate(DateTime.Now) as DateTime?).Value, "Resulting date is not today!");
        }
        
        [TestMethod]
        public void OnlyTimeStepObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(new TimeSpan(1, 30, 0));
            Assert.AreEqual(DateTime.Today, (scr.Obfuscate(DateTime.Now) as DateTime?).Value, "Resulting date is not today!");
        }
        
        [TestMethod]
        public void OnlyDateStepObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(new TimeSpan(5, 0, 0, 0));
            Assert.IsTrue((scr.Obfuscate(DateTime.Now) as DateTime?).Value.Ticks % new TimeSpan(5, 0, 0, 0).Ticks == 0, "Obfuscation is not a multiple of the TimeSpan!");
        }
        
        [TestMethod]
        public void FullDateTimeStepObfuscation ()
        {
            ScrambleDateAttribute scr = new ScrambleDateAttribute(new TimeSpan(5, 1, 30, 0));
            Assert.IsNotNull(scr.Obfuscate(DateTime.MinValue) as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate(DateTime.MinValue) as DateTime?).Value.Ticks % new TimeSpan(5, 0, 0, 0).Ticks == 0, "Obfuscation is not a multiple of the TimeSpan in days!");
        }
    }
}
