using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class Done : State
    {
        public void Continue ()
        {
            throw new NotImplementedException("Already finished, cannot continue.");
        }

        public void Exit ()
        {
            throw new NotImplementedException("Already exited.");
        }
    }
}
