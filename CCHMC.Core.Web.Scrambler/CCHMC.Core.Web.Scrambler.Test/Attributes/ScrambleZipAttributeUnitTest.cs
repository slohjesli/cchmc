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
    public class ScrambleZipAttributeUnitTest
    {
        [TestMethod]
        public void DefaultTest ()
        {
            ScrambleZipAttribute scr;

            for (int i=0; i < 1000; i++)
            {
                scr = new ScrambleZipAttribute();
                Assert.IsNotNull(scr.Obfuscate(1) as int?, "Did not obfuscate as int!");
                Assert.IsTrue((scr.Obfuscate(1) as int?) >= 10000 && (scr.Obfuscate(1) as int?) <= 99999, "Did not obfuscate with a 5-digit value!");
            }
        }
        
        [TestMethod]
        public void NotStrictObfuscation ()
        {
            ScrambleZipAttribute scr;

            for (int i=0; i < 1000; i++)
            {
                scr = new ScrambleZipAttribute(false);
                Assert.IsNotNull(scr.Obfuscate(1) as int?, "Did not obfuscate as int!");
                Assert.IsTrue((scr.Obfuscate(1) as int?) >= 10000 && (scr.Obfuscate(1) as int?) <= 99999, "Did not obfuscate with a 5-digit value!");
            }
        }
        
        [TestMethod]
        public void Strict_IntObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate(5) as int?, "Did not obfuscate as int!");
            Assert.IsTrue((scr.Obfuscate(5) as int?) >= 10000 && (scr.Obfuscate(5) as int?) <= 99999, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_UIntObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((uint)5) as uint?, "Did not obfuscate as uint!");
            Assert.IsTrue((scr.Obfuscate(5) as uint?) >= 10000 && (scr.Obfuscate(5) as uint?) <= 99999, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_ShortObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((short)5) as short?, "Did not obfuscate as short!");
            Assert.IsTrue((scr.Obfuscate(5) as short?) >= 10000, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_UShortObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((ushort)5) as ushort?, "Did not obfuscate as ushort!");
            Assert.IsTrue((scr.Obfuscate(5) as ushort?) >= 10000, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_LongObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate(5L) as long?, "Did not obfuscate as long!");
            Assert.IsTrue((scr.Obfuscate(5) as long?) >= 10000L && (scr.Obfuscate(5) as long?) <= 99999L, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_ULongObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((ulong)5) as ulong?, "Did not obfuscate as ulong!");
            Assert.IsTrue((scr.Obfuscate(5) as ulong?) >= 10000 && (scr.Obfuscate(5) as ulong?) <= 99999, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_CharObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate('D') as char?, "Did not obfuscate as char!");
            Assert.IsTrue((scr.Obfuscate(5) as char?) >= 10000, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_StringInvalidLengthObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate("123") as string, "Did not obfuscate as string!");
            Assert.AreEqual(5, scr.Obfuscate(5).ToString().Length, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_5DigitStringObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate("12345") as string, "Did not obfuscate as int!");
            Assert.AreEqual(5, scr.Obfuscate("").ToString().Length, "Did not obfuscate with a 5-digit value!");
        }
        
        [TestMethod]
        public void Strict_5Plus4FormatObfuscation ()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);;
            Assert.IsNotNull(scr.Obfuscate("12345-6789") as string, "Did not obfuscate as string!");
            Assert.AreEqual(10, scr.Obfuscate("").ToString().Length, "Did not obfuscate with a 9-digit value with a hyphen separator!");
        }
        
        [TestMethod]
        public void Strict_Plus4NoHyphenFormatObfuscation()
        {
            ScrambleZipAttribute scr = new ScrambleZipAttribute(true);
            Assert.IsNotNull(scr.Obfuscate("123456789") as string, "Did not obfuscate as string!");
            Assert.AreEqual(9, scr.Obfuscate("").ToString().Length, "Did not obfuscate with a 9-digit value!");
        }
    }
}
