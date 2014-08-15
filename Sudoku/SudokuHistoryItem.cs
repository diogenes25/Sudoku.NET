using System;
using System.Globalization;

namespace DE.Onnen.Sudoku
{
	/// <summary>
	/// A clone of the current board including some extra infos.
	/// </summary>
	public class SudokuHistoryItem
	{
		/// <summary>
		/// Integer represenning the board.
		/// </summary>
		public int[] BoardInt { get; private set; }

		/// <summary>
		/// CellID of the cell that was set before this HistoryItem was created.
		/// </summary>
		public int CellID { get; private set; }

		/// <summary>
		/// Digt that was set before this HistoryItem was created.
		/// </summary>
		public int Digit { get; private set; }

		/// <summary>
		/// Last result.
		/// </summary>
		public SudokuLog SudokuResults { get; private set; }

		/// <summary>
		/// Percent.
		/// </summary>
		public double Percent { get; private set; }

		/// <summary>
		/// Create a HistoryItem of the Board.
		/// </summary>
		/// <remarks>Create a small clone of the board.</remarks>
		/// <param name="board">Current board</param>
		/// <param name="cell">Last changed Cell</param>
		/// <param name="sudokuResult"></param>
		public SudokuHistoryItem(IBoard board, ICell cell, SudokuLog sudokuResult)
		{
			this.BoardInt = Board.CreateSimpleBoard(board);
			if (cell == null)
			{
				this.CellID = -1;
				this.Digit = -1;
			}
			else
			{
				this.CellID = cell.ID;
				this.Digit = cell.Digit;
			}
			this.SudokuResults = sudokuResult;
			if (board == null)
			{
				this.Percent = 0;
			}
			else
			{
				this.Percent = board.SolvePercent;
			}
		}

		/// <summary>
		/// Infos.
		/// </summary>
		/// <returns>String</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "Cell({0}) [{1}{2}] {3} {4}%",
									this.CellID,
									((char)(int)((this.CellID / Consts.DimensionSquare) + 65)),
									((this.CellID % Consts.DimensionSquare) + 1),
									this.Digit,
									string.Format("{0:0.00}", this.Percent)
							);
			//return "Cell(" + this.CellID + ") [" + ((char)(int)((this.CellID / Consts.DimensionSquare) + 65)) + "" + ((this.CellID % Consts.DimensionSquare) + 1) + "] " + this.Digit + " " + this.Percent + "%";
		}
	}
}