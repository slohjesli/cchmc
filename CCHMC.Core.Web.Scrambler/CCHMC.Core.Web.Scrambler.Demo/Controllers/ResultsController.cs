using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCHMC.Core.Web.Scrambler.Demo.Models;

namespace CCHMC.Core.Web.Scrambler.Demo.Controllers
{
    public class ResultsController : ApiController
    {
        // GET api/<controller>
        public DisplayAttributeTest Get()
        {

            var test = new DisplayAttributeTest();

            test.thing1 = "Cat";
            test.thing2 = "Hat";
            test.temporal = DateTime.Now;

            return test;
        }
    }
    public class BasicController : ApiController
    {
        // GET api/<controller>
        public BasicClass Get()
        {
            var test = new BasicClass();
            test.Name = "That one guy";

            return test;
        }
    }
    public class RecursiveController : ApiController
    {
        // GET api/<controller>
        public NestedObject Get ()
        {

            var test = new NestedObject {
                Name = "Robert Robertson",
                ID = 1,
                Birthdate = DateTime.Now
            };

            NestedObject cur = test;
            for (int i=0; i < 10; i++)
            {
                cur = cur.SpawnChild();
            }

            return test;
        }
    }
    public class PropertiesController : ApiController
    {

        public TestProps GetProperties ()
        {
            var test = new TestProps (){
                boolField = false,
                boolProp = false,
                charField = 'J',
                charProp = 'J',
                datetimeField = DateTime.Now,
                datetimeProp = DateTime.UtcNow,
                decField = 1.2345m,
                decProp = 5.4321m,
                doubField = 1.23,
                doubProp = 3.21,
                floatField = 7.89F,
                floatProp = 9.87F,
                shortField = 50,
                shortProp = 55,
                intField = 100,
                intProp = 2000,
                longField = 1000000000L,
                longProp = 9999999999L,
                //objField = new { a=1, b=2 },
                //objProp = new { c="one", d="two" },
                strField = "This sentence is false",
                strProp = "I am lying right now.",
                unscrambledField = "This should not be scrambled.",
                unscrambledProp = "Neither should this be scrambled."
            };

            return test;
        }
    }
    public class ClassController : ApiController
    {
        //[Scrambler]
        public TestClass GetClass ()
        {
            var test = new TestClass (){
                boolField = false,
                boolProp = false,
                charField = 'J',
                charProp = 'J',
                datetimeField = DateTime.Now,
                datetimeProp = DateTime.UtcNow,
                decField = 1.2345m,
                decProp = 5.4321m,
                doubField = 1.23,
                doubProp = 3.21,
                floatField = 7.89F,
                floatProp = 9.87F,
                shortField = 50,
                shortProp = 55,
                intField = 100,
                intProp = 2000,
                longField = 1000000000L,
                longProp = 9999999999L,
                //objField = new { a=1, b=2 },
                //objProp = new { c="one", d="two" },
                strField = "This sentence is false",
                strProp = "I am lying right now.",
                unscrambledField = "This should not be scrambled.",
                unscrambledProp = "Neither should this be scrambled."
            };

            return test;
        }
    }

    public class AttributesController : ApiController
    {
        public TestAttributes GetSpecific ()
        {
            var test = new TestAttributes() {
                intProp = 12345,
                scramblearg = "SENSITIVE DATA, SHOULD NOT BE SEEN",
                scrambleDate = new DateTime(2014, 5, 28),
                scrambleDateStr = "I'm not really a time",
                scrambleDateTime = DateTime.Now,
                scrambleDateTimeStr = "Neither am I",
                scrambleName = "John Jacob Jingleheimer Schmidt",
                scrambleNum = 11497,
                scrambleNumStr = "Nor am I! We should start a club or something.",
                scrambleParagraph = "As Gregor Samsa awoke one morning from uneasy dreams he found himself transformed in his bed into a gigantic insect. He was lying on his hard, as it were armor-plated, back and when he lifted his head a little he could see his dome-like brown belly divided into stiff arched segments on top of which the bed quilt could hardly keep in position and was about to slide off completely. His numerous legs, which were pitifully thin compared to the rest of his bulk, waved helplessly before his eyes.",
                scrambleSentence = "What has happened to me? he thought.",
                scrambleTime = new DateTime(DateTime.Now.TimeOfDay.Ticks),
                scrambleTimeStr = "Indeed we should!",
                scrambleZip = 45220,
                scrambleZipStr = "Eh, maybe some other time."
            };

            return test;
        }
    }
}