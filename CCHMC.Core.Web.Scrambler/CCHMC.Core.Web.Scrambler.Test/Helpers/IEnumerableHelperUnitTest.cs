using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class IEnumerableHelperUnitTest
    {
        [TestMethod]
        public void GetGenericIEnumerableTypeSingleInterfaceValidType()
        {
            CollectionAssert.AreEquivalent(typeof(IList<string>).GetGenericArguments(), IEnumerableHelper.GetGenericIEnumerableType(typeof(List<string>)));
        }

        [TestMethod]
        public void GetGenericIEnumerableTypeMultipleInterfacesValidType()
        {
            CollectionAssert.AreEquivalent(typeof(List<string>).GetGenericArguments(), IEnumerableHelper.GetGenericIEnumerableType(typeof(List<string>)));
        }

        [TestMethod]
        public void GetGenericIEnumerableTypeInvalidType()
        {
            CollectionAssert.AreEquivalent(new Type[] { typeof(IEnumerable) }, IEnumerableHelper.GetGenericIEnumerableType(typeof(object)));
        }

        [TestMethod]
        public void ObfuscateIEnumerableNullObj()
        {
            Assert.AreEqual(null, IEnumerableHelper.ObfuscateIEnumerable(null, typeof(IEnumerable<object>), new Dictionary<object, object>()));
        }

        [TestMethod]
        public void ObfuscateIEnumerableInvalidType()
        {
            var obj = new List<string> { "Yes", "No" };
            var res = IEnumerableHelper.ObfuscateIEnumerable(obj, typeof(object), new Dictionary<object, object>());
            Assert.IsNull(res);
        }

        [TestMethod]
        public void ObfuscateIEnumerableDictionary()
        {
            var obj = new Dictionary<string, string> { { "Yes", "yes" }, { "No", "no" } };
            var res = IEnumerableHelper.ObfuscateIEnumerable(obj, typeof(Dictionary<string, string>), new Dictionary<object, object>());
            Assert.IsTrue(res is Dictionary<string, string>);
            Assert.AreNotEqual(obj, res);
        }

        [TestMethod]
        public void ObfuscateIEnumerableList()
        {
            var obj = new List<string> { "Yes", "No" };
            var res = IEnumerableHelper.ObfuscateIEnumerable(obj, typeof(IList<string>), new Dictionary<object, object>());
            Assert.IsNotNull(res as IList<string>);
            Assert.AreNotEqual(obj, res);
        }

        [TestMethod]
        public void ObfuscateIEnumerableCollection()
        {
            var obj = new List<string> { "Yes", "No" };
            var res = IEnumerableHelper.ObfuscateIEnumerable(obj, typeof(ICollection<string>), new Dictionary<object, object>());
            Assert.IsNotNull(res as ICollection<string>);
            Assert.AreNotEqual(obj, res);
        }

        [TestMethod]
        public void ObfuscateIEnumerableIEnumerable()
        {
            var obj = new List<string> { "Yes", "No" };
            var res = IEnumerableHelper.ObfuscateIEnumerable(obj, typeof(IEnumerable<string>), new Dictionary<object, object>());
            Assert.IsTrue(res is IEnumerable<string>);
            Assert.AreNotEqual(obj, res);
        }

        [TestMethod]
        public void ObfuscateIEnumerableNullObfuscated()
        {
            var obj = new List<string> { "Yes", "No" };
            var res = IEnumerableHelper.ObfuscateIEnumerable(new List<string> { "Yes", "No" }, typeof(IList<string>), null);
            Assert.AreNotEqual(obj, res);
        }
    }
}
