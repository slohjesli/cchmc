using CCHMC.Core.Web.Scrambler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    internal static class DateTimeHelper
    {
        /// <summary>
        /// Creates the DateTime object with any fields unflagged in the mask set to their minimum values.
        /// </summary>
        /// <param name="dt">The DateTime to which the mask will be applied</param>
        /// <param name="mask">The mask indicating the fields to be retained.</param>
        /// <returns>A DateTime with only the appropriate fields</returns>
        internal static DateTime ApplyMask(DateTime dt, DateTimeMask mask)
        {
            mask = mask ?? DateTimeMask.DateTime;
            return new DateTime(mask.Year ? dt.Year : DateTime.MinValue.Year,
                                 mask.Month ? dt.Month : DateTime.MinValue.Month,
                                 mask.Day ? dt.Day : DateTime.MinValue.Day,
                                 mask.Hour ? dt.Hour : 0,
                                 mask.Minute ? dt.Minute : 0,
                                 mask.Seconds ? dt.Second : 0);
        }

        //Todo: Should this be in the DateTimeHelper or the RandomHelper?
        /// <summary>
        /// Creates a random DateTime in the range defined by ConstantValues.
        /// </summary>
        /// <returns>A randomly generated DateTime.</returns>
        private static DateTime GenerateDateTime()
        {
            return new DateTime(RandomHelper.LongRandom(ConstantValues.MinDateTimeTicks, ConstantValues.MaxDateTimeTicks));
        }

        /// <summary>
        /// Creates a random DateTime in the range defined in ConstantValues which is also a multiple of the given step size, if possible.
        /// </summary>
        /// <param name="step">The step size to use generating the DateTime.</param>
        /// <returns>A random DateTime which is a multiple of the given step size.</returns>
        private static DateTime GenerateDateTime(TimeSpan step)
        {
            if (step == TimeSpan.Zero)
                return new DateTime(ConstantValues.MinDateTimeTicks);
            return new DateTime(RandomHelper.LongRandom(ConstantValues.MinDateTimeTicks / step.Ticks, ConstantValues.MaxDateTimeTicks / step.Ticks) * step.Ticks);
        }

        /// <summary>
        /// Creates a random DateTime in the range defined in ConstantValues
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        internal static DateTime GenerateDateTime (DateTimeMask mask)
        {
            return ApplyMask(GenerateDateTime(), mask);
        }

        internal static DateTime GenerateDateTime(DateTimeMask mask, TimeSpan step)
        {
            if (step == TimeSpan.Zero)
                return ApplyMask(new DateTime(ConstantValues.MinDateTimeTicks), mask);
            return new DateTime(RandomHelper.LongRandom(ApplyMask(ConstantValues.MinDateTime, mask).Ticks / step.Ticks, 
                                                        ApplyMask(ConstantValues.MaxDateTime, mask).Ticks / step.Ticks)
                                * step.Ticks);
        }

        /// <summary>
        /// Helper function for the ObfuscateFromDateTime to find critical values for years, hours, minutes, and seconds.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private static int GetLastDivisible(this List<int> list, int val)
        {
            int tmp = list.LastOrDefault(t => val % t == 0);
            if (tmp == 0)
            {
                if (val == 0)
                    tmp = 1;
                else
                    tmp = 0;
            }
            return tmp;
        }

        /// <summary>
        /// Creates an obfuscation using strict settings for a DateTime.
        /// </summary>
        /// <param name="obj">The DateTime used to determine the strict options.</param>
        /// <returns></returns>
        private static DateTime ObfuscateFromDateTime(DateTime obj)
        {
            //Years: 5, 10, 25, 50 
            //Months and Hours: 3, 6, 12
            //Days: 5, 10, 15
            //Minutes and Seconds: 5, 15, 30
            int Year = ConstantValues.YearVals.GetLastDivisible(obj.Year);
            int Month = ConstantValues.MonthVals.LastOrDefault(t => obj.Month % t == 0);
            if (Month == 0)
                Month = 1;
            int Day = ConstantValues.DayVals.LastOrDefault(t => obj.Day % t == 0);
            if (Day == 0)
                Day = 1;
            int Hour = ConstantValues.HourVals.GetLastDivisible(obj.Hour);
            int Minute = ConstantValues.MinSecVals.GetLastDivisible(obj.Minute);
            int Second = ConstantValues.MinSecVals.GetLastDivisible(obj.Second);
            
            //Determine year and month ahead of time so the day can be restricted based on the days in that month.
            Year = ConstantValues.MinDateTime.Year + RandomHelper.NextByStep(100, Year);
            Month = RandomHelper.NextByStep(12, Month);

            return new DateTime(Year, 
                                Month, 
                                RandomHelper.NextByStep(DateTime.DaysInMonth(Year, Month), Day),
                                RandomHelper.NextByStep(24, Hour),
                                RandomHelper.NextByStep(60, Minute),
                                RandomHelper.NextByStep(60, Second));
        }

        /// <summary>
        /// Performs a strict obfuscation for the Date, Time, and DateTime attributes.
        /// </summary>
        /// <param name="obj">The object to obfuscate</param>
        /// <param name="mask"></param>
        /// <returns></returns>
        internal static DateTime StrictObfuscation(object obj, DateTimeMask mask)
        {
            DateTime _obfuscate;
            DateTime? dtObj = null;
            //Check for any zeros
            if (obj is string)
            {
                DateTime time;
                if (DateTime.TryParse((string)obj, out time))
                    dtObj = time;
            }
            else
            {
                dtObj = obj as DateTime?;
            }

            //Generate the result as appropriate.
            if (dtObj == null) 
            {
                _obfuscate = GenerateDateTime();
            } 
            else
            {
                _obfuscate = ObfuscateFromDateTime((DateTime)dtObj);
            }

            return ApplyMask(_obfuscate, mask);
        }
    }
}
