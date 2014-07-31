using System.ComponentModel;

namespace de.onnen.Sudoku.SudokuExternal
{
    /// <summary>
    /// Smallest element in a sudoku grid, capable of containing a single digit.
    /// </summary>
    /// <remarks>
    /// A cell is always a member of a single row, a single column and a single box. <br />
    /// There are 81 cells in a standard sudoku grid.
    /// </remarks>
    public interface ICell : ICellBase, INotifyPropertyChanged
    {
        /// <summary>
        /// A list of every candidate.
        /// <remarks>
        /// @see BaseValue
        /// </remarks>
        /// </summary>
        System.Collections.Generic.List<int> Candidates { get; }

        /// <summary>
        /// A numerical value between 1 and 9, which must be placed in the cells in order to complete the puzzle.
        /// <remarks>
        /// For each digit, there must be 9 instances in the solution to satisfy all constraints.
        /// </remarks>
        /// </summary>
        int Digit { get; }

        SudokuLog SetDigit(int digit);

        //bool SetDigit(int digit, SudokuLog sudokuResult);

        /// <summary>
        /// Removing a candidate from the grid, by means of logical deduction.
        /// </summary>
        /// <remarks>
        /// Most advanced solving techniques result in one or more eliminations.
        /// </remarks>
        /// <param name="candidate">Candidate to be removed</param>
        /// <param name="child"></param>
        /// <returns></returns>
        bool RemovePossibleDigit(int candidate, SudokuLog child);
    }
}