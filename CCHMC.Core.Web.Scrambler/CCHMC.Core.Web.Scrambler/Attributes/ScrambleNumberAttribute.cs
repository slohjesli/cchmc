using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// Attribute for numerical values.
    /// </summary>
    public class ScrambleNumberAttribute : ScrambleAttribute
    {
        private int Prefix;

        /// <summary>
        /// Creates a randomized number to obfuscate the decorated field or property.
        /// </summary>
        /// <param name="strict">If true, the generated value will be the same number of digits.</param>
        public ScrambleNumberAttribute (bool strict=false)
        {
            IsStrict = strict;
            if (!strict)
            {
                _obfuscate = RandomHelper.Random.Next();
            }
        }

        /// <summary>
        /// Creates a randomized number beginning with the given prefix. 
        /// The generated number will follow strict settings, so it will match the value length.
        /// </summary>
        /// <param name="prefix">The value to start the obfuscation.</param>
        public ScrambleNumberAttribute(int prefix)
        {
            IsStrict = true;
            Prefix = prefix;
        }

        /// <summary>
        /// Attribute to obfuscate numerical values within a range.
        /// </summary>
        /// <param name="min">The minimum value which could be generated.</param>
        /// <param name="max">The maximum value which could be generated.</param>
        public ScrambleNumberAttribute (int min, int max)
        {
            if (min > max)
            {
                //Flip the values.
                var tmp = max;
                max = min;
                min = tmp;
            }
            _obfuscate = RandomHelper.Random.Next(max - min) + min;
        }

        private object ObfuscateWholeNumber(object obj)
        {
            int pow;
            if (obj is string)
            {
                pow = ((string)obj).Length - 1;
            } else
            {
                //It's a number value, convert to string to get the length
                pow = obj.ToString().Length - 1;//(int)Math.Floor(Math.Log10((long)obj));
            }

            if (Prefix != 0)
            {
                pow -= (int)Math.Ceiling(Math.Log10(Math.Abs(Prefix)));
                if (pow < 0)
                    pow = -1;
            }

            long min, max;
            min = (long)Math.Pow(10, pow);
            if (long.MaxValue / 10 >= min)
                max = (long)Math.Pow(10, pow + 1);
            else
                max = long.MaxValue;
            var obf = (long)Prefix * Math.Pow(10, pow + 1) + RandomHelper.LongRandom(min, max);
            var minobf = Math.Min(obf, (ulong)ConstantValues.NumberMaxValues[obj.GetType()]).ToString();
            return ConversionHelper.TryConvertTo(minobf, obj.GetType());
        }

        private object ObfuscateDecimalNumber(object obj)
        {
            int pow;
            //Determine the number of sig figs before and after the decimal.
            var spl = obj.ToString().Split(".".ToCharArray());
            pow = spl[0].Length;

            if (Prefix != 0)
                pow -= (int)Math.Ceiling(Math.Log10(Math.Abs(Prefix)));

            double beg = (double)RandomHelper.Random.Next((int)Math.Pow(10, pow - 1), (int)Math.Pow(10, pow)) + Prefix * Math.Pow(10, pow);
            if (spl.Length > 1)
            {
                pow = spl[1].Length;
                int sigfigs = (int)Math.Pow(10, pow);
                var end = Math.Truncate(RandomHelper.Random.NextDouble() * sigfigs) / sigfigs;
                //Make sure there are enough sigfigs in the decimal.
                if (Math.Truncate(end * sigfigs/10)/(sigfigs/10) == end)
                    end += Math.Pow(.1, pow);
                var ret = beg + end;
                //Todo: There must be a better way of making sure the conversion and addition doesn't cause rounding errors which mess up the prefix
                //Make sure the prefix isn't messed up by rounding.
                if (Prefix != 0 && ret >= Prefix + 1)
                    ret -= .1;
                return ConversionHelper.TryConvertTo(ret, obj.GetType());
            }
            return ConversionHelper.TryConvertTo(beg, obj.GetType());
        }
        /// <summary>
        /// Creates or returns the appropriate obfuscation for the field or property.
        /// </summary>
        /// <param name="obj">The value to be obfuscated.</param>
        /// <returns>A randomized number to replace the value.</returns>
        public override object Obfuscate (object obj)
        {
            if (obj == null)
                return null;
            if (_obfuscate == null)
            {
                if (IsStrict)
                {
                    if (ConstantValues.WholeNumberTypes.Contains(obj.GetType()) || (obj is string && !((string)obj).Contains('.')))
                    {
                        _obfuscate = ObfuscateWholeNumber(obj);
                    } 
                    else if (ConstantValues.NonwholeNumberTypes.Contains(obj.GetType()) || (obj is string)) 
                    {
                        _obfuscate = ObfuscateDecimalNumber(obj);
                    } 
                    else
                    {
                        //We shouldn't reach here
                        throw new InvalidCastException(String.Format("Could not cast type {0} as a number!", obj.GetType()));
                    }
                } 
                else
                {
                    _obfuscate = RandomHelper.Random.Next();
                }
            }

            return _obfuscate;
        }
    }
}
