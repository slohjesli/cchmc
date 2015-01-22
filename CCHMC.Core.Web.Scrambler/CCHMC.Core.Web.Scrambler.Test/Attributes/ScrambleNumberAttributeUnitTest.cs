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
    public class ScrambleNumberAttributeUnitTest
    {
        [TestMethod]
        public void DefaultTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute();
            Assert.IsNull(scr.Obfuscate(null));
        }
     
        [TestMethod]
        public void StrictIntTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsTrue(scr.Obfuscate(1234) as int? >= 1000 && scr.Obfuscate(1234) as int? <= 9999, "Obfuscation was not of the correct length!");
        }
     
        [TestMethod]
        public void StrictUIntTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsTrue(scr.Obfuscate((uint)1234) as uint? >= 1000 && scr.Obfuscate((uint)1234) as uint? <= 9999, "Obfuscation was not of the correct length!");
        }
     
        [TestMethod]
        public void StrictStringTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.AreEqual(3, ((string)scr.Obfuscate("123")).Length, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate(null)));
        }
     
        [TestMethod]
        public void StrictCharTest ()
        {
            int floor = (int)Math.Pow(10, Math.Floor(Math.Log10('A')));
            char t = (char)floor;
            char t2 = (char)(floor * 10);
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsTrue(scr.Obfuscate('A') as char? >= (char)floor && scr.Obfuscate('A') as char? < (char)(floor * 10), 
                          String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate('A')));
        }
     
        [TestMethod]
        public void StrictLongTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsNotNull(scr.Obfuscate(555555555555L) as long?);
            Assert.IsTrue(scr.Obfuscate(555555555555L) as long? >= 100000000000L && scr.Obfuscate(555555555555L) as long? <= 999999999999L, "Obfuscation was not of the correct length!");
        }
     
        [TestMethod]
        public void StrictULongTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((ulong)555555555555L) as ulong?);
            Assert.IsTrue(scr.Obfuscate((ulong)555555555555L) as ulong? >= 100000000000L && scr.Obfuscate((ulong)555555555555L) as ulong? <= 999999999999L, "Obfuscation was not of the correct length!");
        }
     
        [TestMethod]
        public void StrictShortTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            short test = 52;
            Assert.IsNotNull(scr.Obfuscate(test) as short?, "Did not obfuscate as short!");
            Assert.IsTrue(scr.Obfuscate((short)52) as short? >= 10 && scr.Obfuscate((short)52) as short? <= 99, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate((short)52)));
        }
     
        [TestMethod]
        public void StrictUShortTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((ushort)52) as ushort?, "Did not obfuscate as ushort!");
            Assert.IsTrue(scr.Obfuscate((ushort)52) as ushort? >= 10 && scr.Obfuscate((ushort)52) as ushort? <= 99, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate((ushort)52)));
        }
     
        [TestMethod]
        public void StrictByteTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((byte)52) as byte?, String.Format("Did not obfuscate as byte! ({0})", scr.Obfuscate((byte)52)));
            Assert.IsTrue(scr.Obfuscate((byte)52) as byte? >= 10 && scr.Obfuscate((byte)52) as byte? <= 99, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate((byte)52)));
        }
     
        [TestMethod]
        public void StrictSByteTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
            Assert.IsNotNull(scr.Obfuscate((sbyte)52) as sbyte?, "Did not obfuscate as sbyte!");
            Assert.IsTrue(scr.Obfuscate((sbyte)52) as sbyte? >= 10 && scr.Obfuscate((sbyte)52) as sbyte? <= 99, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate((sbyte)52)));
        }
     
        [TestMethod]
        public void StrictFloatTest ()
        {
            float val = 3.5f;
            for (int i = 0; i < 1000; i++)
            {
                ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
                Assert.IsNotNull(Convert.ToSingle(scr.Obfuscate(val)), String.Format("Failed casting value ({0}) to float!", scr.Obfuscate(val)));
                Assert.IsTrue(Convert.ToSingle(scr.Obfuscate(val)) > 1, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate(val)));
            }
        }
     
        [TestMethod]
        public void StrictDoubleTest ()
        {
            double val = 3.5D;
            for (int i = 0; i < 1000; i++)
            {
                ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
                Assert.IsNotNull(Convert.ToDouble(scr.Obfuscate(val)), String.Format("Failed casting value ({0}) to double!", scr.Obfuscate(val)));
                Assert.IsTrue(String.Format("{0}", scr.Obfuscate(val) as double?).Length == 3, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate(val)));
            }
        }
     
        [TestMethod]
        public void StrictDecimalTest ()
        {
            decimal val = 3.5m;
            for (int i = 0; i < 1000; i++)
            {
                ScrambleNumberAttribute scr = new ScrambleNumberAttribute(true);
                Assert.IsNotNull(Convert.ToDecimal(scr.Obfuscate(val)), String.Format("Failed casting value ({0}) to decimal!", scr.Obfuscate(val)));
                Assert.IsTrue(Convert.ToDecimal(scr.Obfuscate(val)) > 1, String.Format("Obfuscation was not of the correct length! ({0})", scr.Obfuscate(val)));
            }
        }
     
        [TestMethod]
        public void NotStrictTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(false);
            Assert.IsNotNull(scr.Obfuscate(55) as int?, "Did not obfuscate as int!");
        }
     
        [TestMethod]
        public void NegativeNegativeRangeTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(-5, -5);
            int val = 55;
            Assert.IsNotNull(scr.Obfuscate(val) as int?, "Did not obfuscate as int!");
            Assert.AreEqual(-5, scr.Obfuscate(val) as int?);
        }
     
        [TestMethod]
        public void NegativeZeroRangeTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(-5, 0);
            int val = 55;
            Assert.IsNotNull(scr.Obfuscate(val) as int?, "Did not obfuscate as int!");
            Assert.IsTrue(scr.Obfuscate(val) as int? >= -5 && scr.Obfuscate(val) as int? <= 0, "Generated number out of range!");
        }
     
        [TestMethod]
        public void NegativePositiveRangeTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(-5, 5);
            int val = 55;
            Assert.IsNotNull(scr.Obfuscate(val) as int?, "Did not obfuscate as int!");
            Assert.IsTrue(scr.Obfuscate(val) as int? >= -5 && scr.Obfuscate(val) as int? <= 5, "Generated number out of range!");
        }
     
        [TestMethod]
        public void ZeroNegativeRangeTest ()
        {
            ScrambleNumberAttribute scr;
            scr = new ScrambleNumberAttribute(0, -5);
            int val = 55;
            Assert.IsTrue((int)scr.Obfuscate(val) <= 0 && (int)scr.Obfuscate(val) >= -5, "Invalid min and max values were not reversed!");
        }
     
        [TestMethod]
        public void ZeroZeroRangeTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(0, 0);
            int val = 55;
            Assert.IsNotNull(scr.Obfuscate(val) as int?, "Did not obfuscate as int!");
            Assert.AreEqual(0, scr.Obfuscate(val) as int?);
        }
     
        [TestMethod]
        public void ZeroPositiveRangeTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(0, 5);
            int val = 55;
            Assert.IsNotNull(scr.Obfuscate(val) as int?, "Did not obfuscate as int!");
            Assert.IsTrue(scr.Obfuscate(val) as int? >= 0 && scr.Obfuscate(val) as int? <= 5, "Generated number out of range!");
        }
     
        [TestMethod]
        public void PositiveNegativeRangeTest ()
        {
            ScrambleNumberAttribute scr;
            scr = new ScrambleNumberAttribute(5, -5);
            int val = 55;
            Assert.IsTrue((int)scr.Obfuscate(val) <= 5 && (int)scr.Obfuscate(val) >= -5, "Invalid min and max values were not reversed!");
        }
     
        [TestMethod]
        public void PositiveZeroRangeTest ()
        {
            ScrambleNumberAttribute scr;
            scr = new ScrambleNumberAttribute(5, 0);
            int val = 55;
            Assert.IsTrue((int)scr.Obfuscate(val) <= 5 && (int)scr.Obfuscate(val) >=0, "Invalid min and max values were not reversed!");
        }
     
        [TestMethod]
        public void PositivePositiveRangeTest ()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5, 5);
            int val = 55;
            Assert.IsNotNull(scr.Obfuscate(val) as int?, "Did not obfuscate as int!");
            Assert.AreEqual(5, scr.Obfuscate(val) as int?);
        }

        [TestMethod]
        public void ConstrutorPositivePrefix()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            Assert.AreEqual(5, scr.Obfuscate(1));
        }

        [TestMethod]
        public void ConstrutorNegativePrefix()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(-5);
            Assert.AreEqual(-5, scr.Obfuscate(1));
        }

        [TestMethod]
        public void ConstrutorZeroPrefix()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(0);
            Assert.IsTrue((int)scr.Obfuscate(1) >= 0 && (int)scr.Obfuscate(1) < 10);
        }

        //Todo: Should Longs be allowed as prefixes?
        [TestMethod]
        public void ConstrutorPrefixLongLength()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(int.MaxValue);
            long val = (long)(int.MaxValue) * 10L;
            Assert.IsTrue((long)scr.Obfuscate(val) >= int.MaxValue * 10L && (long)scr.Obfuscate(val) < int.MaxValue * 10L + 10L);
        }

        [TestMethod]
        public void PrefixObfuscationTooLong()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(555555555);
            Assert.AreEqual(555555555, scr.Obfuscate(7));
        }

        [TestMethod]
        public void PrefixObfuscateString()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            string val = "107";
            Assert.AreEqual('5', ((string)scr.Obfuscate(val))[0]);
            Assert.AreEqual(3, ((string)scr.Obfuscate(val)).Length);
        }

        [TestMethod]
        public void PrefixObfuscateInt()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            Assert.IsTrue((int)scr.Obfuscate(97) >= 50 && (int)scr.Obfuscate(97) < 60, String.Format("Expected: 50-59. Actual: {0}", (int)scr.Obfuscate(97)));
        }

        [TestMethod]
        public void PrefixObfuscateUInt()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            uint val = (uint)97;
            Assert.IsTrue((uint)scr.Obfuscate(val) >= 50 && (uint)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (uint)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateLong()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            long val = 97L;
            Assert.IsTrue((long)scr.Obfuscate(val) >= 50 && (long)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (long)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateULong()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            ulong val = (ulong)97;
            Assert.IsTrue((ulong)scr.Obfuscate(val) >= 50 && (ulong)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (ulong)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateShort()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            short val = (short)97;
            Assert.IsTrue((short)scr.Obfuscate(val) >= 50 && (short)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (short)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateUShort()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            ushort val = (ushort)97;
            Assert.IsTrue((ushort)scr.Obfuscate(val) >= 50 && (ushort)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (ushort)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateByte()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            byte val = (byte)97;
            Assert.IsTrue((byte)scr.Obfuscate(val) >= 50 && (byte)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (byte)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateSByte()
        {
            ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
            sbyte val = (sbyte)97;
            Assert.IsTrue((sbyte)scr.Obfuscate(val) >= 50 && (sbyte)scr.Obfuscate(val) < 60, String.Format("Expected: 50-59. Actual: {0}", (sbyte)scr.Obfuscate(val)));

        }

        [TestMethod]
        public void PrefixObfuscateFloat()
        {
            for (int i = 0; i < 1000; i++)
            {
                ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
                float val = (float)9.7;
                Assert.IsTrue((float)scr.Obfuscate(val) >= 5.0 && (float)scr.Obfuscate(val) < 6.0, String.Format("Expected: 5.0-6.0. Actual: {0}", (float)scr.Obfuscate(val)));
            }
        }

        [TestMethod]
        public void PrefixObfuscateDouble()
        {
            double val = (double)9.7;
            for (int i = 0; i < 1000; i++)
            {
                ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
                Assert.IsTrue((double)scr.Obfuscate(val) >= 5.0 && (double)scr.Obfuscate(val) < 6.0, String.Format("Expected: 5.0-6.0. Actual: {0}", (double)scr.Obfuscate(val)));
            }
        }

        [TestMethod]
        public void PrefixObfuscateDecimal()
        {
            decimal val = (decimal)9.7;
            for (int i = 0; i < 1000; i++)
            {
                ScrambleNumberAttribute scr = new ScrambleNumberAttribute(5);
                Assert.IsTrue((decimal)scr.Obfuscate(val) >= 5.0m && (decimal)scr.Obfuscate(val) < 6.0m, String.Format("Expected: 5.0-6.0. Actual: {0}", (decimal)scr.Obfuscate(val)));
            }
        }
    }
}
