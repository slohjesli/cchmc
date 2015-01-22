using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// Helper to generate Lorem Ipsum to fill blocks of text in strings.
    /// </summary>
    internal static class LoremIpsumHelper
    {
        private static readonly string WordSeparator = " ";
        private static readonly string ParagraphSeparator = "\n\r";

        /// <summary>
        /// The Lorem Ipsum library.
        /// </summary>
        private static List<string> LoremIpsumText = new List<string> {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. ",
            "Donec ut augue leo. ",
            "Proin quis tempor risus. ",
            "Praesent dignissim dui ante, eu vestibulum turpis laoreet eget. ",
            "Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. ",
            "Curabitur nec erat ut quam facilisis ultricies. ",
            "Sed congue neque at tortor egestas, nec vestibulum turpis consequat. ",
            "Praesent placerat, eros in condimentum varius, urna lacus rhoncus neque, quis accumsan odio diam vel lorem. ",
            "Nulla et magna at erat gravida volutpat et quis risus. ",
            "Mauris bibendum lobortis quam quis dictum. ",
            "Sed laoreet porta erat, at commodo turpis egestas eu. ",
            "Nulla auctor eu neque fringilla ornare. ",
            "In suscipit cursus massa vel iaculis. ",
            "Sed quam sapien, volutpat quis vehicula sit amet, scelerisque a eros. ",
            "Aliquam malesuada pretium lorem et varius. ",
            "Suspendisse potenti. ",
            "Fusce quis odio eu lectus posuere mattis facilisis ut odio. ",
            "Nullam ac molestie ligula. ",
            "Curabitur id odio pulvinar lorem commodo porttitor pellentesque quis nibh. ",
            "Nam vel accumsan ante, at adipiscing metus. ",
            "Curabitur ac quam vel lectus pharetra vehicula. ",
            "Fusce arcu velit, congue a dictum eu, facilisis a orci. ",
            "Duis placerat, lectus viverra porttitor tempor, lorem neque venenatis tortor, id tincidunt leo ante ut urna. ",
            "Nam molestie faucibus mauris. ",
            "Nunc suscipit a arcu nec ultrices. ",
            "Integer facilisis blandit est ut tincidunt. ",
            "Nunc elementum quam diam, ut vestibulum tellus tincidunt ac. ",
            "Proin convallis nunc felis, id aliquam lorem accumsan quis. ",
            "Pellentesque at nunc posuere odio blandit egestas. ",
            "Aenean placerat aliquet mi, et egestas neque pretium quis. ",
            "Sed vel magna lacus. ",
            "Sed in odio in nibh tempus scelerisque at eu felis. ",
            "Nam quis elementum quam, volutpat sodales justo. ",
            "Pellentesque gravida mi a orci commodo aliquam. ",
            "Maecenas ultricies quam sapien, sit amet malesuada ligula auctor egestas. ",
            "Maecenas id facilisis dolor. ",
            "Proin dictum justo mi, eu eleifend ligula dictum ut. ",
            "Fusce venenatis orci nec dui scelerisque, tristique lobortis nibh viverra. ",
            "Curabitur vitae odio sollicitudin, tincidunt nunc eget, tincidunt diam. ",
            "Nulla pellentesque urna id nunc elementum semper. ",
            "Phasellus sodales, nisi nec pulvinar accumsan, purus turpis commodo justo, quis sagittis urna eros non nulla. ",
            "Sed tincidunt enim adipiscing ante iaculis bibendum. ",
            "In rutrum hendrerit congue. ",
            "Vestibulum quam augue, cursus sed rhoncus sit amet, eleifend vel nisl. ",
            "Vivamus scelerisque vestibulum nibh nec lobortis. ",
            "Sed eleifend, turpis eu vulputate tempor, nulla nulla gravida lorem, quis consequat erat arcu sed elit. ",
            "Nullam rhoncus viverra est vel aliquet. ",
            "Suspendisse pharetra felis hendrerit felis iaculis, vitae vehicula leo adipiscing. ",
            "Aliquam at vulputate lorem, sit amet imperdiet elit. ",
            "Ut viverra pulvinar dolor non pharetra. ",
            "Curabitur vitae lectus imperdiet, sagittis nibh at, placerat diam. ",
            "Morbi in libero placerat, lobortis eros ac, rutrum nibh. ",
            "Sed vestibulum tempus justo, eget faucibus magna pretium in. ",
            "Duis eros sapien, euismod quis nunc eget, placerat posuere purus. ",
            "Cras id egestas lacus. ",
            "In hac habitasse platea dictumst. ",
            "Duis et lacus a nisl mattis accumsan. ",
            "Duis luctus ligula et lobortis fermentum. ",
            "Phasellus tempus ligula at ultricies laoreet. ",
            "Mauris nisl sem, adipiscing eu gravida ac, vehicula sit amet purus. ",
            "Maecenas porta, urna sit amet dapibus laoreet, urna dolor semper odio, at tincidunt magna nunc vitae diam. ",
            "Mauris auctor odio quis mollis aliquet. ",
            "Ut interdum ipsum arcu, auctor commodo odio adipiscing in. ",
            "Duis nec est ac elit vulputate adipiscing. ",
            "Supesse nisl sed tortor potenti. ",
            "Donec interdum vel nisl sit amet interdum. ",
            "Vestibulum a suscipit tortor. ",
            "Vivamus at condimentum risus. ",
            "In at fringilla nunc. ",
            "Ut ultricies eros sit amet risus vestibulum, et ultricies urna lobortis. ",
            "Ut auctor dolor in augue luctus, quis auctor ante porta. ",
            "Donec sapien odio, pretium quis scelerisque vitae, consectetur et arcu. ",
            "Vivamus quis tortor massa. ",
            "Suspendisse auctor nisl sed tortor rhoncus aliquet. ",
            "Sed elementum imperdiet felis in commodo. ",
            "Aenean leo sapien, mattis sit amet lacinia quis, pellentesque venenatis odio. ",
            "Sed volutpat accumsan nisi, non fermentum neque tincidunt a. ",
            "Etiam congue odio et ante interdum sollicitudin. ",
            "Proin sed magna vel neque suscipit aliquet. ",
            "Etiam elementum elementum nisl, in porta nisl rutrum eget. ",
            "Cras bibendum pharetra lorem, pulvinar faucibus metus varius at. ",
            "Donec laoreet, nisi in auctor sollicitudin, diam felis ultrices diam, eget volutpat ipsum augue id mauris. ",
            "Maecenas ante enim, cursus id fermentum vel, semper a turpis. ",
            "Vestibulum ac dapibus libero. ",
            "Mauris vulputate quam gravida ipsum lacinia commodo. ",
            "Integer mattis pulvinar pellentesque. ",
            "Duis commodo elit vel auctor ullamcorper. ",
            "Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Integer id leo nec nisl suscipit semper ac sed tellus. ",
            "Curabitur sollicitudin ipsum ut ante dignissim, ac sollicitudin nisi adipiscing. ",
            "Phasellus interdum ultricies pellentesque. ",
            "Phasellus vitae eleifend mi, sed eleifend lectus. ",
            "Duis porttitor ante convallis metus pretium, eget sagittis tellus ullamcorper. ",
            "In lacus urna, auctor sit amet est nec, gravida porttitor enim. ",
            "Nulla metus enim, accumsan et nibh et, ornare gravida tellus. ",
            "Nam mollis mi id vestibulum consectetur. ",
            "Sed consequat purus non erat mattis tempus. ",
            "Maecenas consequat magna velit, non blandit tortor aliquam vehicula. ",
            "Vivamus vel metus iaculis dolor egestas consectetur. ",
            "Vestibulum nec mollis felis, ut faucibus risus. ",
            "Vestibulum imperdiet in est eu luctus. ",
            "Aenean vehicula pharetra nisl, a gravida dolor sagittis sit amet. ",
            "Aliquam mauris elit, lacinia eget fermentum quis, tincidunt in dolor. ",
            "Quisque pretium tortor et augue adipiscing, ut ullamcorper ante venenatis. ",
            "Suspendisse quis nibh cursus, vehicula lacus in, malesuada nunc. ",
            "In nec nulla pellentesque, viverra nunc vel, cursus sapien. ",
            "Nunc sollicitudin enim non dui elementum, ac auctor elit ornare. ",
            "Pellentesque dignissim diam a mauris sodales, nec luctus turpis accumsan. ",
            "Fusce non orci vehicula, scelerisque nulla quis, ultrices velit. ",
            "Cras facilisis nibh libero. ",
            "Aliquam malesuada nisi non erat feugiat, eu aliquet lectus vulputate. ",
            "Nulla at leo dui. ",
            "Nam sit amet vulputate libero. ",
            "Aenean lacus neque, adipiscing sit amet nibh a, sagittis feugiat ante. ",
            "Cras at justo ipsum. "
        };

        /// <summary>
        /// The index of the current sentence in the LoremIpsumText.
        /// </summary>
        private static int CurrentPosition = 0;

        /// <summary>
        /// Gets the next sentence from the LoremIpsumText.
        /// </summary>
        /// <returns>A string from the LoremIpsumText.</returns>
        internal static string Next ()
        {
            string ret = LoremIpsumText[CurrentPosition];
            CurrentPosition = (CurrentPosition + 1) % LoremIpsumText.Count();
            return ret;
        }

        /// <summary>
        /// Gets a number of words from the Lorem Ipsum.
        /// </summary>
        /// <param name="num">The number of words to generate.</param>
        /// <returns>A string with one or the specified number of words.</returns>
        public static string GetWords(int num = 1)
        {
            string ret = Next();
            while (ret.Count(t => t == ' ') + 1 < num)
            {
                ret += Next();
            }
            ret = String.Join(WordSeparator, ret.Split(WordSeparator.ToCharArray()).Take(num));

            return ret;
        }

        /// <summary>
        /// Gets a number of sentences of Lorem Ipsum.
        /// </summary>
        /// <param name="num">The number of sentences to retrieve.</param>
        /// <returns>A string containing a number of sentences.</returns>
        public static string GetSentences (int num = 1)
        {
            if (num < 0)
                num = 0;
            //Cycle through the list instead of creating permutations to avoid complications.
            List<string> sentences = new List<string>();
            while (num + CurrentPosition > LoremIpsumText.Count())
            {
                sentences.AddRange(LoremIpsumText.GetRange(CurrentPosition, LoremIpsumText.Count() - CurrentPosition));
                num -= (LoremIpsumText.Count() - CurrentPosition);
                CurrentPosition = 0;
            }
            sentences.AddRange(LoremIpsumText.GetRange(CurrentPosition, num));
            CurrentPosition = (CurrentPosition + num) % LoremIpsumText.Count();

            return String.Join(String.Empty, sentences);
        }

        /// <summary>
        /// Gets a number of paragraphs of Lorem Ipsum, separated by carriage returns.
        /// </summary>
        /// <param name="num">The number of paragraphs to return.</param>
        /// <returns>A number of paragraphs of Lorem Ipsum, separated by carriage returns.</returns>
        public static string GetParagraphs(int num = 1)
        {
            List<string> paragraphs = new List<string>();

            for (int i=0; i < num; i++)
            {
                paragraphs.Add(GetSentences(RandomHelper.Random.Next(4, 8)));
            }

            return String.Join(ParagraphSeparator, paragraphs);
        }
    }
}
