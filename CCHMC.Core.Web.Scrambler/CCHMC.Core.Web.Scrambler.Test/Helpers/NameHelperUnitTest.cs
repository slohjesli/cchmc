using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class NameHelperUnitTest
    {
        [TestMethod]
        public void GenerateName_FemaleNamesConsistentGeneration ()
        {
            Name name;

            for (int i=0; i < 100; i++)
            {
                name = NameHelper.GenerateName(ScrambleNameAttribute.Gender.Female);
                CollectionAssert.Contains(NameHelper.FemaleFNames, name.FirstName);
                CollectionAssert.Contains(NameHelper.FemaleMNames, name.MiddleName);
                CollectionAssert.Contains(NameHelper.LNames, name.LastName);
            }

        }
        
        [TestMethod]
        public void GenerateName_MaleNamesConsistentGeneration ()
        {
            Name name;

            for (int i=0; i < 100; i++)
            {
                name = NameHelper.GenerateName(ScrambleNameAttribute.Gender.Male);
                CollectionAssert.Contains(NameHelper.MaleFNames, name.FirstName);
                CollectionAssert.Contains(NameHelper.MaleMNames, name.MiddleName);
                CollectionAssert.Contains(NameHelper.LNames, name.LastName);
            }

        }
        
        [TestMethod]
        public void GenerateName_RandomNamesDistributionGeneration ()
        {
            Name name;

            Dictionary<string, int> distro = new Dictionary<string, int>();
            distro.Add("female", 0);
            distro.Add("male", 0);
            distro.Add("failure", 0);

            for (int i=0; i < 1000; i++)
            {
                name = NameHelper.GenerateName(ScrambleNameAttribute.Gender.Random);

                if (NameHelper.FemaleFNames.Contains(name.FirstName) && NameHelper.FemaleMNames.Contains(name.MiddleName))
                {
                    distro["female"]++;
                } else if (NameHelper.MaleFNames.Contains(name.FirstName) && NameHelper.MaleMNames.Contains(name.MiddleName))
                {
                    distro["male"]++;
                } else
                {
                    distro["failure"]++;
                }
            }
            Assert.AreEqual(0, distro["failure"], "Some names were not all-male or all-female!");
            Assert.IsTrue(distro["female"] > 400 && distro["female"] < 600, String.Format("Unusual number of female names. ({0})", distro["female"]));
            Assert.IsTrue(distro["male"] > 400 && distro["male"] < 600, String.Format("Unusual number of male names. ({0})", distro["male"]));

        }

        [TestMethod]
        public void NoNamesInFormatTest ()
        {
            Name name = new Name("Adrianna", "Lianne", "Ellis");
            
            Assert.AreEqual("", NameHelper.Format("", name));
            //Test with characters used for replacement markers, but not in the correct sequence.
            Assert.AreEqual("}}{I}FF}F", NameHelper.Format("}}{I}FF}F", name));
        }

        [TestMethod]
        public void SinglePartOfNameTest ()
        {
            Name name = new Name("Adrianna", "Lianne", "Ellis");
            
            //Make sure each replacement works individually.
            Assert.AreEqual("Adrianna", NameHelper.Format("{F}", name));
            Assert.AreEqual("Lianne", NameHelper.Format("{M}", name));
            Assert.AreEqual("Ellis", NameHelper.Format("{L}", name));
            Assert.AreEqual("A", NameHelper.Format("{FI}", name));
            Assert.AreEqual("L", NameHelper.Format("{MI}", name));
            Assert.AreEqual("E", NameHelper.Format("{LI}", name));
        }

        [TestMethod]
        public void RepeatNameTest ()
        {
            Name name = new Name("Adrianna", "Lianne", "Ellis");

            //Test names with themselves.
            Assert.AreEqual("Adrianna Adrianna", NameHelper.Format("{F} {F}", name));
            Assert.AreEqual("Lianne Lianne", NameHelper.Format("{M} {M}", name));
            Assert.AreEqual("Ellis Ellis", NameHelper.Format("{L} {L}", name));
            Assert.AreEqual("A A", NameHelper.Format("{FI} {FI}", name));
            Assert.AreEqual("L L", NameHelper.Format("{MI} {MI}", name));
            Assert.AreEqual("E E", NameHelper.Format("{LI} {LI}", name));
        }

        [TestMethod]
        public void MixNameAndOtherCharactersTest ()
        {
            Name name = new Name("Adrianna", "Lianne", "Ellis");
            
            //Test names with other characters
            Assert.AreEqual("(Adrianna)", NameHelper.Format("({F})", name));
            Assert.AreEqual("Ellis, Adrianna", NameHelper.Format("{L}, {F}", name));
            Assert.AreEqual("Adrianna Ellis Lianne Adrianna HelpI'mtrappedinanamefactory Lianne Adrianna Ellis Lianne...", NameHelper.Format("{F} {L} {M} {F} HelpI'mtrappedinanamefactory {M} {F} {L} {M}...", name));
            Assert.AreEqual("1, 2, buckle my shoe; A, E, look at that bee!", NameHelper.Format("1, 2, buckle my shoe; {FI}, {LI}, look at that bee!", name));
            Assert.AreEqual("OLd MAcdonALd hAd A fArm, E I E I O", NameHelper.Format("O{MI}d M{FI}cdon{FI}{MI}d h{FI}d {FI} f{FI}rm, {LI} I {LI} I O", name));
        }

        [TestMethod]
        public void CoexistingNamesTest ()
        {
            Name name = new Name("Adrianna", "Lianne", "Ellis");
            
            //Test names with each other
            Assert.AreEqual("Adrianna Lianne Ellis (ALE)", NameHelper.Format("{F} {M} {L} ({FI}{MI}{LI})", name));
        }

        [TestMethod]
        public void NestingTest ()
        {
            Name name = new Name("Adrianna", "Lianne", "Ellis");
            
            //Test nesting
            Assert.AreEqual("{AI}", NameHelper.Format("{{FI}I}", name));
            Assert.AreEqual("{LI}", NameHelper.Format("{{MI}I}", name));
            Assert.AreEqual("{EI}", NameHelper.Format("{{LI}I}", name));
            Assert.AreEqual("{A}", NameHelper.Format("{{FI}}", name));
            Assert.AreEqual("{L}", NameHelper.Format("{{MI}}", name));
            Assert.AreEqual("{E}", NameHelper.Format("{{LI}}", name));
        }
    }
}
