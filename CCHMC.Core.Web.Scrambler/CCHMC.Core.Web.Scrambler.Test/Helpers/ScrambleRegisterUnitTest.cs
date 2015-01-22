using System;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class ScrambleRegisterUnitTest
    {
        Type type = typeof(SimpleObject);

        [TestCleanup]
        public void Cleanup()
        {
            ScrambleRegister.RegisteredTypes = new Dictionary<Type,ScrambleAttribute>();
            ScrambleRegister.RegisteredMembers = new Dictionary<Type, Dictionary<string, ScrambleAttribute>>();
        }


        [TestMethod]
        public void ScrambleRegisterIgnoreMember()
        {
            ScrambleRegister.Ignore<SimpleObject>(t => t.Name);

            var scram = ScrambleRegister.GetScrambleAttribute(type, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());
        }


        [TestMethod]
        public void RegisterIgnoreType ()
        {
            ScrambleRegister.Ignore<SimpleObject>();
            var scram = ScrambleRegister.GetScrambleAttribute(type);
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());
        }

        [TestMethod]
        public void RegisterAlternateAttributeType()
        {
            ScrambleRegister.Register<SimpleObject>(new ScrambleNameAttribute());
            var scram = ScrambleRegister.GetScrambleAttribute(type);
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleNameAttribute), scram.GetType());
        }

        [TestMethod]
        public void RegisterNullAttributeType()
        {
            ScrambleRegister.Register<SimpleObject>(null);
            var scram = ScrambleRegister.GetScrambleAttribute(type);
            Assert.IsNull(scram);
        }
        
        [TestMethod]
        public void RegisterIgnoreMember ()
        {
            ScrambleRegister.Ignore<SimpleObject>(t => t.Name);
            var members = type.GetMember("Name");

            var scram = ScrambleRegister.GetScrambleAttribute(type, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());
        }

        [TestMethod]
        public void RegisterAlternateAttributeMember ()
        {
            ScrambleRegister.Register<SimpleObject>(t => t.Name, new ScrambleNameAttribute());

            var scram = ScrambleRegister.GetScrambleAttribute(type, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleNameAttribute), scram.GetType());
        }

        [TestMethod]
        public void RegisterDefaultMember()
        {
            ScrambleRegister.Register<SimpleObject>(t => t.Name, new ScrambleAttribute());

            var scram = ScrambleRegister.GetScrambleAttribute(type, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleAttribute), scram.GetType());
        }

        [TestMethod]
        public void RegisterNullMember()
        {
            bool argnullex = false;
            try
            {
                ScrambleRegister.Register<SimpleObject>(null, new ScrambleAttribute());
            }
            catch (ArgumentNullException ex)
            {
                argnullex = true;
            }

            Assert.IsTrue(argnullex);
        }

        [TestMethod]
        public void GetRegisteredType ()
        {
            ScrambleRegister.Register<SimpleObject>(new ScrambleAttribute());
            var test = ScrambleRegister.GetScrambleAttribute(type);
            Assert.AreEqual(typeof(ScrambleAttribute), test.GetType());
        }

        [TestMethod]
        public void GetUnregisteredType ()
        {
            var test = ScrambleRegister.GetScrambleAttribute(type);
            Assert.IsNull(test);
        }

        [TestMethod]
        public void GetRegisteredMember ()
        {
            ScrambleRegister.Register<SimpleObject>(t => t.Name, new ScrambleAttribute());
            var test = ScrambleRegister.GetScrambleAttribute<SimpleObject>(t => t.Name);
            Assert.AreEqual(typeof(ScrambleAttribute), test.GetType());
        }

        [TestMethod]
        public void GetUnregistered ()
        {
            var test = ScrambleRegister.GetScrambleAttribute<SimpleObject>(t => t.Name);
            Assert.IsNull(test);
        }

        [TestMethod]
        public void GenericIgnoreType()
        {
            ScrambleRegister<SimpleObject> register = new ScrambleRegister<SimpleObject>();
            register.Ignore();

            var scram = ScrambleRegister.GetScrambleAttribute(type);
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());
        }

        [TestMethod]
        public void GenericIgnoreMember()
        {
            ScrambleRegister<SimpleObject> register = new ScrambleRegister<SimpleObject>();
            register.Ignore(t => t.Name);

            var scram = ScrambleRegister.GetScrambleAttribute(type, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());
        }

        [TestMethod]
        public void GenericRegisterType()
        {
            ScrambleRegister<SimpleObject> register = new ScrambleRegister<SimpleObject>();
            register.Register(new ScrambleNameAttribute());

            var scram = ScrambleRegister.GetScrambleAttribute(type);
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleNameAttribute), scram.GetType());
        }

        [TestMethod]
        public void GenericRegisterMember()
        {
            ScrambleRegister<SimpleObject> register = new ScrambleRegister<SimpleObject>();
            register.Register(t => t.Name, new ScrambleNameAttribute());

            var scram = ScrambleRegister.GetScrambleAttribute(type, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleNameAttribute), scram.GetType());
        }

        [TestMethod]
        public void GenericChainCommands()
        {
            Type tp = typeof(SimpleObjectLarger);
            ScrambleRegister<SimpleObjectLarger> register = new ScrambleRegister<SimpleObjectLarger>();
            register.Register(new ScrambleAttribute())
                    .Ignore(t => t.Name)
                    .Register(t => t.AirspeedVelocityOfUnladenSwallow_European, new ScrambleNumberAttribute(20, 30))
                    .Register(t => t.AirspeedVelocityOfUnladenSwallow_African, new ScrambleNumberAttribute(30, 40))
                    .Ignore(t => t.Id);

            var scram = ScrambleRegister.GetScrambleAttribute(tp);
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleAttribute), scram.GetType());

            scram = ScrambleRegister.GetScrambleAttribute(tp, "Name");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());

            scram = ScrambleRegister.GetScrambleAttribute(tp, "AirspeedVelocityOfUnladenSwallow_European");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleNumberAttribute), scram.GetType());

            scram = ScrambleRegister.GetScrambleAttribute(tp, "AirspeedVelocityOfUnladenSwallow_African");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleNumberAttribute), scram.GetType());

            scram = ScrambleRegister.GetScrambleAttribute(tp, "Id");
            Assert.IsNotNull(scram);
            Assert.AreEqual(typeof(ScrambleIgnoreAttribute), scram.GetType());
        }
    }
}
