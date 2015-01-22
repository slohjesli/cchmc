using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    internal static class ConversionHelper
    {

        /// <summary>
        /// Attempts to convert an object to the target type.
        /// </summary>
        /// <param name="input">The object to be converted.</param>
        /// <param name="type">The target type.</param>
        /// <returns>The object as the appropriate type, or null if the conversion fails.</returns>
        internal static Object TryConvertTo(object input, Type type)
        {
            object result = null;
            try
            {
                result = Convert.ChangeType(input, type);
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Converts the obfuscation to the appropriate type.
        /// </summary>
        /// <param name="obfuscate">The value to be converted.</param>
        /// <param name="type">The target type.</param>
        /// <returns>The value in obfuscate as the appropriate type.</returns>
        internal static object ConvertType(object obfuscate, Type type)
        {
            if (obfuscate != null && !type.IsAssignableFrom(obfuscate.GetType()))
            {
                var conv = TryConvertTo(obfuscate, type);
                if (conv != null)
                {
                    obfuscate = conv;
                }
                else
                {
                    //We shouldn't get here unless the obfuscation does not match the object type.
                    throw new InvalidCastException(String.Format("Could not cast obfuscation ({0}) to the appropriate type ({1})!", obfuscate.GetType(), type));
                }
            }
            return obfuscate;
        }
    }
}
