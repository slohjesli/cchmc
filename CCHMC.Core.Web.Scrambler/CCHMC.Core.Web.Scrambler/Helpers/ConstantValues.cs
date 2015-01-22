using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// Stores the constant values for the Scrambler attributes and helpers.
    /// </summary>
    internal static class ConstantValues
    {
        /// <summary>
        /// 365 Days in a year for 100 years
        /// </summary>
        public const int CenturyDays = (365 * 100);

        /// <summary>
        /// A list of types which are valid for whole numbers.
        /// </summary>
        public static readonly List<Type> WholeNumberTypes = new List<Type> {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long), 
            typeof(ulong),
            typeof(byte),
            typeof(sbyte),
            typeof(char)
        };

        public static readonly List<Type> NonwholeNumberTypes = new List<Type> {
            typeof(double), typeof(float), typeof(decimal)
        };

        /// <summary>
        /// Stores the maximum values for various number types.
        /// </summary>
        public static readonly Dictionary<Type, ulong> NumberMaxValues = new Dictionary<Type, ulong>()
        {
            {typeof(char), (ulong)char.MaxValue },
            {typeof(byte), (ulong)byte.MaxValue },
            {typeof(sbyte), (ulong)sbyte.MaxValue },
            { typeof(short), (ulong)short.MaxValue },
            { typeof(ushort), (ulong)ushort.MaxValue },
            { typeof(int), (ulong)int.MaxValue },
            { typeof(uint), (ulong)uint.MaxValue },
            { typeof(long), (ulong)long.MaxValue },
            { typeof(ulong), (ulong)ulong.MaxValue },
            { typeof(string), (ulong)ulong.MaxValue }
        };

        /// <summary>
        /// Stores critical values for minutes and seconds for detection during strict Time and DateTime analysis.
        /// </summary>
        public static readonly List<int> MinSecVals = new List<int> { 5, 10, 15, 30 };
        
        /// <summary>
        /// Stores critical values for hours for detection during strict Time and DateTime analysis.
        /// </summary>
        public static readonly List<int> HourVals = new List<int> { 3, 6, 12 };

        /// <summary>
        /// Stores critical values for days for detection during strict Date and DateTime analysis.
        /// </summary>
        public static readonly List<int> DayVals = new List<int> { 5, 10, 15 };
        
        /// <summary>
        /// Stores critical values for hours for detection during strict Time and DateTime analysis.
        /// </summary>
        public static readonly List<int> MonthVals = new List<int> { 3, 6 };

        /// <summary>
        /// Stores critical values for years within a century for detection during strict Date and DateTime analysis.
        /// </summary>
        public static readonly List<int> YearVals = new List<int> { 5, 10, 25, 50, 100 };

        /// <summary>
        /// The minimum date for generating DateTimes.
        /// </summary>
        public static readonly DateTime MinDateTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// The ticks in the minimum DateTime.
        /// </summary>
        public static readonly long MinDateTimeTicks = MinDateTime.Ticks;

        /// <summary>
        /// The maximum date for generating DateTimes.
        /// </summary>
        public static readonly DateTime MaxDateTime = new DateTime(2100, 12, 31);
        /// <summary>
        /// The ticks in the maximum DateTime.
        /// </summary>
        public static readonly long MaxDateTimeTicks = MaxDateTime.Ticks;
    }
}
