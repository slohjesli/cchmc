using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using CCHMC.Core.Web.Scrambler.Attributes;

namespace CCHMC.Core.Web.Scrambler.Test.Attributes
{
    [TestClass]
    public class ScrambleBedAttributeUnitTest
    {
        Regex bed = new Regex("^..[0-9]{2}[A-Z][0-9]$");
        ScrambleBedAttribute scram;

        [TestInitialize]
        public void Init()
        {
            scram = new ScrambleBedAttribute();
        }

        [TestMethod]
        public void ObfuscateValidString()
        {
            Assert.IsTrue(bed.IsMatch((string)scram.Obfuscate("A256N1")), String.Format("Result did not match the bed format! ({0})", scram.Obfuscate(null)));
        }

        [TestMethod]
        public void ObfuscateNull()
        {
            Assert.IsNull(scram.Obfuscate(null));
        }

        [TestMethod]
        public void ObfuscateNonString()
        {
            Assert.IsTrue(bed.IsMatch((string)scram.Obfuscate(new object())), String.Format("Result did not match the bed format! ({0})", scram.Obfuscate(null)));
        }

        [TestMethod]
        public void ObfuscateEmptyString()
        {
            Assert.IsTrue(bed.IsMatch((string)scram.Obfuscate("")), String.Format("Result did not match the bed format! ({0})", scram.Obfuscate(null)));
        }

        [TestMethod]
        public void ObfuscateShortString()
        {
            Assert.IsTrue(bed.IsMatch((string)scram.Obfuscate("a")), String.Format("Result did not match the bed format! ({0})", scram.Obfuscate(null)));
        }

        [TestMethod]
        public void ObfuscateLongString()
        {
            Assert.IsTrue(bed.IsMatch((string)scram.Obfuscate("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")), String.Format("Result did not match the bed format! ({0})", scram.Obfuscate(null)));
        }
    }
}
