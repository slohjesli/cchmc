using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Attributes
{
    [TestClass]
    public class ScrambleTextAttributeUnitTest
    {
        Regex se = new Regex(@" ");
        Regex re = new Regex(@"\. ");
        Regex para = new Regex(@"\n\r");

        [TestMethod]
        public void DefaultObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute();
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.IsTrue(re.Matches(scr.Obfuscate(String.Empty) as String).Count == 1, "Too many sentences in obfuscation!");
        }

        [TestMethod]
        public void StrictWordObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(true);
            Assert.IsTrue(se.Matches(scr.Obfuscate("Hello") as String).Count == 0, "Too many words in obfuscation!");
        }

        [TestMethod]
        public void StrictWordsObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(true);
            Assert.IsTrue(se.Matches(scr.Obfuscate("Hello good sir") as String).Count == 2, "Incorrect number of words in obfuscation!");
        }

        [TestMethod]
        public void StrictSentenceObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(true);
            var tmp = scr.Obfuscate("This needs to be at least ten words for a sentence.");
            Assert.IsTrue(re.Matches(tmp as String).Count == 1 || re.Matches(tmp as String).Count == 2, String.Format("Too many sentences in obfuscation! ({0})", re.Matches(tmp as String).Count));
        }

        [TestMethod]
        public void StrictSentencesObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(true);
            var tmp = scr.Obfuscate("This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence.");
            Assert.IsTrue(re.Matches(tmp as String).Count == 3 || re.Matches(tmp as String).Count == 4, "Incorrect number of sentences in obfuscation!");
        }

        [TestMethod]
        public void StrictParagraphObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(true);
            scr.Obfuscate("This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence.");
            Assert.IsTrue(re.Matches(scr.Obfuscate(String.Empty) as String).Count >= 6, String.Format("Too few sentences in obfuscation! ({0})", re.Matches(scr.Obfuscate(String.Empty) as String).Count));
            Assert.IsTrue(para.Matches(scr.Obfuscate(String.Empty) as String).Count == 0 || para.Matches(scr.Obfuscate(String.Empty) as String).Count == 1, "Too many paragraphs in obfuscation!");
        }

        [TestMethod]
        public void StrictParagraphsObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(true);
            var tmp = scr.Obfuscate("This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence. This needs to be at least ten words for a sentence.");
            Assert.IsTrue(para.Matches(tmp as String).Count == 2 || para.Matches(tmp as String).Count == 3, "Incorrect number of paragraphs in obfuscation!");
        }

        [TestMethod]
        public void NonstrictObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute();
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.IsTrue(re.Matches(scr.Obfuscate(String.Empty) as String).Count == 1, "Too many sentences in obfuscation!");
        }

        [TestMethod]
        public void WordObfuscation()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(ScrambleTextAttribute.Size.Word);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual(0, se.Matches(scr.Obfuscate(String.Empty) as String).Count, "Too many words in obfuscation!");
            Assert.IsTrue((scr.Obfuscate(String.Empty) as string).Length > 0);
        }
        
        [TestMethod]
        public void ParagraphObfuscation ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(ScrambleTextAttribute.Size.Paragraph);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.IsTrue(re.Matches(scr.Obfuscate(String.Empty) as String).Count > 1);
            Assert.AreEqual(0, para.Matches(scr.Obfuscate(String.Empty) as String).Count, "Too many paragraphs in obfuscation!");
        }
        
        [TestMethod]
        public void SentenceObfuscation ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(ScrambleTextAttribute.Size.Sentence);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual(1, re.Matches(scr.Obfuscate(String.Empty) as String).Count, "Too many sentences in obfuscation!");
        }
        
        [TestMethod]
        public void NegativeWords ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(-5, ScrambleTextAttribute.Size.Word);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual("", scr.Obfuscate(String.Empty));
        }
        
        [TestMethod]
        public void ZeroWords ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(0, ScrambleTextAttribute.Size.Word);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual("", scr.Obfuscate(String.Empty));
        }
        
        [TestMethod]
        public void MultipleWords ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(5, ScrambleTextAttribute.Size.Word);
            Assert.AreEqual(4, se.Matches(scr.Obfuscate(String.Empty) as String).Count, "Incorrect number of word in obfuscation!");
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
        }
        
        [TestMethod]
        public void NegativeNumberOfParagraphs ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(-5, ScrambleTextAttribute.Size.Paragraph);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual("", scr.Obfuscate(String.Empty));
        }
        
        [TestMethod]
        public void ZeroParagraphs ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(0, ScrambleTextAttribute.Size.Paragraph);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual("", scr.Obfuscate(String.Empty));
        }
        
        [TestMethod]
        public void MultipleParagraphs ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(5, ScrambleTextAttribute.Size.Paragraph);
            Assert.IsTrue(para.Matches(scr.Obfuscate(String.Empty) as String).Count == 4, "Incorrect number of paragraphs in obfuscation!");
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
        }
        
        [TestMethod]
        public void NegativeNumberOfSentences ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(-5, ScrambleTextAttribute.Size.Sentence);
            Assert.AreEqual("", scr.Obfuscate(String.Empty));
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
        }
        
        [TestMethod]
        public void ZeroSentences ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(0, ScrambleTextAttribute.Size.Sentence);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.AreEqual("", scr.Obfuscate(String.Empty));
        }
        
        [TestMethod]
        public void MultipleSentences ()
        {
            ScrambleTextAttribute scr = new ScrambleTextAttribute(5, ScrambleTextAttribute.Size.Sentence);
            Assert.AreEqual(typeof(string), scr.Obfuscate(String.Empty).GetType());
            Assert.IsTrue(re.Matches(scr.Obfuscate(String.Empty) as String).Count == 5, "Incorrect number of sentences in obfuscation!");
        }
    }
}
