using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// Attribute to obfuscate strings.
    /// </summary>
    public class ScrambleTextAttribute : ScrambleAttribute
    {
        /// <summary>
        /// The sizes available for generating text.
        /// </summary>
        public enum Size { Word, Sentence, Paragraph };

        /// <summary>
        /// Creates a string containing an amount of text to obfuscate a field or property.
        /// </summary>
        /// <param name="strict">If true, the amount of text generated with reflect the length of the obfuscated text. Otherwise, it will be one sentence.</param>
        public ScrambleTextAttribute(bool strict = false)
        {
            IsStrict = strict;
            if (!IsStrict)
            {
                _obfuscate = LoremIpsumHelper.GetSentences();
            }
        }

        /// <summary>
        /// Creates a string containing a word, sentence, or paragraph to obfuscate a field or property.
        /// </summary>
        /// <param name="size">Specifies what should be generated in the given count; either Word, Sentence, or Paragraph.</param>
        public ScrambleTextAttribute (Size size)
        {
            if (size == Size.Paragraph)//paragraphs)
                _obfuscate = LoremIpsumHelper.GetParagraphs();
            else if (size == Size.Sentence)
                _obfuscate = LoremIpsumHelper.GetSentences();
            else
                _obfuscate = LoremIpsumHelper.GetWords();
        }

        /// <summary>
        /// Creates a string containing a number of sentences or paragraphs to obfuscate a field or property.
        /// </summary>
        /// <param name="count">The number of sentences or paragraphs to be generated.</param>
        /// <param name="size">Specifies what should be generated in the given count; either Word, Sentence, or Paragraph.</param>
        public ScrambleTextAttribute(int count, Size size)
        {
            if (size == Size.Paragraph)//paragraphs)
                _obfuscate = LoremIpsumHelper.GetParagraphs(count);
            else if (size == Size.Sentence)
                _obfuscate = LoremIpsumHelper.GetSentences(count);
            else
                _obfuscate = LoremIpsumHelper.GetWords(count);
        }

        public override object Obfuscate(object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                if (IsStrict)
                {
                    //Strict is not a default setting; it can be set with "ScrambleTextAttribute(Strict=true).
                    //Should Strict be a more standard option over the paragraph option?
                    //Check if it's a char or a string.
                    if (obj is char)
                    {
                        _obfuscate = RandomHelper.NextChar();
                    }
                    else if (obj is string)
                    {
                        //Todo: Move this to another function to reduce code block size

                        //Check the size of the string being obfuscated.
                        //Estimate about 10 words per sentence; about 10 sentences per paragraph. 
                        //todo: This is probably excessive in terms of paragraphs; a lower threshold may be preferable.
                        //Use words/sentences plus up to 10 to have some degree of randomness to the length while still having a close approximation.
                        var strObj = obj as string;
                        int words = 1 + strObj.Count(t => t == ' ');
                        int sentences = (words + RandomHelper.Random.Next(10)) / 10;
                        int paragraphs = (sentences + RandomHelper.Random.Next(10)) / 10;
                        //Check which is the correct size.
                        if (paragraphs > 1)
                        {
                            _obfuscate = LoremIpsumHelper.GetParagraphs(paragraphs);
                        }
                        else if (sentences > 1)
                        {
                            _obfuscate = LoremIpsumHelper.GetSentences(sentences);
                        }
                        else
                        {
                            _obfuscate = LoremIpsumHelper.GetWords(words);
                        }
                    }
                } else
                {
                    if (obj is char)
                        _obfuscate = RandomHelper.NextChar();
                    else if (obj is string)
                        _obfuscate = LoremIpsumHelper.GetSentences();
                }
            }
            return _obfuscate;
        }
    }
}
