using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Models;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// Attribute for strings containing names.
    /// </summary>
    public class ScrambleNameAttribute : ScrambleAttribute
    {
        /// <summary>
        /// The Gender options for generated names.
        /// </summary>
        public enum Gender { Female, Male, Random };

        public enum NamePart 
        { 
            FirstName, 
            FirstInitial, 
            MiddleName, 
            MiddleInitial, 
            LastName, 
            LastInitial 
        }

        /// <summary>
        /// Create a randomized name to obfuscate a Name property.
        /// </summary>
        public ScrambleNameAttribute (bool strict = false)
        {
            IsStrict = strict;
            if (!strict)
            {
                _obfuscate = NameHelper.GenerateName(Gender.Random).FullName;
            }
        }
        
        /// <summary>
        /// Create a randomized name to obfuscate a Name property.
        /// </summary>
        /// <param name="format">
        /// The format to use for the name, with {F} for first name, {M} for middle name, and {L} for last name. 
        /// {FI}, {MI}, and {LI} can be used for first name initial, middle initial, and last name initial respectively.
        /// </param>
        public ScrambleNameAttribute (string format)
        {
            IsStrict = false;
            _obfuscate = NameHelper.Format(format, NameHelper.GenerateName(Gender.Random));
        }

        /// <summary>
        /// Create a list of randomized names to obfuscate a Name property.
        /// </summary>
        /// <param name="count">The number of names to generate.</param>
        /// <param name="separator">The string to use to separate the names from each other. Defaults to "; ".</param>
        public ScrambleNameAttribute (int count, string separator="; ")
            : this(count, "{L}, {F}", separator)
        {

        }
        
        /// <summary>
        /// Create a list of randomized names to obfuscate a Name property.
        /// </summary>
        /// <param name="count">The number of names to generate.</param>
        /// <param name="format">
        /// The format to use for the name, with {F} for first name, {M} for middle name, and {L} for last name. 
        /// {FI}, {MI}, and {LI} can be used for first name initial, middle initial, and last name initial respectively.
        /// </param>
        /// <param name="separator">The string to use to separate the names from each other. Defaults to "; ".</param>
        public ScrambleNameAttribute (int count, string format, string separator="; ")
        {
            List<Name> names = new List<Name>();
            for (int i=0; i < count; i++)
            {
                names.Add(NameHelper.GenerateName(Gender.Random));
            }
            _obfuscate = String.Join(separator, names.Select(t => NameHelper.Format(format, t)));
        }
        
        /// <summary>
        /// Create a randomized name to obfuscate a Name property.
        /// </summary>
        /// <param name="format">
        /// The format to use for the name, with {F} for first name, {M} for middle name, and {L} for last name. 
        /// {FI}, {MI}, and {LI} can be used for first name initial, middle initial, and last name initial respectively.
        /// </param>
        /// <param name="gender">
        /// The gender for the generated obfuscated name.
        /// </param>
        public ScrambleNameAttribute (string format, Gender gender)
        {
            IsStrict = false;
            _obfuscate = NameHelper.Format(format, NameHelper.GenerateName(gender));
        }

        /// <summary>
        /// Creates or returns the formatted name for the field or property.
        /// </summary>
        /// <param name="obj">The value being obfuscated.</param>
        /// <returns>A string contraining a formatted name.</returns>
        public override object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                if (IsStrict && obj is string)
                {
                    string strObj = obj as string;
                    var splitObj = Regex.Split(strObj, " ");
                    if (splitObj.Count() <= 3){
                        _obfuscate = NameHelper.Format(DetectionHelper.DetectNameFormat(obj as string), NameHelper.GenerateName(Gender.Random));
                    } else
                    {
                        //If there are more than three spaces or there are separators (e.g. -;/&| ), assume it's a list of names.
                        string format = "{L}, {F}";

                        List<Name> names = new List<Name>();
                        for (int i=0; i < splitObj.Count() / 2; i++)
                        {
                            names.Add(NameHelper.GenerateName(Gender.Random));
                        }
                        _obfuscate = String.Join("; ", names.Select(t => NameHelper.Format(format, t)));
                    }
                } else if (obj.GetType().IsAssignableFrom(typeof(List<string>)))
                {
                    var lObj = obj as IList<string>;
                    _obfuscate = lObj.Select(t => NameHelper.GenerateName(Gender.Random).FullName).ToList();
                } else if (obj.GetType().IsAssignableFrom(typeof(string[])))
                {
                    var aObj = obj as string[];
                    _obfuscate = aObj.Select(t => NameHelper.GenerateName(Gender.Random).FullName).ToArray();
                }
            }
            return _obfuscate;
        }
    }
}
