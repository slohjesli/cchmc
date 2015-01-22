using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCHMC.Core.Web.Scrambler.Attributes;

namespace CCHMC.Core.Web.Scrambler.Demo.Models
{
    //[Scramble(Ignore=true)]
    public class TestClass
    {
        public bool boolField;
        public bool boolProp { get; set; }
        
        public char charField;
        public char charProp { get; set; }

        //[Scramble(Ignore = true)]
        public int PrimaryId { get; set; }
        
        //[Scramble( Strict = true, ]
        public DateTime datetimeField;
        public DateTime datetimeProp { get; set; }

        public decimal decField;
        public decimal decProp { get; set; }
        
        public double doubField;
        public double doubProp { get; set; }
        
        public float floatField;
        public float floatProp { get; set; }
        
        public short shortField;
        public short shortProp { get; set; }

        public int intField;
        public int intProp { get; set; }
        
        public long longField;
        public long longProp { get; set; }
        
        //public object objField;
        //public object objProp { get; set; }
        
        public string strField;
        public string strProp { get; set; }

        [Scramble(Ignore=true)]
        public string unscrambledField;
        [Scramble(Ignore=true)]
        public string unscrambledProp { get; set; }
    }
}