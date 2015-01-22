using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Models;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// Attribute for DateTimes or for strings containing full DateTime values.
    /// </summary>
    public class ScrambleDateTimeAttribute : ScrambleAttribute
    {
        /// <summary>
        /// Sets a field or property to use a DateTime as its obfuscation.
        /// </summary>
        /// <param name="strict">If true, the obfuscation will be based on the value of the field or property being obfuscated.</param>
        public ScrambleDateTimeAttribute (bool strict = false)
        {
            IsStrict = strict;

            if (!strict)
            {
                _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.DateTime);
            }
        }
        
        /// <summary>
        /// Sets a field or property to be obfuscated with a DateTime which is a multiple of the given TimeSpan.
        /// </summary>
        /// <param name="step">The step size to use when generating the obfuscation.</param>
        public ScrambleDateTimeAttribute (TimeSpan stepsize)
        {
            if (stepsize == TimeSpan.Zero)
            {
                _obfuscate = DateTime.Now;
            }
            else
            {
                _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.DateTime, stepsize);
            }
        }

        /// <summary>
        /// Creates or returns a DateTime obfuscation for the field or property.
        /// </summary>
        /// <param name="obj">The value being obfuscated.</param>
        /// <returns>A DateTime value to obfuscate the field or property.</returns>
        public override object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                if (IsStrict)
                {
                    _obfuscate = DateTimeHelper.StrictObfuscation(obj, DateTimeMask.DateTime);
                } else
                {
                    _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.DateTime);
                }
            }
            return _obfuscate;
        }
    }
}
