using ExternalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScramblerPresentation.Models
{
    public class IndexClass
    {
        public string Name { get; set; }
        public List<InternalClass> Internals { get; set; }
        public List<InternalClass> Alts { get; set; }
        public List<ExternalClass> Externals { get; set; }
    }
}