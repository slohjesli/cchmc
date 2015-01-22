using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCHMC.Core.Web.Scrambler.Demo.Models
{
    public class SpeedResults
    {
        public int NumberOfExecutions { get; set; }
        public TimeSpan UnobfuscatedTime { get; set; }
        public TimeSpan ObfuscatedTime { get; set; }
    }
}