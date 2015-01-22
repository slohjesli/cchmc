using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// Helper used to determine the best obfuscator to use for a given object.
    /// </summary>
    internal static class DetectionHelper
    {
        //Patterns: 
        //(_), (___) //This can't really be detected. It's essentially a word or character (though the former can be suitably replaced regardless)
        //(_ _), (_, _) //This is more detectable; in any case, it can be shuffled to something suitable even if it isn't actually a name, since it is just initials.
        //(_ ___), (___, _)
        //(___ _), (_, ___)
        //(___ ___), (___, ___)
        //(_ _ _), (_, _ _)//Again, since these are just initials, it can easily be replaced regardless of whether or not it is a name.
        //(_ _ ___), (___, _ _)
        //(___ _ ___), (___, ___ _)
        //(___ ___ ___), (___, ___ ___)

        private static string[] ThreePartNameFormat(string[] names)
        {
            if (names[0].Last() == ',')
            {
                //It's a last name, then the first name
                if (names[0].Count() > 2)
                    names[0] = "{L},";
                else
                    names[0] = "{LI},";
                if (names[1].Count() > 2)
                    names[1] = "{F}";
                else
                    names[1] = "{FI}";
                if (names[2].Count() > 2)
                    names[2] = "{L}";
                else
                    names[2] = "{LI}";
            }
            else
            {
                //It's first, middle, last
                if (names[0].Count() > 1)
                    names[0] = "{F}";
                else
                    names[0] = "{FI}";
                if (names[1].Count() > 1)
                    names[1] = "{M}";
                else
                    names[1] = "{MI}";
                if (names[2].Count() > 1)
                    names[2] = "{L}";
                else
                    names[2] = "{LI}";
            }
            return names;
        }

        /// <summary>
        /// Checks a string for several possible formats for a name and returns a matching format.
        /// </summary>
        /// <param name="obj">The string with the name to match.</param>
        /// <returns>A format to be used to creata a String name.</returns>
        public static string DetectNameFormat (string obj)
        {
            string format = String.Empty;
            if (obj.Count(t => t == ' ') <= 2)
            {
                string[] names = obj.Split(new char[] { ' ' });

                switch (names.Count())
                {
                    case 1:
                        //Only one name; check if it's an initial.
                        if (names[0].Length == 1)
                            names[0] = "{FI}";
                        else
                            names[0] = "{F}";
                        break;
                    case 2:
                        //Two names; check if either is an initial and check for commas to indicate a last name.
                        if (names[0].Last() == ',')
                        {
                            //It's a last name, then the first name
                            if (names[0].Count() > 2)
                                names[0] = "{L},";
                            else
                                names[0] = "{LI},";
                            if (names[1].Count() > 2)
                                names[1] = "{F}";
                            else
                                names[1] = "{FI}";
                        } else
                        {
                            //It's first, then last
                            if (names[0].Count() > 1)
                                names[0] = "{F}";
                            else
                                names[0] = "{FI}";
                            if (names[1].Count() > 1)
                                names[1] = "{L}";
                            else
                                names[1] = "{LI}";
                        }
                        break;
                    case 4:
                        //Four names; this implies that there are two middle names, a maiden name is included, etc. 
                        //Since the Name class is only intended to handle three parts of a name, remove one of the names and treat it as though it were three.
                        //Check if the second name has a comma; if it does, then the extra name is probably a second last name at the beginning, so remove it. 
                        if (names[1].Last() == ',')
                        {
                            names = new string[] { names[1], names[2], names[3] };
                        }
                        else
                        {
                            //Otherwise, remove the third element, since it is likely that that's the extra name.
                            names = new string[] { names[0], names[1], names[3] };
                        }
                        names = ThreePartNameFormat(names);
                        break;
                    case 3:
                        //Three names; check if the first and middle are initials, if the last is an initial, or if all three are initials and check for commas to indicate the last name.
                        names = ThreePartNameFormat(names);
                        break;
                    default:
                        break;
                }

                format = String.Join(" ", names);
            }
            return format;
        }
    }
}
