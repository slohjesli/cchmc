using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// A Helper for generating Random values.
    /// </summary>
    internal static class RandomHelper
    {
        /// <summary>
        /// The Random to be used internally to prevent reseeding causing duplicated values.
        /// </summary>
        public static readonly Random Random = new Random();

        /// <summary>
        /// The characters in the uppercase alphabet, used when generating random characters.
        /// </summary>
        private static readonly string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Generates a random number using the given stepsize.
        /// </summary>
        /// <param name="max">The maximum value for the generated number.</param>
        /// <param name="step">The step size.</param>
        /// <returns>A number less than or equal to max which is divisible by step.</returns>
        public static int NextByStep(int max, int step)
        {
            return NextByStep(0, max, step);
        }
        /// <summary>
        /// Generates a random number using the given stepsize.
        /// </summary>
        /// <param name="min">The minimum value for the generated number.</param>
        /// <param name="max">The maximum value for the generated number.</param>
        /// <param name="step">The step size.</param>
        /// <returns>A number between min and max which is divisible by step.</returns>
        public static int NextByStep(int min, int max, int step)
        {
            if (min > max)
            {
                var tmp = max;
                max = min;
                min = tmp;
            }
            else if (min == max || step == 0)
                return min;
            return Random.Next((min/step) + 1, (max / step)) * step;
        }

        /// <summary>
        /// Generates a random uppercase alpha character.
        /// </summary>
        /// <returns></returns>
        public static char NextChar()
        {
            return _characters[Random.Next(26)];
        }

        /// <summary>
        /// Generates a string containing a number of the given length, with leading zeroes.
        /// </summary>
        /// <param name="size">The length of the string to generate</param>
        /// <returns>A string of all numbers of the specified length.</returns>
        public static string NextNumberOfLength(int size)
        {
            if (size <= 0)
                return string.Empty;
            return Random.Next((int)Math.Pow(10, size)).ToString().PadLeft(size, '0');
        }

        /// <summary>
        /// Generates a random number which will always be small enough to be contained in the given type.
        /// </summary>
        /// <param name="min">The minimum value for the generated number.</param>
        /// <param name="max">The maximum value for the generated number.</param>
        /// <param name="type">The target type in which the number needs to fit.</param>
        /// <returns>A random number between min and max which is less than the maximum value for the number type, or the max value if min and max are both larger than the type's maximum.</returns>
        public static long TypeSafeNext(long min, long max, Type type)
        {
            return RandomHelper.LongRandom((long)Math.Min(min, (long)Math.Min(long.MaxValue, ConstantValues.NumberMaxValues[type])), (long)Math.Min(max, (long)Math.Min(long.MaxValue, ConstantValues.NumberMaxValues[type])));
        }

        /// <summary>
        /// Generates a random value which can extend into long values.
        /// </summary>
        /// <param name="min">The minimum value which could be returned.</param>
        /// <param name="max">The maximum value which could be returned.</param>
        /// <returns>A value between min and max.</returns>
        public static long LongRandom(long min, long max) 
        {            
            if (min == max)
                return min;

            byte[] buf = new byte[8];
            Random.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
