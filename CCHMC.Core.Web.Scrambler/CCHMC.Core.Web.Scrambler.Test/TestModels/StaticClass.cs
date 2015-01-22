using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Test.TestModels
{
    public static class StaticClass
    {
        private static List<SimpleObject> simpleObjects = new List<SimpleObject> 
        { 
            new SimpleObject { Name = "Boq" }, 
            new SimpleObject { Name = "Wicked Witch" } 
        };
        public static List<SimpleObject> SimpleObjects
        {
            get
            {
                return simpleObjects;
            }
        }

        public static IEnumerable<SimpleObject> SimpleObjectsSelect
        {
            get
            {
                return simpleObjects.Select(t=>t);
            }
        }

        private static Dictionary<string, SimpleObject> simpleDictionary = new Dictionary<string, SimpleObject> 
        { 
            { "Silver", new SimpleObject { Name = "Boq" } }, 
            { "Green", new SimpleObject { Name = "Wicked Witch" } }
        };
        public static Dictionary<string, SimpleObject> SimpleDictionary
        {
            get
            {
                return simpleDictionary;
            }
        }

        private static ScrambleModel scrambleModel = new ScrambleModel 
        { 
            subProp = new Subobject { strProp = "String!" } 
        };
        public static ScrambleModel ScrambleModel
        {
            get
            {
                return scrambleModel;
            }
        }
    }
}
