using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class StarZeroes : State
    {
        MunkresFSM Munkres { get; set; }

        public StarZeroes (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            for (int row=0; row<Munkres.Matrix.Data.Count(); row++)
            {
                for (int col=0; col<Munkres.Matrix.Data[0].Count(); col++)
                {
                    //Check that it is a zero and that the column and row are not covered.
                    if (Munkres.Matrix.Data[row][col]==0 && Munkres.Result.Rows[row]==0 && Munkres.Result.Columns[col]==0)
                    {
                        //Star the zero
                        Munkres.Result.Matches[row][col] = 1;
                        //Cover the row and column
                        Munkres.Result.Rows[row] = 1;
                        Munkres.Result.Columns[col] = 1;
                    }
                }
            }
            Munkres.Result.ClearCovers();
            Munkres.CurrentState = new CoverColumns(Munkres);
        }

        public void Exit ()
        {
            throw new NotImplementedException("Cannot exit from Step 2.");
        }
    }
}
