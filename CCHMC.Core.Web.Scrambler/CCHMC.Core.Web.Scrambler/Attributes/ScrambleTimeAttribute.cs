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
    /// Attribute for DateTimes or for strings containing Time values.
    /// </summary>
    public class ScrambleTimeAttribute : ScrambleAttribute
    {
        /// <summary>
        /// Sets a field or property to use a Time stored in a DateTime as its obfuscation.
        /// </summary>
        /// <param name="strict">If true, the obfuscation will be based on the value of the field or property being obfuscated.</param>
        public ScrambleTimeAttribute (bool strict = false)
        {
            IsStrict = strict;

            if (!strict)
            {
                _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.Time);
            }
        }
        
        /// <summary>
        /// Sets a field or property to be obfuscated with a Time which is a multiple of the given TimeSpan.
        /// </summary>
        /// <param name="step">The step size to use when generating the obfuscation.</param>
        public ScrambleTimeAttribute (TimeSpan step)
        {
            step = new TimeSpan(step.Ticks % TimeSpan.TicksPerDay);
            if (step == TimeSpan.Zero)
            {
                _obfuscate = DateTimeHelper.ApplyMask(DateTime.Now, DateTimeMask.Time);
            }
            else
            {
                _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.Time, step);
            }
        }
        
        /// <summary>
        /// Sets a field or property to be obfuscated with a DateTime which is a multiple of the given TimeSpan and within the given range.
        /// </summary>
        /// <param name="min">The minimum value for the generated Time.</param>
        /// <param name="max">The maximum value for the generated Time.</param>
        /// <param name="step">The step size to use when generating the obfuscation.</param>
        public ScrambleTimeAttribute (DateTime min, DateTime max, TimeSpan step)
        {
            _obfuscate = new DateTime(min.Ticks + RandomHelper.LongRandom(0, (min.Ticks - max.Ticks) / step.Ticks) * step.Ticks);
        }

        /// <summary>
        /// Creates or returns the appropriate obfuscation for the property.
        /// </summary>
        /// <param name="obj">The value being obfuscated.</param>
        /// <returns>A randomized Time to obfuscate the field or property.</returns>
        public override object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                if (IsStrict)
                {
                    //Todo: Can the standard masks be stored as actual DateTimeMasks, so new copies don't need to be instantiated?
                    _obfuscate = DateTimeHelper.StrictObfuscation(obj, DateTimeMask.Time);
                } else
                {
                    _obfuscate = DateTimeHelper.GenerateDateTime(DateTimeMask.Time);
                }
            }
            return _obfuscate;

        }
    }
}
