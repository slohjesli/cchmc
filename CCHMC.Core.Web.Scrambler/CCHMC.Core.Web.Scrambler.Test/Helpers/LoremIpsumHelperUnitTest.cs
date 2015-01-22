using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class LoremIpsumHelperUnitTest
    {
        string WordSeparator = " ";
        string SentenceSeparator = @"\. ";
        string ParagraphSeparator = "\n\r";

        [TestMethod]
        public void NextGetsSentence()
        {
            Assert.AreEqual(1, Regex.Matches(LoremIpsumHelper.Next(), SentenceSeparator).Count);
        }

        [TestMethod]
        public void NextIncrements()
        {
            string sentence = LoremIpsumHelper.Next();
            string nextsentence = LoremIpsumHelper.Next();
            Assert.AreNotEqual(sentence, nextsentence);
        }

        [TestMethod]
        public void NextRepeats()
        {
            List<string> sentences = new List<string>();
            string sentence = LoremIpsumHelper.Next();
            //Get all of the sentences
            while (!sentences.Contains(sentence))
            {
                sentences.Add(sentence);
                sentence = LoremIpsumHelper.Next();
            }
            //Cycle through again to verify equality and repitition.
            foreach(string s in sentences)
            {
                Assert.AreEqual(s, sentence);
                sentence = LoremIpsumHelper.Next();
            }
        }

        [TestMethod]
        public void GetWordTest()
        {
            string word = LoremIpsumHelper.GetWords();
            Assert.AreEqual(0, Regex.Matches(word, WordSeparator).Count);
            Assert.AreNotEqual(String.Empty, word);
        }

        [TestMethod]
        public void GetWordsTest()
        {
            Assert.AreEqual(0, Regex.Matches(LoremIpsumHelper.GetWords(0), WordSeparator).Count);
            for (int i = 1; i < 20; i++){
                var words = LoremIpsumHelper.GetWords(i);
                Assert.AreEqual(i - 1, Regex.Matches(words, WordSeparator).Count, String.Format("{0}", words));
            }
        }
        
        [TestMethod]
        public void GetSentenceTest ()
        {
            Assert.AreEqual(1, Regex.Matches(LoremIpsumHelper.GetSentences(), SentenceSeparator).Count);
        }
        
        [TestMethod]
        public void GetSentencesTest ()
        {
            for (int i=0; i < 500; i++)
                Assert.AreEqual(i, Regex.Matches(LoremIpsumHelper.GetSentences(i), SentenceSeparator).Count);
        }
        
        [TestMethod]
        public void GetParagraphTest ()
        {
            Assert.AreEqual(0, Regex.Matches(LoremIpsumHelper.GetParagraphs(), ParagraphSeparator).Count);
        }
        
        [TestMethod]
        public void GetParagraphsTest ()
        {
            Assert.AreEqual(0, Regex.Matches(LoremIpsumHelper.GetParagraphs(0), "\n\r").Count);
            for (int i=1; i < 20; i++)
                Assert.AreEqual(i - 1, Regex.Matches(LoremIpsumHelper.GetParagraphs(i), "\n\r").Count);
        }
    }
}
