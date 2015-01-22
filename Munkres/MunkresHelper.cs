using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCHMC.Core.AI.Models;

namespace CCHMC.Core.AI.Munkres
{
    public static class MunkresHelper
    {
        /// <summary>
        /// Translates the results into a list of assignments in the matrix.
        /// </summary>
        /// <param name="results">The MatchResult created as a solution to the assignment problem by Munkre's Algorithm.</param>
        /// <returns>A list of the optimal assignments.</returns>
        public static List<Assignment> Assign (this MunkresFSM munkres, MatchResult results)
        {
            var ret = new List<Assignment>();
            for (int i=0; i<results.Matches.Count(); i++)
            {
                for (int j=0; j<results.Matches[i].Count(); j++)
                {
                    if (results.Matches[i][j]==1)
                    {
                        ret.Add(new Assignment(munkres.Matrix.RowNames[i], munkres.Matrix.ColumnNames[j]));
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Find the first uncovered zero for step 4.
        /// </summary>
        /// <returns>A Tuple with the row and column of the first uncovered zero found.</returns>
        public static Tuple<int, int> FindZero (this MunkresFSM munkres)
        {
            var ret = munkres.Matrix.Data.Select((value, row) => new { value = value.Select((v, col) => new { v, col })
                                                                            .Where(u=>munkres.Result.Columns[u.col]==0), row })
                                 .Where(t=>t.value.Any() && munkres.Result.Rows[t.row]==0)
                                 .Select(t=>new Tuple<int, int>(t.row, t.value.FirstOrDefault().col))
                                 .FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// Check if there are any ztarred zeroes in a row.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <returns>True if there is a starred zero, false otherwise.</returns>
        public static bool StarInRow (this MunkresFSM munkres, int row)
        {
            return munkres.Result.Matches[row].Any(t=>t==1);
        }

        /// <summary>
        /// Finds the starred zero in a row.
        /// </summary>
        /// <param name="row">The row to search for the starred zero</param>
        /// <returns>The column for the starred zero in the row.</returns>
        public static int FindStarInRow (this MunkresFSM munkres, int row)
        {
            var col = -1;
            for (int c=0; c<munkres.Matrix.Data[row].Count(); c++)
            {
                if (munkres.Result.Matches[row][c]==1)
                {
                    col = c;
                }
            }
            return col;
        }

        /// <summary>
        /// Find a starred zero in a column.
        /// </summary>
        /// <param name="col">The column to search for a starred zero.</param>
        /// <returns>The row of the starred zero in the column.</returns>
        public static int FindStarInCol (this MunkresFSM munkres, int col)
        {
            int row = -1;
            for (int r=0; r<munkres.Matrix.Data.Count(); r++)
            {
                if (munkres.Result.Matches[r][col]==1)
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
        public static int FindPrimeInRow (this MunkresFSM munkres, int row, int col)
        {
            for (int c=0; c<munkres.Matrix.Data[row].Count(); c++)
            {
                if (munkres.Result.Matches[row][c] == 2)
                {
                    col = c;
                }
            }
            return col;
        }
        
        /// <summary>
        /// Finds the minimum uncovered value in the matrix.
        /// </summary>
        /// <returns></returns>
        public static double FindMin (this MunkresFSM munkres)
        {
            double min = munkres.Matrix.Data.Select(t => t.Max()).Max();

            for (int row=0; row<munkres.Result.Matches.Count(); row++)
            {
                for (int col=0; col<munkres.Result.Matches.Count(); col++)
                {
                    if (munkres.Result.Rows[row]==0 && munkres.Result.Columns[col]==0 && min>munkres.Matrix.Data[row][col])
                    {
                        min = munkres.Matrix.Data[row][col];
                    }
                }
            }

            return min;
        }
    }
}
