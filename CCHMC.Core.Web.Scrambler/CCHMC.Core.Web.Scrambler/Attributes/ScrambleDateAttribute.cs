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
    /// Attribute for DateTimes or Strings which have only date values.
    /// </summary>
    public class ScrambleDateAttribute : ScrambleAttribute
    {
        /// <summary>
        /// Sets a field or property to use a Date as its obfuscation.
        /// </summary>
        /// <param name="strict">If true, the obfuscation will be based on the value of the field or property being obfuscated.</param>
        public ScrambleDateAttribute (bool strict = false)
        {
            IsStrict = strict;

            if (!strict)
            {
                _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.Date);
            }
        }

        /// <summary>
        /// Sets a field or property to be obfuscated with a Date which is a multiple of the given TimeSpan.
        /// </summary>
        /// <param name="step">The step size to use when generating the obfuscation.</param>
        public ScrambleDateAttribute (TimeSpan step)
        {
            //Adjust step size to be a number of days.
            step = new TimeSpan((step.Ticks / TimeSpan.TicksPerDay) * TimeSpan.TicksPerDay);

            if (step == TimeSpan.Zero)
            {
                _obfuscate = DateTime.Today;
            }
            else
            {
                _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.Date, step);
            }
        }

        /// <summary>
        /// Creates or returns the obfuscation for the decorated object.
        /// </summary>
        /// <param name="obj">The value being obfuscated.</param>
        /// <returns>The appropriate obfuscation for the object.</returns>
        public override object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                if (IsStrict)
                {
                    _obfuscate = DateTimeHelper.StrictObfuscation(obj, DateTimeMask.Date);
                } else
                {
                    _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.Date);
                }
            }
            return _obfuscate;
        }
    }
}
