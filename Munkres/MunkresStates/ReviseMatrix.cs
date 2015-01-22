using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class ReviseMatrix : State
    {
        MunkresFSM Munkres { get; set; }

        public ReviseMatrix (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            double min = Munkres.FindMin();
            for (int row=0; row<Munkres.Result.Matches.Count(); row++)
            {
                for (int col=0; col<Munkres.Result.Matches.Count(); col++)
                {
                    if (Munkres.Result.Rows[row]==1)
                    {
                        Munkres.Matrix.Data[row][col] += min;
                    }
                    if (Munkres.Result.Columns[col]==0)
                    {
                        Munkres.Matrix.Data[row][col] -= min;
                    }
                }
            }
            Munkres.CurrentState = new PrimeZeroes(Munkres);
        }

        public void Exit ()
        {
            throw new NotImplementedException("Cannot exit from Step 6.");
        }
    }
}
