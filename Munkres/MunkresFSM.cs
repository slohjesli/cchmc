using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.AI.Models;
using CCHMC.Core.AI.Munkres.MunkresStates;

namespace CCHMC.Core.AI.Munkres
{
    public class MunkresFSM
    {
        /// <summary>
        /// The matrix which holds the data as well as the names for the assignments.
        /// </summary>
        public Matrix Matrix { get; set; }
        /// <summary>
        /// The assignment matrix, which correlates to the data matrix.
        /// </summary>
        public MatchResult Result { get; set; }
        /// <summary>
        /// A position of a zero in the matrix.
        /// </summary>
        public Tuple<int, int> RowCol { get; set; }
        /// <summary>
        /// The alternating series of starred and primed zeroes for Munkre's Algorithm.
        /// </summary>
        public List<Tuple<int, int>> Path { get; set; }
        /// <summary>
        /// The current step the algorithm is on.
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Creates a new Munkres Finite State Machine. In order to run the algorithm, call the Algorithm function on a MunkresFSM object.
        /// </summary>
        /// <param name="matrix">The matrix whose data will be analyzed for assignments.</param>
        public MunkresFSM (Matrix matrix)
        {
            //Initialize the matrix to the given matrix and creates an empty Results object, which will store the covers and assignments until completion.
            Matrix = matrix;
            Result = new MatchResult(Matrix.Data.Count(), Matrix.Data[0].Count());

            //Initialize the current state to Step 1, wherein the matrix is simplified by reducing each row and column by its minimum value.
            CurrentState = new Simplify(this);
        }
        
        /// <summary>
        /// Munkre's Algorithm, an algorithm which finds the minimum cost for assignments in a square matrix.
        /// </summary>
        /// <returns>The optimal set of assignments.</returns>
        public List<Assignment> Algorithm ()
        {
            //Until the code exits, it will execute the code associated with the current state; each state will update the CurrentState as appropriate.
            while (!(CurrentState is Done))
            {
                CurrentState.Continue();
            }
            //Once the FSM reaches Done, the algorithm has completed and a list of assignments can be made from the results matrix.
            return this.Assign(Result);
        }

        /// <summary>
        /// Converts the path created in Step 5: Create Path to a proper assignment matrix.
        /// </summary>
        public void AugmentPath ()
        {
            for (int i=0; i<Path.Count(); i++)
            {
                //Starred results are not assigned.
                if (Result.Matches[Path[i].Item1][Path[i].Item2]==1)
                {
                    Result.Matches[Path[i].Item1][Path[i].Item2] = 0;
                } 
                else //Otherwise, they are assigned.
                {
                    Result.Matches[Path[i].Item1][Path[i].Item2] = 1;
                }
            }
        }

        /// <summary>
        /// Clear any primed zeroes from the matrix.
        /// </summary>
        public void ErasePrimes ()
        {
            for (int row=0; row<Result.Matches.Count(); row++)
            {
                for (int col=0; col<Result.Matches.Count(); col++)
                {
                    if (Result.Matches[row][col]==2)
                    {
                        Result.Matches[row][col] = 0;
                    }
                }
            }
        }
    }
}
