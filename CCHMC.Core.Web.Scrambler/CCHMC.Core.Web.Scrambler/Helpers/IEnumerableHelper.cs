using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    internal static class IEnumerableHelper
    {

        /// <summary>
        /// Gets the type of object stored in the IEnumerable type it is passed.
        /// </summary>
        /// <param name="type">The type of the IEnumerable to check.</param>
        /// <returns>A Type array containing the generic arguments of the given Type, or the IEnumerable type if no generic type can be found.</returns>
        internal static Type[] GetGenericIEnumerableType(Type type)
        {
            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments();

            foreach (var i in type.GetInterfaces())
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return i.GetGenericArguments();

            //Default value
            return new Type[] { typeof(IEnumerable) };
        }

        /// <summary>
        /// Generates an obfuscation for an IEnumerable.
        /// </summary>
        /// <param name="obj">The IEnumerable to obfuscate.</param>
        /// <param name="type">The full type of the IEnumerable.</param>
        /// <param name="obfuscated">A list of objects which have already been obfuscated.</param>
        /// <returns>An obfuscated IEnumerable of an acceptable type.</returns>
        internal static IEnumerable ObfuscateIEnumerable(IEnumerable obj, Type type, Dictionary<object, object> obfuscated)
        {
            if (obj == null)
                return null;
            
            if (obfuscated == null)
                obfuscated = new Dictionary<object, object>();

            List<Type> interfaces = new List<Type>(type.GetInterfaces());
            if (type.IsInterface)
                interfaces.Add(type);
            interfaces = interfaces.Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t).ToList();
            if (interfaces.Contains(typeof(IDictionary)))
            {
                var dObj = obj as IDictionary;
                IDictionary ret = (IDictionary)Activator.CreateInstance(type);
                foreach (object o in dObj.Keys)
                {
                    ret[o] = ObfuscationHelper.GetObfuscation(type, null, dObj[o], null, obfuscated);
                }
                return ret;
            }
            ///Most of the rest of these should be able to be collapsed for the most part into a single section, with some change at the end.
            else if (interfaces.Contains(typeof(IList)) || interfaces.Contains(typeof(IList<>)))
            {
                //Todo: Cleanup
                List<object> lObj = new List<object>();
                foreach (var el in obj as IList)
                {
                    lObj.Add(el);
                }
                lObj = lObj.ToList().Select(t => ObfuscationHelper.GetObfuscation(type, null, t, null, obfuscated)).ToList();

                var toTypes = GetGenericIEnumerableType(type);
                var target = typeof(Enumerable)
                    .GetMethod("Cast", new[] { typeof(List<object>) })
                    .MakeGenericMethod(toTypes)
                    .Invoke(null, new object[] { lObj });
                var result = typeof(Enumerable)
                    .GetMethod("ToList")
                    .MakeGenericMethod(toTypes)
                    .Invoke(null, new object[] { target });

                if (obj is Array)
                {
                    return typeof(Enumerable)
                    .GetMethod("ToArray")
                    .MakeGenericMethod(toTypes)
                    .Invoke(null, new object[] { result }) as IList;
                }

                return result as IList;
            }
            else if (interfaces.Contains(typeof(ICollection)) || interfaces.Contains(typeof(ICollection<>)))
            {
                var cObj = obj as ICollection;
                List<object> ret = new List<object>();
                foreach (var i in cObj)
                {
                    ret.Add(ObfuscationHelper.GetObfuscation(type, null, i, null, obfuscated));
                }

                var toTypes = GetGenericIEnumerableType(type);
                var target = typeof(Enumerable)
                    .GetMethod("Cast", new[] { typeof(List<object>) })
                    .MakeGenericMethod(toTypes)
                    .Invoke(null, new object[] { ret.AsEnumerable() });
                var result = typeof(Enumerable)
                    .GetMethod("ToList")
                    .MakeGenericMethod(toTypes)
                    .Invoke(null, new object[] { target });
                return result as ICollection;
            }
            else if (interfaces.Contains(typeof(IEnumerable)))
            {
                var cObj = obj as IEnumerable;
                List<object> ret = new List<object>();
                foreach (var i in cObj)
                {
                    ret.Add(ObfuscationHelper.GetObfuscation(type, null, i, null, obfuscated));
                }

                var toTypes = GetGenericIEnumerableType(type);
                var result = typeof(Enumerable)
                    .GetMethod("Cast", new[] { typeof(IEnumerable) })
                    .MakeGenericMethod(toTypes)
                    .Invoke(null, new object[] { ret.AsEnumerable() });
                if (obj is IQueryable)
                {
                    return (result as IEnumerable).AsQueryable();
                }
                return result as IEnumerable;
            }

            return null;
        }
    }
}
