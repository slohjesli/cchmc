using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Attributes;

namespace CCHMC.Core.Web.Scrambler.Test.Attributes
{
    [TestClass]
    public class ScrambleIgnoreAttributeUnitTest
    {
        [TestMethod]
        public void Constructor()
        {
            ScrambleIgnoreAttribute scram = new ScrambleIgnoreAttribute();
            Assert.IsTrue(scram.Ignore, "Initializing a ScrambleIgnore attribute did not set the Ignore flag to true!");
        }
    }
}
