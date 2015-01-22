using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.AI.Munkres.MunkresStates
{
    public class Simplify : State
    {
        MunkresFSM Munkres { get; set; }

        public Simplify (MunkresFSM munkres)
        {
            Munkres = munkres;
        }

        public void Continue ()
        {
            //Subtract the minimum from each row.
            for (int row=0; row<Munkres.Matrix.Data.Count(); row++)
            {
                double min = Munkres.Matrix.Data[row].Min();
                for (int col=0; col<Munkres.Matrix.Data[row].Count(); col++)
                {
                    Munkres.Matrix.Data[row][col] -= min;
                }
            }

            //Subtract the minimum from each column.
            for (int col=0; col<Munkres.Matrix.Data[0].Count(); col++)
            {
                var colmin = Munkres.Matrix.Data.Select(t => t[col]).Min();//Min in col x
                for (int row=0; row<Munkres.Matrix.Data.Count(); row++)
                {
                    Munkres.Matrix.Data[row][col] -= colmin;
                }
            }

            Munkres.CurrentState = new StarZeroes(Munkres);
        }

        public void Exit ()
        {
            throw new NotImplementedException("Cannot exit from Step 1.");
        }
    }
}
