using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class RandomHelperUnitTest
    {
        [TestMethod]
        public void NextByStepZeroStep()
        {
            Assert.AreEqual(2, RandomHelper.NextByStep(2, 50, 0));
        }

        [TestMethod]
        public void NextByStepValidStep() 
        {
            for (int i = 0; i < 100; i++)
                Assert.AreEqual(0, RandomHelper.NextByStep(5, 2) % 2, "Did not produce an even number with a step size of two!");
        }

        [TestMethod]
        public void NextByStepPositiveMax()
        {
            for (int i = 0; i < 100; i++)
            {
                int res = RandomHelper.NextByStep(5, 1);
                Assert.IsTrue(res >= 0 && res < 5, String.Format("Produced a result outside of the range (0, 5)! ({0})", res));
            }
        }

        [TestMethod]
        public void NextByStepNegativeMax()
        {
            //Repeat to increase the chances of failure if failure may occur.
            for (int i = 0; i < 100; i++)
                Assert.IsTrue(RandomHelper.NextByStep(-5, 1) <= 0, "Produced a result which was greater than zero!");
        }

        [TestMethod]
        public void NextByStepZeroMax()
        {
            Assert.AreEqual(0, RandomHelper.NextByStep(0, 1));
        }

        [TestMethod]
        public void NextByStepMinEqualsMax()
        {
            Assert.AreEqual(5, RandomHelper.NextByStep(5, 5, 1));
        }

        [TestMethod]
        public void NextByStepMinGreaterThanMax()
        {
            for (int i = 0; i < 100; i++)
            {
                int res = RandomHelper.NextByStep(10, 5, 1);
                Assert.IsTrue(res > 5 && res <= 10, "Did not switch min and max to produce the reversed range!");
            }
        }

        [TestMethod]
        public void NextCharValidChars()
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < 500; i++)
            {
                Assert.IsTrue(chars.Contains(RandomHelper.NextChar()));
            }
        }

        [TestMethod]
        public void NextNumberOfLengthZeroSize()
        {
            Assert.AreEqual(String.Empty, RandomHelper.NextNumberOfLength(0));
        }

        [TestMethod]
        public void NextNumberOfLengthNegativeSize()
        {
            Assert.AreEqual(String.Empty, RandomHelper.NextNumberOfLength(-10));
        }

        [TestMethod]
        public void NextNumberOfLengthValidSize()
        {
            var res = RandomHelper.NextNumberOfLength(5);
            Assert.AreEqual(5, res.Length);
            Assert.IsTrue(new Regex(@"^[0-9]{5}$").IsMatch(res));
        }

        [TestMethod]
        public void LongRandomRangeTest ()
        {
            //Test with no range (min = max)
            Assert.AreEqual(1, RandomHelper.LongRandom(1, 1));
            Assert.AreEqual(-1, RandomHelper.LongRandom(-1, -1));
            //Test generating long values
            Assert.AreEqual(3000000000L, RandomHelper.LongRandom(3000000000L, 3000000000L));
        }

        [TestMethod]
        public void LongRandomDistributionTest ()
        {
            //Test for favoring certain values.
            int[] results = new int[1000];
            for (long i=0; i < 1000000; i++)
            {
                long r = RandomHelper.LongRandom(3000000000L, 3000001000L);
                results[r - 3000000000L] += 1;
            }
            List<int> res = new List<int>(results);
            double avg = res.Sum() / res.Count;
            double stddev = Math.Sqrt(res.Select(t=>(t - avg)*(t - avg)).Sum()/res.Count) ;
            for (long i=0; i < 1000; i++){
                Assert.IsTrue(results[i] > 1000 - 4.417173 * stddev && results[i] < 1000 + 4.417173 * stddev, 
                    String.Concat("Results with value ", i, " are more than 4.417173 standard deviations from the average ",
                                  "(1% chance of happening in a test of 1000 values for truly random values) "));
            }
        }

        [TestMethod]
        public void TypeSafeNextByte()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(byte)), byte.MaxValue);
        }

        [TestMethod]
        public void TypeSafeNextSByte()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(sbyte)), sbyte.MaxValue);
        }

        [TestMethod]
        public void TypeSafeNextShort()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(short)), short.MaxValue);
        }

        [TestMethod]
        public void TypeSafeNextUShort()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(ushort)), ushort.MaxValue);
        }

        [TestMethod]
        public void TypeSafeNextInt()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(int)), int.MaxValue);
        }

        [TestMethod]
        public void TypeSafeNextUInt()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(uint)), uint.MaxValue);
        }

        [TestMethod]
        public void TypeSafeNextLong()
        {
            Assert.AreEqual(RandomHelper.TypeSafeNext(long.MaxValue, long.MaxValue, typeof(long)), long.MaxValue);
        }
    }
}
