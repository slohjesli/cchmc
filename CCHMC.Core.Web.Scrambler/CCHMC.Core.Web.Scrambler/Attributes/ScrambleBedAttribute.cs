using CCHMC.Core.Web.Scrambler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    public class ScrambleBedAttribute : ScrambleAttribute
    {
        /// <summary>
        /// Sets a number or string field or property to be obfuscated in the form of a ZIP code.
        /// </summary>
        /// <param name="strict">If true, it will create the number to match the type and format, if applicable; otherwise, it will generate a 5-digit ZIP code.</param>
        public ScrambleBedAttribute ()
        {

        }

        /// <summary>
        /// Creates or returns an obfuscation for the field or property.
        /// </summary>
        /// <param name="obj">The value to be obfuscated.</param>
        /// <returns>A randomly generated ZIP code.</returns>
        public override object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                var strObj = obj as string;
                if (String.IsNullOrEmpty(strObj))
                {
                    strObj = RandomHelper.NextChar().ToString() + RandomHelper.NextChar().ToString();
                }
                else if (strObj.Length == 1)
                {
                    strObj += RandomHelper.NextChar().ToString();
                }
                _obfuscate = String.Concat(strObj.Substring(0, 2),
                    RandomHelper.NextNumberOfLength(2),
                    RandomHelper.NextChar().ToString(),
                    RandomHelper.NextNumberOfLength(1));
                
            }
            return _obfuscate;
        }
    }
}
