using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.AI.Models;

namespace CCHMC.Core.AI.Munkres
{
    //Based on http://csclab.murraystate.edu/bob.pilgrim/445/munkres.html

    public class Munkres
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
        /// The current step the algorithm is on.
        /// </summary>
        private int Step { get; set; }
        /// <summary>
        /// A position of a zero in the matrix.
        /// </summary>
        private Tuple<int, int> RowCol { get; set; }
        /// <summary>
        /// The alternating series of starred and primed zeroes for Munkre's Algorithm.
        /// </summary>
        private List<Tuple<int, int>> Path { get; set; }

        public Munkres (Matrix matrix)
        {
            Matrix = matrix;
            Result = new MatchResult(Matrix.Data.Count(), Matrix.Data[0].Count());
            Step = 1;
        }

        /// <summary>
        /// Munkre's Algorithm, an algorithm which finds the minimum cost for assignments in a square matrix.
        /// </summary>
        /// <param name="matrix">The matrix containing the data for assignment.</param>
        /// <returns>The optimal set of assignments.</returns>
        public List<Assignment> Algorithm ()
        {
            bool done = false;

            while (!done)
            {
                switch (Step)
                {
                    case 1:
                        Step1();
                        break;
                    case 2:
                        Step2();
                        break;
                    case 3:
                        Step3();
                        break;
                    case 4:
                        Step4();
                        break;
                    case 5:
                        Step5();
                        break;
                    case 6:
                        Step6();
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return Assign(Result);
        }

        /// <summary>
        /// Translates the results into a list of assignments in the matrix.
        /// </summary>
        /// <param name="results">The MatchResult created as a solution to the assignment problem by Munkre's Algorithm.</param>
        /// <returns>A list of the optimal assignments.</returns>
        private List<Assignment> Assign (MatchResult results)
        {
            var ret = new List<Assignment>();
            for (int i=0; i<results.Matches.Count(); i++)
            {
                for (int j=0; j<results.Matches[i].Count(); j++)
                {
                    if (results.Matches[i][j]==1)
                    {
                        ret.Add(new Assignment(Matrix.RowNames[i], Matrix.ColumnNames[j]));
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Simplify the matrix by subtracting the minimum of each column and row from its column or row.
        /// </summary>
        private void Step1 ()
        {
            //Subtract the minimum from each row.
            for (int row=0; row<Matrix.Data.Count(); row++)
            {
                double min = Matrix.Data[row].Min();
                for (int col=0; col<Matrix.Data[row].Count(); col++)
                {
                    Matrix.Data[row][col] -= min;
                }
            }

            //Subtract the minimum from each column.
            for (int col=0; col<Matrix.Data[0].Count(); col++)
            {
                var colmin = Matrix.Data.Select(t => t[col]).Min();//Min in col x
                for (int row=0; row<Matrix.Data.Count(); row++)
                {
                    Matrix.Data[row][col] -= colmin;
                }
            }

            Step = 2;
        }

        /// <summary>
        /// Find uncovered zeroes in the matrix, starring them and covering their rows and columns as they are found. 
        /// Once all elements have been checked, clear the covers and go to step 3.
        /// </summary>
        private void Step2 ()
        {
            for (int row=0; row<Matrix.Data.Count(); row++)
            {
                for (int col=0; col<Matrix.Data[0].Count(); col++)
                {
                    //Check that it is a zero and that the column and row are not covered.
                    if (Matrix.Data[row][col]==0 && Result.Rows[row]==0 && Result.Columns[col]==0)
                    {
                        //Star the zero
                        Result.Matches[row][col] = 1;
                        //Cover the row and column
                        Result.Rows[row] = 1;
                        Result.Columns[col] = 1;
                    }
                }
            }
            Result.ClearCovers();
            Step = 3;
        }

        /// <summary>
        /// Cover each column with a starred zero. If all columns are covered, the algorithm is done; otherwise, go to step 4.
        /// </summary>
        private void Step3 ()
        {
            //Get the columns of starred zeroes.
            for (int row=0; row<Matrix.Data.Count(); row++)
            {
                for (int col=0; col<Matrix.Data[0].Count(); col++)
                {
                    if (Result.Matches[row][col]==1)
                    {
                        Result.Columns[col] = 1;
                    }
                }
            }

            if (Result.Columns.Where(t=>t==1).Sum() >= Result.Columns.Count())
            {
                //If all columns are covered, the starred zeroes are a unique solution.
                Step = 7;
            } else
            {
                //Otherwise, continue trying to solve it.
                Step = 4;
            }

        }

        /// <summary>
        /// Prime an uncovered zero. If there are no starred zeroes in its row, go to step 5; otherwise, cover this row and uncover the starred zero's column.
        /// Repeat until there are no uncovered zeroes, then find the minimum uncovered value and go to step 6.
        /// </summary>
        private void Step4 ()
        {
            //Default values
            Tuple<int, int> rowcol = new Tuple<int, int>(-1, -1);
            bool done = false;

            while (!done)
            {
                rowcol = FindZero();
                if (rowcol.Item1==-1)
                {
                    done = true;
                    Step = 6;
                } else
                {
                    Result.Matches[rowcol.Item1][rowcol.Item2] = 2;
                    if (StarInRow(rowcol.Item1))
                    {
                        rowcol = new Tuple<int,int>(rowcol.Item1, FindStarInRow(rowcol.Item1));
                        Result.Rows[rowcol.Item1] = 1;
                        Result.Columns[rowcol.Item2] = 0;
                    } else
                    {
                        done = true;
                        Step = 5;
                        RowCol = rowcol;
                    }
                }
            }
        }

        /// <summary>
        /// Find the first uncovered zero for step 4.
        /// </summary>
        /// <returns>A Tuple with the row and column of the first uncovered zero found.</returns>
        private Tuple<int, int> FindZero ()
        {
            var ret = Matrix.Data.Select((value, row) => new { value = value.Select((v, col) => new { v, col })
                                                                            .Where(u=>Result.Columns[u.col]==0), row })
                                 .Where(t=>t.value.Any() && Result.Rows[t.row]==0)
                                 .Select(t=>new Tuple<int, int>(t.row, t.value.FirstOrDefault().col))
                                 .FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// Check if there are any ztarred zeroes in a row.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <returns>True if there is a starred zero, false otherwise.</returns>
        private bool StarInRow (int row)
        {
            return Result.Matches[row].Any(t=>t==1);
        }

        /// <summary>
        /// Finds the starred zero in a row.
        /// </summary>
        /// <param name="row">The row to search for the starred zero</param>
        /// <returns>The column for the starred zero in the row.</returns>
        private int FindStarInRow (int row)
        {
            var col = -1;
            for (int c=0; c<Matrix.Data[row].Count(); c++)
            {
                if (Result.Matches[row][c]==1)
                {
                    col = c;
                }
            }
            return col;
        }

        /// <summary>
        /// Construct a series of alternating primed and starred zeroes, starting with the uncovered zero found in step 4. 
        /// Then, find the starred zero in that zero's column, then the primed zero in the starred zero's row. 
        /// Repeat until there is a primed zero without a starred zero.
        /// Finally, convert this series into a path and clean up the results.
        /// </summary>
        private void Step5 ()
        {
            bool done = false;
            int r = -1;
            int c = -1;
            Path = new List<Tuple<int, int>>();
            Path.Add(RowCol);

            while (!done)
            {
                r = FindStarInCol(Path.Last().Item2);
                if (r!=-1)
                {
                    Path.Add(new Tuple<int, int>(r, Path.Last().Item2));
                } else
                {
                    done = true;
                }
                if (!done)
                {
                    c = FindPrimeInRow(Path.Last().Item1, Path.Last().Item2);
                    Path.Add(new Tuple<int,int>(Path.Last().Item1, c));
                }
            }

            AugmentPath();
            Result.ClearCovers();
            ErasePrimes();
            Step = 3;
        }

        /// <summary>
        /// Find a starred zero in a column.
        /// </summary>
        /// <param name="col">The column to search for a starred zero.</param>
        /// <returns>The row of the starred zero in the column.</returns>
        private int FindStarInCol (int col)
        {
            int row = -1;
            for (int r=0; r<Matrix.Data.Count(); r++)
            {
                if (Result.Matches[r][col]==1)
                {
                    row = r;
                }
            }

            return row;
        }

        /// <summary>
        /// Finds a primed zero in a row.
        /// </summary>
        /// <param name="row">The row to search for primed zeroes.</param>
        /// <param name="col">The default column to return.</param>
        /// <returns>The column of the primed zero if there is one; otherwise, returns the column passed in.</returns>
        private int FindPrimeInRow (int row, int col)
        {
            for (int c=0; c<Matrix.Data[row].Count(); c++)
            {
                if (Result.Matches[row][c] == 2)
                {
                    col = c;
                }
            }
            return col;
        }

        /// <summary>
        /// Converts the path created in Step 5 to a proper assignment matrix.
        /// </summary>
        private void AugmentPath ()
        {
            for (int i=0; i<Path.Count(); i++)
            {
                if (Result.Matches[Path[i].Item1][Path[i].Item2]==1)
                {
                    Result.Matches[Path[i].Item1][Path[i].Item2] = 0;
                } else
                {
                    Result.Matches[Path[i].Item1][Path[i].Item2] = 1;
                }
            }
        }

        /// <summary>
        /// Clear any primed zeroes from the matrix.
        /// </summary>
        private void ErasePrimes ()
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

        /// <summary>
        /// Update the matrix with the minimum found is step 4 by adding it to every covered row and subtracting it from every uncovered column.
        /// </summary>
        private void Step6 ()
        {
            double min = FindMin();
            for (int row=0; row<Result.Matches.Count(); row++)
            {
                for (int col=0; col<Result.Matches.Count(); col++)
                {
                    if (Result.Rows[row]==1)
                    {
                        Matrix.Data[row][col] += min;
                    }
                    if (Result.Columns[col]==0)
                    {
                        Matrix.Data[row][col] -= min;
                    }
                }
            }
            Step = 4;
        }

        /// <summary>
        /// Finds the minimum uncovered value in the matrix.
        /// </summary>
        /// <returns></returns>
        private double FindMin ()
        {
            double min = Matrix.Data.Select(t => t.Max()).Max();

            for (int row=0; row<Result.Matches.Count(); row++)
            {
                for (int col=0; col<Result.Matches.Count(); col++)
                {
                    if (Result.Rows[row]==0 && Result.Columns[col]==0 && min>Matrix.Data[row][col])
                    {
                        min = Matrix.Data[row][col];
                    }
                }
            }

            return min;
        }
    }
}
