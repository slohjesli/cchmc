using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class Check : State
    {
        MunkresFSM Munkres { get; set; }

        public Check (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            if (Munkres.Result.Columns.Where(t=>t==1).Sum() >= Munkres.Result.Columns.Count())
            {
                //If all columns are covered, the starred zeroes are a unique solution.
                Exit();
            } 
            else
            {
                //Otherwise, continue trying to solve it.
                Munkres.CurrentState = new PrimeZeroes(Munkres);
            }
        }

        public void Exit ()
        {
            Munkres.CurrentState = new Done();
        }
    }
}
