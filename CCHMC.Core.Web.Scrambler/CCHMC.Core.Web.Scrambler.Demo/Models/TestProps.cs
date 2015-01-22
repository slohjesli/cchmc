using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCHMC.Core.Web.Scrambler.Attributes;

namespace CCHMC.Core.Web.Scrambler.Demo.Models
{
    public class TestProps
    {
        //[Scramble]
        public bool boolField;
        //[Scramble]
        public bool boolProp { get; set; }
        
        //[Scramble]
        public char charField;
        //[Scramble]
        public char charProp { get; set; }
        
        [Scramble]
        public DateTime datetimeField;
        [Scramble]
        public DateTime datetimeProp { get; set; }
        
        //[Scramble]
        public decimal decField;
        //[Scramble]
        public decimal decProp { get; set; }
        
        [Scramble]
        public double doubField;
        [Scramble]
        public double doubProp { get; set; }
        
        //[Scramble]
        public float floatField;
        //[Scramble]
        public float floatProp { get; set; }

        public short shortField;
        public short shortProp { get; set; }

        [Scramble]
        public int intField;
        [Scramble]
        public int intProp { get; set; }
        
        [Scramble]
        public long longField;
        [Scramble]
        public long longProp { get; set; }
        
        //[Scramble]
        //public object objField;
        //[Scramble]
        //public object objProp { get; set; }
        
        //[Scramble]
        //public short shortField;
        //[Scramble]
        //public short shortProp { get; set; }
        
        [Scramble]
        public string strField;
        [Scramble]
        public string strProp { get; set; }
        
        public string unscrambledField;
        public string unscrambledProp { get; set; }
    }
}