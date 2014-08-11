using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DE.Onnen.Sudoku
{
	/// <summary>
	/// Smallest element in a sudoku grid, capable of containing a single digit.
	/// </summary>
	/// <remarks>
	/// A cell is always a member of a single row, a single column and a single box.<br />
	/// There are 81 cells in a standard sudoku grid.
	/// </remarks>
	public interface ICell : ICellBase, INotifyPropertyChanged
	{
		/// <summary>
		/// A list of every candidate.
		/// </summary>
		/// <remarks>
		/// @see BaseValue
		/// </remarks>
		ReadOnlyCollection<int> Candidates { get; }

		/// <summary>
		/// A numerical value between 1 and 9, which must be placed in the cells in order to complete the puzzle.
		/// </summary>
		/// <remarks>
		/// For each digit, there must be 9 instances in the solution to satisfy all constraints.
		/// </remarks>
		int Digit { get; }

		/// <summary>
		/// Set the digit and removes candidates in nested Houses (col, row and box).
		/// </summary>
		/// <param name="digit">Digit to cell.</param>
		/// <returns>Log with every action that was done regarding this action.</returns>
		SudokuLog SetDigit(int digit);

		/// <summary>
		/// Removing a candidate from the grid, by means of logical deduction.
		/// </summary>
		/// <remarks>
		/// Most advanced solving techniques result in one or more eliminations.
		/// </remarks>
		/// <param name="candidate">Candidate to be removed</param>
		/// <param name="child"></param>
		/// <returns>true = The candidate was succesful removed. false = candidate was no in the cell.</returns>
		bool RemoveCandidate(int candidateToRemove, SudokuLog sudokuResult);
	}
}