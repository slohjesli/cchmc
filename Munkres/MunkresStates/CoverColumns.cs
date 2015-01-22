using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class CoverColumns : State
    {
        MunkresFSM Munkres { get; set; }

        public CoverColumns (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            //Get the columns of starred zeroes.
            for (int row=0; row<Munkres.Matrix.Data.Count(); row++)
            {
                for (int col=0; col<Munkres.Matrix.Data[0].Count(); col++)
                {
                    if (Munkres.Result.Matches[row][col]==1)
                    {
                        Munkres.Result.Columns[col] = 1;
                    }
                }
            }
            Munkres.CurrentState = new Check(Munkres);
        }

        public void Exit ()
        {
            Munkres.CurrentState = new Done();
        }
    }
}
