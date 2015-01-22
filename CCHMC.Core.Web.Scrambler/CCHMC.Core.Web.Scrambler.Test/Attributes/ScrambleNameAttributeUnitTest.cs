using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Attributes
{
    [TestClass]
    public class ScrambleNameAttributeUnitTest
    {
        Regex FullName = new Regex(@"^[A-Z][a-z]+ [A-Z][a-z]+ [A-Z][A-Za-z]+$");
        Regex Initials = new Regex(@"^[A-Z] [A-Z] [A-Z]$");
        Regex FullWithInit = new Regex(@"^[A-Z][a-z]+ [A-Z] [A-Z][A-Za-z]+$");

        [TestMethod]
        public void StrictStringObfuscation () 
        { 
            ScrambleNameAttribute scr = new ScrambleNameAttribute(true);
            
            scr.Obfuscate("Joshua T Ellis");
            Assert.AreNotEqual("Joshua T Ellis", scr.Obfuscate(String.Empty));
            Assert.IsTrue(FullWithInit.IsMatch((string)scr.Obfuscate(String.Empty)));
        }

        [TestMethod]
        public void StrictListObfuscation()
        {
            ScrambleNameAttribute scr = new ScrambleNameAttribute(true);
            var test = new List<string> { "John", "Jacob", "Jingleheimer", "Schmidt" };
            CollectionAssert.AreNotEqual(new List<string> { "John", "Jacob", "Jingleheimer", "Schmidt" }, (List<string>)scr.Obfuscate(test));
            foreach (var i in (List<string>)scr.Obfuscate(String.Empty))
            {
                Assert.IsTrue(FullWithInit.IsMatch(i));
            }
        }

        [TestMethod]
        public void StrictArrayObfuscation()
        {
            ScrambleNameAttribute scr = new ScrambleNameAttribute(true);
            var test = new string[] { "John", "Jacob", "Jingleheimer", "Schmidt" };
            CollectionAssert.AreNotEqual(new string[] { "John", "Jacob", "Jingleheimer", "Schmidt" }, (string[])scr.Obfuscate(test));
            foreach (var i in (string[])scr.Obfuscate(String.Empty))
            {
                Assert.IsTrue(FullWithInit.IsMatch(i));
            }
        }

        [TestMethod]
        public void FormatFullNameTest ()
        {
            ScrambleNameAttribute scr;

            scr = new ScrambleNameAttribute("{F} {M} {L}");
            Assert.IsNotNull(scr.Obfuscate(String.Empty) as string);
            Assert.IsTrue(FullName.Matches(scr.Obfuscate(String.Empty) as string).Count == 1);
        }

        [TestMethod]
        public void FormatFullInitialsTest ()
        {
            ScrambleNameAttribute scr = new ScrambleNameAttribute("{FI} {MI} {LI}");

            Assert.IsNotNull(scr.Obfuscate(String.Empty) as string);
            Assert.IsTrue(Initials.Matches(scr.Obfuscate(String.Empty) as string).Count == 1);
        }

        [TestMethod]
        public void GenerateFemaleNameTest ()
        {
            ScrambleNameAttribute scr = new ScrambleNameAttribute("{F}", ScrambleNameAttribute.Gender.Female);
            Assert.IsNotNull(scr.Obfuscate(String.Empty) as string);
            CollectionAssert.Contains(NameHelper.FemaleFNames, scr.Obfuscate(String.Empty));
        }

        [TestMethod]
        public void GenerateMaleNameTest ()
        {
            ScrambleNameAttribute scr = new ScrambleNameAttribute("{F}", ScrambleNameAttribute.Gender.Male);
            Assert.IsNotNull(scr.Obfuscate(String.Empty) as string);
            CollectionAssert.Contains(NameHelper.MaleFNames, scr.Obfuscate(String.Empty));
        }

        [TestMethod]
        public void DefaultDistributionTest ()
        {
            ScrambleNameAttribute scr;
            string male = "male",
                female = "female";
            Dictionary<string, int> results = new Dictionary<string,int>{ { female, 0 }, { male, 0 } };

            for (int i=0; i < 10000; i++)
            {
                scr = new ScrambleNameAttribute();
                Assert.IsTrue(FullWithInit.Matches(scr.Obfuscate(String.Empty) as string).Count == 1, scr.Obfuscate(String.Empty) as string);
                var nms = (scr.Obfuscate(String.Empty) as string).Split(" ".ToCharArray());
                CollectionAssert.Contains(NameHelper.FemaleFNames.Union(NameHelper.MaleFNames).ToList(), nms[0], scr.Obfuscate(String.Empty) as string);
                
                if (NameHelper.FemaleFNames.Contains(nms[0]))
                {
                    results[female] += 1;
                } else if (NameHelper.MaleFNames.Contains(nms[0]))
                {
                    results[male] += 1;
                } else
                {
                    Assert.IsTrue(false, "Generated a name which is not in the NameHelper dictionaries!");
                }
            }
            double stddev = 50;//Math.Sqrt(np(1-p)) = Math.Sqrt(10000 * .5 * .5) = .5 * Math.Sqrt(10000) = .5 * 100 = 50
            Assert.IsTrue(5000 - 2.56 * stddev <= results[female] && results[female] <= 5000 + 2.56 * stddev, String.Format("Unusual number of female names generated! ({0} / 10000)", results[female]));
            Assert.IsTrue(5000 - 2.56 * stddev <= results[male] && results[male] <= 5000 + 2.56 * stddev, String.Format("Unusual number of male names generated! ({0} / 10000)", results[male]));
        }

        [TestMethod]
        public void NotStrictDistributionTest ()
        {
            ScrambleNameAttribute scr;
            string male = "male", 
                female = "female";
            Dictionary<string, int> results = new Dictionary<string,int>{ { female, 0 }, { male, 0 } };

            for (int i=0; i <10000; i++)
            {
                scr = new ScrambleNameAttribute(false);
                var nms = (scr.Obfuscate(string.Empty) as string).Split(" ".ToCharArray());
                CollectionAssert.Contains(NameHelper.FemaleFNames.Union(NameHelper.MaleFNames).ToList(), nms[0]);

                if (NameHelper.FemaleFNames.Contains(nms[0]))
                {
                    results[female] += 1;
                } else if (NameHelper.MaleFNames.Contains(nms[0]))
                {
                    results[male] += 1;
                } else
                {
                    Assert.IsTrue(false, "Generated a name which is not in the NameHelper dictionaries!");
                }
            }
            double stddev = 50;//Math.Sqrt(np(1-p)) = Math.Sqrt(10000 * .5 * .5) = .5 * Math.Sqrt(10000) = .5 * 100 = 50
            Assert.IsTrue(5000 - 2.56 * stddev <= results[female] && results[female] <= 5000 + 2.56 * stddev, String.Format("Unusual number of female names generated! ({0} / 10000)", results[female]));
            Assert.IsTrue(5000 - 2.56 * stddev <= results[male] && results[male] <= 5000 + 2.56 * stddev, String.Format("Unusual number of male names generated! ({0} / 10000)", results[male]));
        }

        [TestMethod]
        public void RandomDistributionTest ()
        {
            ScrambleNameAttribute scr;
            string male = "male",
                female = "female";
            Dictionary<string, int> results = new Dictionary<string,int>{ { female, 0 }, { male, 0 } };

            for (int i=0; i <10000; i++)
            {
                scr = new ScrambleNameAttribute("{F}", ScrambleNameAttribute.Gender.Random);
                CollectionAssert.Contains(NameHelper.FemaleFNames.Union(NameHelper.MaleFNames).ToList(), scr.Obfuscate(String.Empty));
                if (NameHelper.FemaleFNames.Contains(scr.Obfuscate(String.Empty)))
                {
                    results[female] += 1;
                }
                else if (NameHelper.MaleFNames.Contains(scr.Obfuscate(String.Empty)))
                {
                    results[male] += 1;
                } else
                {
                    Assert.IsTrue(false, "Generated a name which is not in the NameHelper dictionaries!");
                }
            }
            double stddev = 50;//Math.Sqrt(np(1-p)) = Math.Sqrt(10000 * .5 * .5) = .5 * Math.Sqrt(10000) = .5 * 100 = 50
            Assert.IsTrue(5000 - 2.56 * stddev <= results[female] && results[female] <= 5000 + 2.56 * stddev, String.Format("Unusual number of female names generated! ({0} / 10000)", results[female]));
            Assert.IsTrue(5000 - 2.56 * stddev <= results[male] && results[male] <= 5000 + 2.56 * stddev, String.Format("Unusual number of male names generated! ({0} / 10000)", results[male]));
        }
    }
}
