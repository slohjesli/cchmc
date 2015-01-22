using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CCHMC.Core.Web.Scrambler.Attributes;

namespace CCHMC.Core.Web.Scrambler.Demo.Models
{
    public class DisplayAttributeTest
    {
        [Scramble("ThingOnePlaceholder")]
        public string thing1;

        [ScrambleName]
        public string thing2 { get; set; }

        [ScrambleDate]
        [DisplayFormat(DataFormatString="{0:d}")]
        public DateTime temporal { get; set; }
    }
}