using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Models;

namespace CCHMC.Core.Web.Scrambler.Test.Models
{
    [TestClass]
    public class DateTimeMaskUnitTest
    {
        [TestMethod]
        public void DefaultConstructor()
        {
            DateTimeMask mask = new DateTimeMask();
            Assert.IsTrue(mask.Year);
            Assert.IsTrue(mask.Month);
            Assert.IsTrue(mask.Day);
            Assert.IsTrue(mask.Hour);
            Assert.IsTrue(mask.Minute);
            Assert.IsTrue(mask.Seconds);
        }

        [TestMethod]
        public void BoolConstructor()
        {
            DateTimeMask mask = new DateTimeMask(false, true, false, true, false, true);
            Assert.IsFalse(mask.Year);
            Assert.IsTrue(mask.Month);
            Assert.IsFalse(mask.Day);
            Assert.IsTrue(mask.Hour);
            Assert.IsFalse(mask.Minute);
            Assert.IsTrue(mask.Seconds);
        }

        [TestMethod]
        public void IntConstructor()
        {
            DateTimeMask mask = new DateTimeMask(42);
            Assert.IsTrue(mask.Year);
            Assert.IsFalse(mask.Month);
            Assert.IsTrue(mask.Day);
            Assert.IsFalse(mask.Hour);
            Assert.IsTrue(mask.Minute);
            Assert.IsFalse(mask.Seconds);
        }
    }
}
