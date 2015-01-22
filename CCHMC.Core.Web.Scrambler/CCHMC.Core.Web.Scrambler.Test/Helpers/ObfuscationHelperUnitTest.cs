using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Settings;
using CCHMC.Core.Web.Scrambler.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Reflection;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Models;
using System.Text.RegularExpressions;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class ObfuscationHelperUnitTest
    {
        [TestInitialize]
        public void Initialize ()
        {
            ObfuscationSettings.IsActiveDefault = true;
        }

        [TestMethod]
        public void NullObfuscateTest ()
        {
            object test = null;
            Assert.IsNull(test.Obfuscate(), "Did not leave null value!");
        }

        [TestMethod]
        public void BaseTypeObfuscateTest ()
        {
            int test = -20;
            Assert.IsTrue((int)test.Obfuscate() >= 0, "Failed to obfuscate int!");
        }

        [TestMethod]
        public void StringObfuscateTest ()
        {
            string test = "Something completely different";
            Assert.AreEqual("REDACTED", (string)test.Obfuscate());
        }

        [TestMethod]
        public void ArrayObfuscateTest()
        {
            string[] test = new string[] { "Item 1", "Item 2", "Item 3", "Item 4" };
            CollectionAssert.AreNotEquivalent(test, (string[])test.Obfuscate());
        }

        [TestMethod]
        public void IEnumerableObfuscateTest()
        {
            var expected = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4" }.Select(t => t);
            var test = (IEnumerable<string>)expected.Obfuscate();
            var il = (test as IEnumerable<object>).ToList();

            var tEnum = test.GetEnumerator();
            var exEnum = expected.GetEnumerator();

            while (tEnum.MoveNext() && exEnum.MoveNext())
            {
                //Cycle through so more information can be given if it fails.
                Assert.AreNotEqual(exEnum.Current, tEnum.Current);
            }
            //Since all of the elements analyzed so far match, if this fails, they have different lengths.
            Assert.AreEqual(expected.Count(), test.Count(), "Result was not of the expected length!");
        }

        [TestMethod]
        public void ListObfuscateTest ()
        {
            List<string> test = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4" };
            var obf = test.Obfuscate();
            var type = obf.GetType();
            var tmp = ((List<string>)obf).ToList();
            CollectionAssert.AreNotEquivalent(test, (List<string>)obf);
        }

        [TestMethod]
        public void DictionaryObfuscateTest ()
        {
            Dictionary<string, string> test = new Dictionary<string, string> { {"a","Item 1"}, {"b", "Item 2"}, {"c", "Item 3"}, {"omega", "Item 4"} };
            CollectionAssert.AreNotEquivalent(test.Select(t=>t.Value).ToList(), ((Dictionary<string, string>)test.Obfuscate()).Select(t=>t.Value).ToList());
        }

        [TestMethod]
        public void ClassObfuscateTest ()
        {
            var test = new Subobject() {
                intField = 100,
                intProp = 2000,
                strField = "This sentence is false",
                strProp = "I am lying right now.",
            };
            
            test = test.Obfuscate() as Subobject;

            Assert.AreNotEqual(100, test.intField, "intField was not obfuscated! (Chance of obfuscation matching: 0.1%)");
            Assert.AreNotEqual(2000, test.intProp, "intProp was not obfuscated! (Chance of obfuscation matching: 0.01%)");
            Assert.AreNotEqual("This sentence is false", test.strField, "strField was not obfuscated!");
            Assert.AreNotEqual("I am lying right now.", test.strProp, "strProp was not obfuscated!");
        }

        [TestMethod]
        public void SubClassObfuscateTest ()
        {
            var sub = new Subobject() {
                intField = 100,
                intProp = 2000,
                strField = "This sentence is false",
                strProp = "I am lying right now.",
            };
            var test = new ScrambleModel() {
                subField = sub,
                subProp = sub
            };
            test = test.Obfuscate() as ScrambleModel;
            
            Assert.AreNotEqual(100, test.subField.intField, "intField was not obfuscated! (Chance of obfuscation matching: 0.1%)");
            Assert.AreNotEqual(2000, test.subField.intProp, "intProp was not obfuscated! (Chance of obfuscation matching: 0.01%)");
            Assert.AreNotEqual("This sentence is false", test.subField.strField, "strField was not obfuscated!");
            Assert.AreNotEqual("I am lying right now.", test.subField.strProp, "strProp was not obfuscated!");
        }

        [TestMethod]
        public void StaticListObfuscationModification()
        {
            List<SimpleObject> obf = (List<SimpleObject>)StaticClass.SimpleObjects.Obfuscate();

            CollectionAssert.AreNotEqual(StaticClass.SimpleObjects, obf);
            CollectionAssert.AreNotEquivalent(StaticClass.SimpleObjects, obf);

            Assert.AreNotSame(StaticClass.SimpleObjects[0], obf[0]);
            Assert.AreNotSame(StaticClass.SimpleObjects[1], obf[1]);
            Assert.AreNotEqual(StaticClass.SimpleObjects[0].Name, obf[0].Name);
            Assert.AreNotEqual(StaticClass.SimpleObjects[1].Name, obf[1].Name);
        }

        [TestMethod]
        public void StaticDictionaryObfuscationModification()
        {
            Dictionary<string, SimpleObject> obf = (Dictionary<string, SimpleObject>)StaticClass.SimpleDictionary.Obfuscate();

            CollectionAssert.AreNotEqual(StaticClass.SimpleObjects, obf);
            CollectionAssert.AreNotEquivalent(StaticClass.SimpleObjects, obf);
        }

        [TestMethod]
        public void StaticIEnumerableObfuscationModification()
        {
            IEnumerable<SimpleObject> obf = (IEnumerable<SimpleObject>)StaticClass.SimpleObjectsSelect.Obfuscate();
            Assert.IsFalse(Enumerable.SequenceEqual(StaticClass.SimpleObjectsSelect, obf));
        }

        [TestMethod]
        public void StaticObjectObfuscationModification()
        {
            ScrambleModel obf = (ScrambleModel)StaticClass.ScrambleModel.Obfuscate();
            Assert.AreNotEqual(StaticClass.ScrambleModel.subProp.strProp, obf.subProp.strProp);
        }

        [TestMethod]
        public void RecursionTest ()
        {
            RecursiveObject bro = new RecursiveObject();
            RecursiveObject sis = new RecursiveObject() { sibling = bro };
            bro.sibling = sis;
            bool success = true;
            string info = "";

            try
            {
                bro.Obfuscate();
                sis.Obfuscate();
            } catch (Exception ex)
            {
                success = false;
                info = ex.Message;
            }
            Assert.IsTrue(success, info);
        }

        [TestMethod]
        public void AnonymousObfuscationTest()
        {
            //Anonymous objects are immutable and won't be changed.
            var test = new { a = "alpha", b = 2, c = false };
            test.Obfuscate();

            Assert.AreEqual("alpha", test.a);
            Assert.AreEqual(2, test.b);
            Assert.IsFalse(test.c);
        }

        [TestMethod]
        public void GetObfuscationNullValue()
        {
            Assert.IsNull(ObfuscationHelper.GetObfuscation(typeof(object), null, null, null, null));
        }

        [TestMethod]
        public void GetObfuscationGettableAttribute()
        {
            var obf = ObfuscationHelper.GetObfuscation(typeof(string), null, "Something random", new FakeMemberInfo("FullName"), null);
            Assert.AreNotEqual("Something random", obf);
            Assert.IsTrue(Regex.IsMatch(obf as string, @"^[A-Z][a-z]+, [A-Z][a-z]+ [A-Z][a-z]+$"), String.Format("Did not use ScrambleNameAttribute for obfuscation! ({0})", obf as string));
        }

        [TestMethod]
        public void GetObfuscationUngettableAttributeNullHistory()
        {
            var obj = new SimpleObject() { Name = "Some kinda name" };
            var obf = ObfuscationHelper.GetObfuscation(null, null, obj, null, null);
            Assert.AreNotEqual(obj.Name, ((SimpleObject)obf).Name);
        }

        [TestMethod]
        public void GetObfuscationUngettableAttribute()
        {
            var obj = new SimpleObject() { Name = "Some kinda name" };
            var obf = ObfuscationHelper.GetObfuscation(null, null, obj, null, new Dictionary<object, object>());
            Assert.AreNotEqual(obj.Name, ((SimpleObject)obf).Name);
        }

        [TestMethod]
        public void GetObfuscationValidAttribute()
        {
            Assert.AreEqual(5, ObfuscationHelper.GetObfuscation(typeof(int), new ScrambleAttribute(5), 2, null, null));
        }

        [TestMethod]
        public void DefaultObfuscateWholeNumbers()
        {
            foreach (var type in new List<Type> { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) })
            {
                Assert.AreEqual(ObfuscationHelper.DefaultObfuscate(type).GetType(), type);
            }
        }

        [TestMethod]
        public void DefaultObfuscateNonwholeNumbers()
        {
            foreach (var type in new List<Type> { typeof(double), typeof(decimal), typeof(float) })
            {
                Assert.AreEqual(ObfuscationHelper.DefaultObfuscate(type).GetType(), type);
            }
        }

        [TestMethod]
        public void DefaultObfuscateString()
        {
            Assert.AreEqual("REDACTED", ObfuscationHelper.DefaultObfuscate(typeof(string)));
        }

        [TestMethod]
        public void DefaultObfuscateCharArray()
        {
            CollectionAssert.AreEquivalent("REDACTED".ToCharArray().ToList(), ((char[])ObfuscationHelper.DefaultObfuscate(typeof(char[]))).ToList());
        }

        [TestMethod]
        public void DefaultObfuscateChar()
        {
            Assert.IsNotNull(ObfuscationHelper.DefaultObfuscate(typeof(char)) as char?);
        }

        [TestMethod]
        public void DefaultObfuscateNullableBoolDistribution()
        {
            Dictionary<string, int> dict = new Dictionary<string, int> {
                { true.ToString(), 0 }, { false.ToString(), 0 }, { "", 0 }
            };
            for (int i = 0; i < 3000; i++)
            {
                dict[((bool?)ObfuscationHelper.DefaultObfuscate(typeof(bool?))).ToString()]++;
            }
            Assert.IsTrue(dict[""] < 1200 && dict[""] > 800, dict[""].ToString());//Average = 1000; Probablility per point = 1/3; (1/3)(1^2 + 0^2 + 0^2) - 1000^2
            Assert.IsTrue(dict["True"] < 1200 && dict["True"] > 800, dict["True"].ToString());
            Assert.IsTrue(dict["False"] < 1200 && dict["False"] > 800, dict["False"].ToString());
        }

        [TestMethod]
        public void DefaultObfuscateBoolDistribution()
        {
            Dictionary<bool, int> dict = new Dictionary<bool, int> {
                { true, 0 }, { false, 0 }
            };
            for (int i = 0; i < 2000; i++)
            {
                dict[(bool)ObfuscationHelper.DefaultObfuscate(typeof(bool))]++;
            }
            Assert.IsTrue(dict[true] < 1200 && dict[true] > 800);
            Assert.IsTrue(dict[false] < 1200 && dict[false] > 800);
        }

        [TestMethod]
        public void TryGetAttributeNumberType()
        {
            foreach (var type in new List<Type> { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(double), typeof(decimal), typeof(float) })
            {
                Assert.AreEqual(typeof(ScrambleNumberAttribute), ObfuscationHelper.TryGetAttribute(type, 5, null).GetType());
            }
        }

        [TestMethod]
        public void TryGetAttributeChar()
        {
            Assert.AreEqual(typeof(ScrambleTextAttribute), ObfuscationHelper.TryGetAttribute(typeof(char), 'a', null).GetType());
        }

        [TestMethod]
        public void TryGetAttributeStringNumber()
        {
            Assert.AreEqual(typeof(ScrambleNumberAttribute), ObfuscationHelper.TryGetAttribute(typeof(string), "5555555555555555555", null).GetType());
        }

        [TestMethod]
        public void TryGetAttributeStringDateTime()
        {
            Assert.AreEqual(typeof(ScrambleDateTimeAttribute), ObfuscationHelper.TryGetAttribute(typeof(string), "5/7/1992 12:32:01.22", null).GetType());
        }

        [TestMethod]
        public void TryGetAttributeStringFirstName()
        {
            MemberInfo mem = typeof(Name).GetProperties().First(t => t.Name == "FirstName");
            var attr = ObfuscationHelper.TryGetAttribute(typeof(string), "Sally", mem) as ScrambleNameAttribute;
            Assert.IsNotNull(attr);
            Assert.IsTrue(NameHelper.FemaleFNames.Contains(attr.Obfuscate(String.Empty)) || NameHelper.MaleFNames.Contains(attr.Obfuscate(String.Empty)));
        }

        [TestMethod]
        public void TryGetAttributeStringMiddleName()
        {
            MemberInfo mem = typeof(Name).GetProperties().First(t => t.Name == "MiddleName");
            var attr = ObfuscationHelper.TryGetAttribute(typeof(string), "Tai", mem) as ScrambleNameAttribute;
            Assert.IsNotNull(attr);
            Assert.IsTrue(NameHelper.FemaleMNames.Contains(attr.Obfuscate(String.Empty)) || NameHelper.MaleMNames.Contains(attr.Obfuscate(String.Empty)));
        }

        [TestMethod]
        public void TryGetAttributeStringLastName()
        {
            MemberInfo mem = typeof(Name).GetProperties().First(t => t.Name == "LastName");
            var attr = ObfuscationHelper.TryGetAttribute(typeof(string), "Roberts", mem) as ScrambleNameAttribute;
            Assert.IsNotNull(attr);
            Assert.IsTrue(NameHelper.LNames.Contains(attr.Obfuscate(String.Empty)));
        }

        [TestMethod]
        public void TryGetAttributeStringName()
        {
            MemberInfo mem = typeof(Name).GetProperties().First(t => t.Name == "FullName");
            ScrambleNameAttribute attr = ObfuscationHelper.TryGetAttribute(typeof(string), "John Jacob Jingleheimer Schmidt", mem) as ScrambleNameAttribute;
            Assert.IsNotNull(attr);
            Assert.IsTrue(new Regex(@"[A-Za-z]+, [A-Za-z]+ [A-Za-z]+").IsMatch((string)attr.Obfuscate(String.Empty)));
        }

        [TestMethod]
        public void TryGetAttributeDateTime()
        {
            Assert.AreEqual(typeof(ScrambleDateTimeAttribute), ObfuscationHelper.TryGetAttribute(typeof(DateTime), DateTime.Now, null).GetType());
        }

        [TestMethod]
        public void TryGetAttributeBoolean()
        {
            Assert.AreEqual(typeof(ScrambleAttribute), ObfuscationHelper.TryGetAttribute(typeof(bool), true, null).GetType());
        }

        [TestMethod]
        public void TryGetAttributeNullableBoolean()
        {
            Assert.AreEqual(typeof(ScrambleAttribute), ObfuscationHelper.TryGetAttribute(typeof(bool?), true, null).GetType());
        }

        [TestMethod]
        public void TryGetAttributeInvalid()
        {
            Assert.IsNull(ObfuscationHelper.TryGetAttribute(typeof(Name), new Name(), null));
        }

        [TestMethod]
        public void CloneNull()
        {
            object test = null;
            Assert.IsNull(test.Clone());
        }

        [TestMethod]
        public void CloneString()
        {
            var test = "Hello";
            var clone = test.Clone();
            Assert.AreEqual(test, clone);
            clone += "!";
            Assert.AreNotEqual(test, clone);
        }

        [TestMethod]
        public void CloneBaseType()
        {
            int test = 1;
            int clone = (int)test.Clone();
            Assert.AreEqual(test, clone);
            Assert.AreNotSame(test, clone);
            clone++;
            Assert.AreNotEqual(test, clone);
        }

        [TestMethod]
        public void CloneCustomObject()
        {
            SimpleObject test = new SimpleObject() { Name="Josh" };
            var clone = test.Clone() as SimpleObject;
            Assert.AreEqual(test.Name, clone.Name);
            Assert.AreNotSame(test, clone);
            clone.Name += " E.";
            Assert.AreNotEqual(test, clone);
        }

        [TestMethod]
        public void CloneIsShallow()
        {
            ScrambleModel test = new ScrambleModel() { intProp=19, strProp="May", subProp = new Subobject() { intProp=27, strProp="Hello" } };
            var clone = test.Clone() as ScrambleModel;
            Assert.AreEqual(test.strProp, clone.strProp);
            Assert.AreEqual(test.intProp, clone.intProp);
            Assert.AreNotSame(test, clone);
            Assert.AreSame(test.subProp, clone.subProp);
            clone.strProp += "day!";
            clone.intProp++;
            Assert.AreNotEqual(test.strProp, clone.strProp);
            Assert.AreNotEqual(test.intProp, clone.intProp);
        }
    }
}
