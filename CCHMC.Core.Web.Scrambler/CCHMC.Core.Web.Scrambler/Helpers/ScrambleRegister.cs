using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CCHMC.Core.Web.Scrambler.Attributes;

using System.ComponentModel;
using System.Linq.Expressions;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// The Helper class for registering types and members with Scramble attributes, for objects which are sealed or automatically generated.
    /// </summary>
    public static class ScrambleRegister
    {
        /// <summary>
        /// Stores the associated ScrambleAttributes for types which have been registered in the AttributeRegister.
        /// Registration applies across all user sessions.
        /// </summary>
        internal static Dictionary<Type, ScrambleAttribute> RegisteredTypes = new Dictionary<Type, ScrambleAttribute>();
        /// <summary>
        /// Stores the associated ScrambleAttributes for members of types which have been registered in the AttributeRegister.
        /// Registration applies across all user sessions.
        /// </summary>
        internal static Dictionary<Type, Dictionary<string, ScrambleAttribute>> RegisteredMembers = new Dictionary<Type, Dictionary<string, ScrambleAttribute>>();

        /// <summary>
        /// Loads scrambler attributes for types which have been registered using the AttributeRegister.
        /// </summary>
        /// <param name="type">The Type to check for Scramble attributes</param>
        /// <returns>The ScrambleAttribute associated with the type, or null if none have been registered.</returns>
        internal static ScrambleAttribute GetScrambleAttribute(Type type)
        {
            if (RegisteredTypes.ContainsKey(type))
            {
                return RegisteredTypes[type];
            }
            return null;
        }

        /// <summary>
        /// Loads scrambler attributes for types which have been registered using the AttributeRegister.
        /// </summary>
        /// <param name="type">The Type to which the member belongs.</param>
        /// <param name="membername">The name of the member with which the Attribute is assigned.</param>
        /// <returns>The ScrambleAttribute associated with the member, or null if none have been registered.</returns>
        internal static ScrambleAttribute GetScrambleAttribute(Type type, string membername)
        {
            if (RegisteredMembers.ContainsKey(type) && RegisteredMembers[type].ContainsKey(membername))
            {
                return RegisteredMembers[type][membername];
            }
            return null;
        }

        /// <summary>
        /// Loads scrambler attributes for types which have been registered using the AttributeRegister.
        /// </summary>
        /// <param name="type">The Type to which the member belongs.</param>
        /// <param name="membername">The name of the member with which the Attribute is assigned.</param>
        /// <returns>The ScrambleAttribute associated with the member, or null if none have been registered.</returns>
        public static ScrambleAttribute GetScrambleAttribute<T>(Expression<Func<T, object>> selector)
        {
            Type type = typeof(T);
            string membername = selector.PropertyName<T>();
            if (RegisteredMembers.ContainsKey(type) && RegisteredMembers[type].ContainsKey(membername))
            {
                return RegisteredMembers[type][membername];
            }
            return null;
        }

        /// <summary>
        /// Registers a Type with the specified Scramble attribute.
        /// </summary>
        /// <param name="type">The Type owning the member to be registered.</param>
        /// <param name="attr">The attribute to be applied to the member.</param>
        private static void Register(Type type, ScrambleAttribute attr)
        {
            RegisteredTypes[type] = attr;
        }

        /// <summary>
        /// Registers a member of a Type with the specified Scramble attribute.
        /// </summary>
        /// <param name="type">The Type owning the member to be registered.</param>
        /// <param name="memberName">The name of the member to register.</param>
        /// <param name="attr">The attribute to be applied to the member.</param>
        private static void Register(Type type, string memberName, ScrambleAttribute attr)
        {
            if (!RegisteredMembers.ContainsKey(type))
            {
                RegisteredMembers[type] = new Dictionary<string, ScrambleAttribute>();
            }
            RegisteredMembers[type][memberName] = attr;
        }

        //from http://stackoverflow.com/a/10902173/1211951
        private static string PropertyName<T>(this Expression<Func<T, object>> propertyExpression)
        {
            MemberExpression mbody = propertyExpression.Body as MemberExpression;

            if (mbody == null)
            {
                //This will handle Nullable<T> properties.
                UnaryExpression ubody = propertyExpression.Body as UnaryExpression;

                if (ubody != null)
                {
                    mbody = ubody.Operand as MemberExpression;
                }

                if (mbody == null)
                {
                    throw new ArgumentException("Expression is not a MemberExpression", "propertyExpression");
                }
            }

            return mbody.Member.Name;
        }

        public static void Ignore<T>()
        {
            Register<T>(new ScrambleIgnoreAttribute());
        }
        public static void Register<T>(ScrambleAttribute scram)
        {
            ScrambleRegister.RegisteredTypes.Add(typeof(T), scram);
        }

        public static void Ignore<T>(Expression<Func<T, object>> selector)
        {
            Register<T>(selector, new ScrambleIgnoreAttribute());
        }
        public static void Register<T>(Expression<Func<T, object>> selector, ScrambleAttribute scram)
        {
            if (selector == null)
                throw new ArgumentNullException();
            var type = typeof(T);
            string memberName = selector.PropertyName<T>();
            Register(type, memberName, scram);
        }
    }

    public class ScrambleRegister<T>
    {
        Type type = typeof(T);

        public ScrambleRegister<T> Ignore()
        {
            ScrambleRegister.Register<T>(new ScrambleIgnoreAttribute());
            return this;
        }

        public ScrambleRegister<T> Register(ScrambleAttribute scram)
        {
            ScrambleRegister.Register<T>(scram);
            return this;
        }

        public ScrambleRegister<T> Ignore(Expression<Func<T, object>> selector)
        {
            ScrambleRegister.Register<T>(selector, new ScrambleIgnoreAttribute());
            return this;
        }

        public ScrambleRegister<T> Register(Expression<Func<T, object>> selector, ScrambleAttribute scram)
        {
            ScrambleRegister.Register<T>(selector, scram);
            return this;
        } 
    }
}
