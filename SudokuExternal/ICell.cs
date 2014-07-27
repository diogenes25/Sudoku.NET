using System.ComponentModel;

namespace de.onnen.Sudoku.SudokuExternal
{
    /// <summary>
    /// Smallest element in a sudoku grid, capable of containing a single digit.
    /// <remarks>
    /// A cell is always a member of a single row, a single column and a single box. <br />
    /// There are 81 cells in a standard sudoku grid.
    /// </remarks>
    /// </summary>
    public interface ICell : ICellBase, INotifyPropertyChanged
    {
        /// <summary>
        /// A bitmask of every possible solution.
        /// <remarks>
        /// A possible solution for an unsolved cell.
        /// Each candidate represents a digit.
        /// Solving a sudoku puzzle is mainly done by elimination of candidates.
        /// When a cell contains a digit, the remaining values are no longer considered candidates for that cell.
        /// In addition, all peers of that cell lose their candidates for that digit, because each house can only contain one instance of each digit.
        /// @see RemovePossibleDigit(int candidate, SudokuResult child)
        /// </remarks>
        /// </summary>
        int BaseValue { get; }

        /// <summary>
        /// A list of every possible solution.
        /// <remarks>
        /// @see BaseValue
        /// </remarks>
        /// </summary>
        System.Collections.Generic.List<int> Candidates { get; }

        event de.onnen.Sudoku.SudokuExternal.CellEventHandler CellEvent;

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
        /// <remarks>
        /// Most advanced solving techniques result in one or more eliminations.
        /// </remarks>
        /// </summary>
        /// <param name="candidate">Candidate to be removed</param>
        /// <param name="child"></param>
        /// <returns></returns>
        bool RemovePossibleDigit(int candidate, SudokuLog child);
    }
}