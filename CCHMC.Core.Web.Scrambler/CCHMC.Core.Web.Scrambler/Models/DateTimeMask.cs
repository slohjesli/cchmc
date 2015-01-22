using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Models
{
    internal class DateTimeMask
    {
        /// <summary>
        /// The standard mask to mask a DateTime to just the date.
        /// </summary>
        public static DateTimeMask Date = new DateTimeMask(56);
        /// <summary>
        /// The standard mask to mask a DateTime to just the time.
        /// </summary>
        public static DateTimeMask Time = new DateTimeMask(7);
        /// <summary>
        /// The standard mask to leave all fields intact.
        /// </summary>
        public static DateTimeMask DateTime = new DateTimeMask(63);

        /// <summary>
        /// The value storing the information to determing what information is masked; the number is used as a six-digit binary value for Year-Month-Day-Hour-Minute-Second.
        /// </summary>
        private int Mask;

        /// <summary>
        /// The flag indicating whether or not the year should be shown.
        /// </summary>
        public bool Year
        {
            get
            {
                return (Mask & 32) == 32;
            }
        }
        /// <summary>
        /// The flag indicating whether or not the month should be shown.
        /// </summary>
        public bool Month
        {
            get
            {
                return (Mask & 16) == 16;
            }
        }
        /// <summary>
        /// The flag indicating whether or not the day should be shown.
        /// </summary>
        public bool Day
        {
            get
            {
                return (Mask & 8) == 8;
            }
        }
        /// <summary>
        /// The flag indicating whether or not the hour should be shown.
        /// </summary>
        public bool Hour
        {
            get
            {
                return (Mask & 4) == 4;
            }
        }
        /// <summary>
        /// The flag indicating whether or not the minute should be shown.
        /// </summary>
        public bool Minute
        {
            get
            {
                return (Mask & 2) == 2;
            }
        }
        /// <summary>
        /// The flag indicating whether or not the second should be shown.
        /// </summary>
        public bool Seconds
        {
            get
            {
                return (Mask & 1) == 1;//Could just use %2, but I left it like this for consistency with the other flags.
            }
        }

        /// <summary>
        /// The default constructor, which does not mask any values.
        /// </summary>
        public DateTimeMask()
            : this (true, true, true, true, true, true)
        {

        }

        /// <summary>
        /// Constructor for a mask which has the respective flags set.
        /// </summary>
        /// <param name="year">True if the year should be visible; false if the mask should conceal it.</param>
        /// <param name="month">True if the month should be visible; false if the mask should conceal it.</param>
        /// <param name="date">True if the day should be visible; false if the mask should conceal it.</param>
        /// <param name="hour">True if the hour should be visible; false if the mask should conceal it.</param>
        /// <param name="min">True if the minute should be visible; false if the mask should conceal it.</param>
        /// <param name="sec">True if the second should be visible; false if the mask should conceal it.</param>
        public DateTimeMask(bool year, bool month, bool date, bool hour, bool min, bool sec)
        {
            Mask = Convert.ToInt32(year) * 32 + Convert.ToInt32(month) * 16 + Convert.ToInt32(date) * 8
                + Convert.ToInt32(hour) * 4 + Convert.ToInt32(min) * 2 + Convert.ToInt32(sec);
        }

        /// <summary>
        /// Constructor for the DateTimeMask for internal use.
        /// </summary>
        /// <param name="mask">The mask to be used; an int treated as a six-digit boolean value, with the rightmost place being the second, then minute, then hour, then day, then month, then year flags.</param>
        internal DateTimeMask(int mask)
        {
            Mask = mask;
        }
    }
}
