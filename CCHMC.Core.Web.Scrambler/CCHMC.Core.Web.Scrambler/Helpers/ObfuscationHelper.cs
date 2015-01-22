using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Settings;
using Newtonsoft.Json;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// Helper used for default obfuscation and related functions.
    /// </summary>
    internal static class ObfuscationHelper
    {
        /// <summary>
        /// A list of types which the default obfuscation will recognize and obfuscate.
        /// </summary>
        private static List<Type> _types = new List<Type>() {
                        typeof(byte),
                        typeof(sbyte),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(char),
                        typeof(string),
                        typeof(double),
                        typeof(float),
                        typeof(decimal),
                        typeof(DateTime),
                        typeof(bool)
                    };
       
        /// <summary>
        /// Finds the preferred obfuscation value for the object.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <param name="scram">The ScrambleAttribute associated with the field or property.</param>
        /// <param name="value">The value to be obfuscated.</param>
        /// <param name="obfuscated">Objects which have already been obfuscated and can be skipped if encountered again.</param>
        /// <returns>An appropriate obfuscation for the value.</returns>
        public static object GetObfuscation (Type type, ScrambleAttribute scram, object value, MemberInfo memInfo, Dictionary<object, object> obfuscated)
        {
            if (scram == null)
                scram = TryGetAttribute(type, value, memInfo);

            object obfuscate = null;
            if (scram != null)
            {
                obfuscate = scram.Obfuscate(value);
            } else if (value != null)
            {
                obfuscate = value.Obfuscate(obfuscated);
            }
            return obfuscate;
        }

        /// <summary>
        /// Obfuscates an object and all of its fields and properties.
        /// </summary>
        /// <param name="obj">The object to be obfuscated.</param>
        /// <returns>An obfuscated replacement for the object.</returns>
        public static object Obfuscate (this object obj)
        {
            return obj.Obfuscate(new Dictionary<object, object>());
        }

        internal static object Clone(this object obj)
        {
            if (obj == null)
                return null;

            return typeof(object).InvokeMember("MemberwiseClone",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                null,
                obj,
                null);
        }

        //Todo: Extract duplicated code to a separate method as much as possible.
        /// <summary>
        /// Obfuscates an object and all of its fields and properties.
        /// </summary>
        /// <param name="obj">The object to be obfuscated.</param>
        /// <param name="obfuscated">Objects which have already been obfuscated and can be skipped if encountered again.</param>
        /// <returns>An obfuscated replacement for the object.</returns>
        private static object Obfuscate (this object obj, Dictionary<object, object> obfuscated)
        {
            if (obfuscated == null)
                obfuscated = new Dictionary<object, object>();

            if (!ObfuscationSettings.IsActive || obj == null)
                return obj;
            if (obfuscated.ContainsKey(obj))
                return obfuscated[obj];

            //Primitive types
            if (_types.Contains(obj.GetType()))
                return DefaultObfuscate(obj.GetType());

            object clone = Clone(obj);

            //Add to the list of already obfuscated objects in case of recursion.
            obfuscated.Add(obj, clone);

            var type = clone.GetType();
            ScrambleAttribute classScramble = Attribute.GetCustomAttribute(type, typeof(ScrambleAttribute)) as ScrambleAttribute 
                                              ?? ScrambleRegister.GetScrambleAttribute(type);

            if (classScramble == null || !classScramble.Ignore)
            {
                if (clone is IEnumerable) {
                    clone = IEnumerableHelper.ObfuscateIEnumerable(clone as IEnumerable, clone.GetType(), obfuscated);
                } else //Other objects
                {
                    var props = type.GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        ScrambleAttribute scramAttr = Attribute.GetCustomAttribute(prop, typeof(ScrambleAttribute)) as ScrambleAttribute
                                                      ?? ScrambleRegister.GetScrambleAttribute(type, prop.Name);

                        if (prop.CanWrite && (scramAttr == null || !scramAttr.Ignore))
                        {
                            var scram = scramAttr ?? classScramble;
                            object val = prop.GetValue(clone);
                            object obfuscate = GetObfuscation(prop.PropertyType, scram, val, prop, obfuscated);

                            if (obfuscate != null)
                                prop.SetValue(clone, ConversionHelper.ConvertType(obfuscate, prop.PropertyType));
                        }
                    }

                    foreach (FieldInfo field in type.GetFields())
                    {
                        ScrambleAttribute scramAttr = Attribute.GetCustomAttribute(field, typeof(ScrambleAttribute)) as ScrambleAttribute
                                                      ?? ScrambleRegister.GetScrambleAttribute(type, field.Name);
                        if (field.IsPublic && !field.Attributes.HasFlag(FieldAttributes.Literal) && (scramAttr == null || !scramAttr.Ignore))
                        {
                            var scram = scramAttr ?? classScramble;
                            var val = field.GetValue(clone);
                            object obfuscate = GetObfuscation(field.FieldType, scram, val, field, obfuscated);

                            if (obfuscate != null)
                                field.SetValue(clone, ConversionHelper.ConvertType(obfuscate, field.FieldType));
                        }
                    }
                }
            }
            return clone;
        }

        /// <summary>
        /// The default method to use when obfuscating if obfuscation is enabled and no alternative is present.
        /// </summary>
        /// <param name="obj">The object to be obfuscated.</param>
        /// <returns>And obfuscated replacement for the object.</returns>
        public static ScrambleAttribute TryGetAttribute(Type type, Object val, MemberInfo memInfo)
        {
            //Detect what the best fit is.
            if (val is char)
            {
                //Not sure how well this could be merged with the String check below, since the String check looks more at the object and may not give it the ScrambleTextAttribute.
                return new ScrambleTextAttribute(true);
            }
            else if (ConstantValues.WholeNumberTypes.Contains(type) || ConstantValues.NonwholeNumberTypes.Contains(type))
            {
                return new ScrambleNumberAttribute(true);
            }
            else if (val is string)
            {
                //Check if it is a number or datetime.
                //Todo: Is there a more efficient way of doing this?
                long o;
                if (long.TryParse((string)val, out o))
                {
                    return new ScrambleNumberAttribute(true);
                } 
                DateTime dt;
                if (DateTime.TryParse((string)val, out dt))
                {
                    return new ScrambleDateTimeAttribute(true);
                }

                if (memInfo != null && memInfo.Name.IndexOf("Name", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (memInfo.Name.IndexOf("first", StringComparison.InvariantCultureIgnoreCase)  >= 0
                        || memInfo.Name.IndexOf("fname", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return new ScrambleNameAttribute("{F}");
                    }
                    else if (memInfo.Name.IndexOf("last", StringComparison.InvariantCultureIgnoreCase) >= 0
                      || memInfo.Name.IndexOf("lname", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return new ScrambleNameAttribute("{L}");
                    }
                    else if (memInfo.Name.IndexOf("middle", StringComparison.InvariantCultureIgnoreCase) >= 0
                        || memInfo.Name.IndexOf("mname", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return new ScrambleNameAttribute("{M}");
                    }
                    else if (memInfo.Name.IndexOf("full", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        return new ScrambleNameAttribute("{L}, {F} {M}");
                    }
                    return new ScrambleNameAttribute(true);
                }
                return new ScrambleTextAttribute(true);
            }
            else if (val is DateTime)
            {
                return new ScrambleDateTimeAttribute(true);
            }
            else if (val is bool)
            {
                //A 1 in 3 chance of returning null if the bool is nullable.
                if (type == typeof(bool?) && RandomHelper.Random.Next(3) == 0)
                {
                    return new ScrambleAttribute(null);
                }
                return new ScrambleAttribute(RandomHelper.Random.Next(2) == 1);
            }
            return null;
        }

        /// <summary>
        /// The default method to use when obfuscating if obfuscation is enabled and no alternative is present.
        /// This should only be used for primitive types with no additional information in the form of a MemberInfo or a given ScrambleAttribute.
        /// </summary>
        /// <param name="obj">The object to be obfuscated.</param>
        /// <returns>And obfuscated replacement for the object.</returns>
        internal static object DefaultObfuscate(Type type)
        {
            //Detect what the best fit is.
            //Use reflection to get the type of the property/field
            if (type.IsAssignableFrom(typeof(byte)) || type.IsAssignableFrom(typeof(sbyte)) || type.IsAssignableFrom(typeof(short))
              || type.IsAssignableFrom(typeof(ushort)) || type.IsAssignableFrom(typeof(int)) || type.IsAssignableFrom(typeof(uint))
              || type.IsAssignableFrom(typeof(long)) || type.IsAssignableFrom(typeof(ulong)))
            {
                return ConversionHelper.TryConvertTo(RandomHelper.LongRandom(0, (long)Math.Min(ConstantValues.NumberMaxValues[type], long.MaxValue)), type);
            }
            else if (type.IsAssignableFrom(typeof(double)) || type.IsAssignableFrom(typeof(decimal)) || type.IsAssignableFrom(typeof(float)))
            {
                return ConversionHelper.TryConvertTo(RandomHelper.Random.NextDouble(), type);
            }
            else if (type.IsAssignableFrom(typeof(string)))
            {
                return "REDACTED";
            }
            else if (type.IsAssignableFrom(typeof(char[]))) 
            {
                return "REDACTED".ToCharArray();
            }
            else if (type.IsAssignableFrom(typeof(DateTime)))
            {
                return DateTime.Now;
            }
            else if (type.IsAssignableFrom(typeof(char)))
            {
                return RandomHelper.NextChar();
            }
            else if (type.IsAssignableFrom(typeof(bool)))
            {
                if (type.IsAssignableFrom(typeof(bool?)) && RandomHelper.Random.Next(3) == 0)
                {
                    return null;
                }
                return RandomHelper.Random.Next(2) == 0;
            }
            return new Object();
        }
    }
}
