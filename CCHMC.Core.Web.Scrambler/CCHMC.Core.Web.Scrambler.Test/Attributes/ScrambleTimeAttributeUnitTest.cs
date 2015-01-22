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
    public class ScrambleTimeAttributeUnitTest
    {
        
        [TestMethod]
        public void DefaultTest ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute();
            Assert.IsNull(scr.Obfuscate(null));
        }
        
        [TestMethod]
        public void Strict_InvalidObject ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(true);

            var NotADateTime = 152;
            //Should just generate a random DateTime.
            Assert.IsNotNull(scr.Obfuscate(NotADateTime) as DateTime?);
        }
        
        [TestMethod]
        public void Strict_ValidStringObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(true);
            DateTime time = new DateTime(2010, 3, 1, 6, 15, 30);
            
            DateTime tmp;
            var obf = scr.Obfuscate(time.ToString());
            Assert.IsTrue(obf is DateTime || DateTime.TryParse((string)obf, out tmp));
        }
        
        [TestMethod]
        public void Strict_NoDuplicableInformationObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(true);
            
            DateTime time = new DateTime(1, 1, 1, 7, 2, 5);
            Assert.IsNotNull(scr.Obfuscate(time) as DateTime?, "Did not generate a DateTime for Strict settings!");
        }
        
        [TestMethod]
        public void Strict_ReplicableDataObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(true);
            
            DateTime time = new DateTime(2010, 3, 1, 6, 15, 30);
            
            var dt = (DateTime)scr.Obfuscate(time);
            Assert.AreEqual(0, dt.Second % time.Second);
            Assert.AreEqual(0, dt.Minute % time.Minute);
            Assert.AreEqual(0, dt.Hour % time.Hour);
        }
        
        [TestMethod]
        public void NotStrictObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(false);
            Assert.IsNotNull(scr.Obfuscate("") as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate("") as DateTime?).Value.Ticks <= TimeSpan.TicksPerDay, String.Format("Resulting time is more than one day! ({0})", scr.Obfuscate(null)));
        }
        
        [TestMethod]
        public void ZeroStepObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(TimeSpan.Zero);
            Assert.IsNotNull(scr.Obfuscate("") as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate("") as DateTime?).Value.Ticks <= TimeSpan.TicksPerDay, String.Format("Resulting time is more than one day! ({0})", scr.Obfuscate(null)));
        }
        
        [TestMethod]
        public void OnlyTimeStepObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(new TimeSpan(5, 0, 0, 0));
            Assert.IsNotNull(scr.Obfuscate("") as DateTime?, "Did not obfuscate as DateTime!");
            Assert.IsTrue((scr.Obfuscate("") as DateTime?).Value.Ticks <= TimeSpan.TicksPerDay, String.Format("Resulting time is more than one day! ({0})", scr.Obfuscate(null)));
        }
        
        [TestMethod]
        public void OnlyDateStepObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(new TimeSpan(1, 30, 0));
            Assert.IsTrue((scr.Obfuscate("") as DateTime?).Value.Ticks % new TimeSpan(1, 30, 0).Ticks == 0, String.Format("Obfuscation is not a multiple of the TimeSpan! ({0}, {1})", (scr.Obfuscate("") as DateTime?).Value.Ticks, new TimeSpan(1, 30, 0).Ticks));
        }
        
        [TestMethod]
        public void FullDateTimeStepObfuscation ()
        {
            ScrambleTimeAttribute scr = new ScrambleTimeAttribute(new TimeSpan(5, 1, 30, 0));
            Assert.IsTrue((scr.Obfuscate(DateTime.MinValue) as DateTime?).Value.Ticks % new TimeSpan(1, 30, 0).Ticks == 0, "Obfuscation is not a multiple of the TimeSpan as a time!");
        }
    }
}
