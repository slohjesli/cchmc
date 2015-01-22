using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Attributes
{
    /// <summary>
    /// Attribute which marks classes, properties, and fields not to be scrambled when the ActionFilter scramblers are in use.
    /// </summary>
    public class ScrambleIgnoreAttribute : ScrambleAttribute
    {
        /// <summary>
        /// Marks classes, properties, and fields not to be scrambled when the ActionFilter scramblers are in use.
        /// </summary>
        public ScrambleIgnoreAttribute ()
        {
            Ignore = true;
        }
    }
}
