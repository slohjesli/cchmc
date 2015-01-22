using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;

namespace CCHMC.Core.Web.Scrambler.Test.TestModels
{
    public class AttributedObject
    {
        public string defaultscramble { get; set; }

        [Scramble(Ignore = true)]
        public string donotscramble_flagged { get; set; }

        [ScrambleIgnore()]
        public string donotscramble_attr { get; set; }

        [Scramble("This is the scrambled version of scramblearg.")]
        public string scramblearg { get; set; }
        
        [ScrambleName("{FI}{MI}{LI}")]
        public string scrambleName { get; set; }
        
        [ScrambleText(ScrambleTextAttribute.Size.Sentence)]
        public string scrambleSentence { get; set; }

        [ScrambleText(ScrambleTextAttribute.Size.Paragraph)]
        public string scrambleParagraph { get; set; }

        [ScrambleDate()]
        public DateTime scrambleDate { get; set; }
        [ScrambleDate()]
        public string scrambleDateStr { get; set; }
        
        [ScrambleTime()]
        public DateTime scrambleTime { get; set; }
        [ScrambleTime()]
        public string scrambleTimeStr { get; set; }
        
        [ScrambleDateTime()]
        public DateTime scrambleDateTime { get; set; }
        [ScrambleDateTime()]
        public string scrambleDateTimeStr { get; set; }
        
        [ScrambleNumber(5, 27)]
        public int scrambleNum { get; set; }
        [ScrambleNumber(5, 27)]
        public string scrambleNumStr { get; set; }
        
        [ScrambleZip()]
        public int scrambleZip { get; set; }
        [ScrambleZip()]
        public string scrambleZipStr { get; set; }
        
        [ScrambleNumber(5, 27)]
        public int intProp { get; set; }
        
    }
}
