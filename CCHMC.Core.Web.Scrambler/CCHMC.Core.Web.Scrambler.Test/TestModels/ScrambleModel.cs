using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Test.TestModels
{
    public class ScrambleModel
    {
        public Subobject subField;
        public Subobject subProp { get; set; }

        public DateTime datetimeField;
        public DateTime datetimeProp { get; set; }

        public bool boolField;
        public bool boolProp { get; set; }
        
        public byte byteField;
        public byte byteProp { get; set; }
        
        public sbyte sbyteField;
        public sbyte sbyteProp { get; set; }
        
        public char charField;
        public char charProp { get; set; }
        
        public decimal decField;
        public decimal decProp { get; set; }
        
        public double doubField;
        public double doubProp { get; set; }
        
        public float floatField;
        public float floatProp { get; set; }
        
        public int intField;
        public int intProp { get; set; }
        
        public uint uintField;
        public uint uintProp { get; set; }
        
        public long longField;
        public long longProp { get; set; }
        
        public ulong ulongField;
        public ulong ulongProp { get; set; }

        public object objField;
        public object objProp { get; set; }
        
        public short shortField;
        public short shortProp { get; set; }
        
        public ushort ushortField;
        public ushort ushortProp { get; set; }
        
        public string strField;
        public string strProp { get; set; }
    }
}
