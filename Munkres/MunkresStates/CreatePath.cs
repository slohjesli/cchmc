using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class CreatePath : State
    {
        MunkresFSM Munkres { get; set; }

        public CreatePath (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            bool done = false;
            int r = -1;
            int c = -1;
            Munkres.Path = new List<Tuple<int, int>>();
            Munkres.Path.Add(Munkres.RowCol);

            while (!done)
            {
                r = Munkres.FindStarInCol(Munkres.Path.Last().Item2);
                if (r!=-1)
                {
                    Munkres.Path.Add(new Tuple<int, int>(r, Munkres.Path.Last().Item2));
                } 
                else
                {
                    done = true;
                }
                if (!done)
                {
                    c = Munkres.FindPrimeInRow(Munkres.Path.Last().Item1, Munkres.Path.Last().Item2);
                    Munkres.Path.Add(new Tuple<int,int>(Munkres.Path.Last().Item1, c));
                }
            }

            Munkres.AugmentPath();
            Munkres.Result.ClearCovers();
            Munkres.ErasePrimes();
            Munkres.CurrentState = new CoverColumns(Munkres);
        }

        public void Exit ()
        {
            throw new NotImplementedException("Cannot exit from Step 5.");
        }
    }
}
