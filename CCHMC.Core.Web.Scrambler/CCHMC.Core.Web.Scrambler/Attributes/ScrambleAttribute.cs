using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// The default scramble attribute. This class is the class to extend to create custom scramble attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class ScrambleAttribute : Attribute
    {
        /// <summary>
        /// Sets the attribute to make its obfuscation reflect the value being obfuscated if appropriate.
        /// </summary>
        public bool IsStrict = false;

        /// <summary>
        /// The value the obfuscater will apply to the field or property, or the fields and properties of the class.
        /// </summary>
        protected object _obfuscate { get; set; }

        /// <summary>
        /// Flag indicating that the class, property, or field should not be obfuscated.
        /// </summary>
        public bool Ignore = false;

        /// <summary>
        /// Returns the appropriate obfuscation for the object.
        /// </summary>
        /// <param name="obj">The value being obfuscated.</param>
        /// <returns>An obfuscated value; by default, the value passed to the initializer or null if none has been passed.</returns>
        public virtual object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            return _obfuscate;
        }

        /// <summary>
        /// Default constructor for the ScramblerAttribute. 
        /// </summary>
        public ScrambleAttribute ()
        {

        }

        /// <summary>
        /// Obfuscates the field or property, or the fields and properties of the class, to the specified object.
        /// </summary>
        /// <param name="obf">The object to use to obfuscate the field(s) and/or property(s)</param>
        public ScrambleAttribute (object obf)
        {
            _obfuscate = obf;
        }
    }
}
