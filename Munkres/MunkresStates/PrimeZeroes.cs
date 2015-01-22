using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class PrimeZeroes : State
    {
        MunkresFSM Munkres { get; set; }

        public PrimeZeroes (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            //Default values
            Tuple<int, int> rowcol = new Tuple<int, int>(-1, -1);
            bool done = false;

            while (!done)
            {
                rowcol = Munkres.FindZero();
                if (rowcol.Item1==-1)
                {
                    done = true;
                    Munkres.CurrentState = new ReviseMatrix(Munkres);
                } 
                else
                {
                    Munkres.Result.Matches[rowcol.Item1][rowcol.Item2] = 2;
                    if (Munkres.StarInRow(rowcol.Item1))
                    {
                        rowcol = new Tuple<int,int>(rowcol.Item1, Munkres.FindStarInRow(rowcol.Item1));
                        Munkres.Result.Rows[rowcol.Item1] = 1;
                        Munkres.Result.Columns[rowcol.Item2] = 0;
                    } 
                    else
                    {
                        done = true;
                        Munkres.RowCol = rowcol;
                        Munkres.CurrentState = new CreatePath(Munkres);
                    }
                }
            }
        }

        public void Exit ()
        {
            throw new NotImplementedException("Cannot exit from Step 4.");
        }
    }
}
