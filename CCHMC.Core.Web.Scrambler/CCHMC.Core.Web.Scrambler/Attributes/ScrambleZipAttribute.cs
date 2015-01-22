using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// Attribute for numbers and string containing ZIP codes.
    /// </summary>
    public class ScrambleZipAttribute : ScrambleAttribute
    {
        /// <summary>
        /// Sets a number or string field or property to be obfuscated in the form of a ZIP code.
        /// </summary>
        /// <param name="strict">If true, it will create the number to match the type and format, if applicable; otherwise, it will generate a 5-digit ZIP code.</param>
        public ScrambleZipAttribute (bool strict = false)
        {
            //Todo: Is there a way to get a list of valid ZIP codes? If not, strict should probably be removed.
            IsStrict = strict;
            if (!strict)
            {
                _obfuscate = RandomHelper.Random.Next(10000, 100000);
            }
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
                if (obj is string)
                {
                    _obfuscate = RandomHelper.NextNumberOfLength(5);
                    //Check format
                    if (((string)obj).Length == 10)
                    {
                        _obfuscate += String.Concat("-", RandomHelper.NextNumberOfLength(4));
                    }
                    else if (((string)obj).Length == 9)
                    {
                        _obfuscate += RandomHelper.NextNumberOfLength(4);
                    }
                }
                else if (ConstantValues.WholeNumberTypes.Contains(obj.GetType()))
                {
                    //If the number type cannot hold the maximum value of 99999, then it must use the max value for that number type.
                    _obfuscate = ConversionHelper.TryConvertTo(RandomHelper.TypeSafeNext(10000, 100000, obj.GetType()), obj.GetType());
                }
            }
            return _obfuscate;
        }
    }
}
