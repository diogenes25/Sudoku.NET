using System;
using de.onnen.Sudoku.SudokuExternal;

namespace de.onnen.Sudoku
{
	public class SudokuHistoryItem
	{
		public int[] BoardInt { get; private set; }

		public int CellID { get; private set; }

		public int Digit { get; private set; }

		public SudokuResult SudokuResults { get; private set; }

		public double Percent { get; private set; }

		public SudokuHistoryItem(Board board, Cell cell, SudokuResult sudokuResult)
		{
			this.BoardInt = SudokuHelper.CreateSimpleBoard(board);
			this.CellID = cell.ID;
			this.Digit = cell.Digit;
			this.SudokuResults = sudokuResult;
			this.Percent = board.SolvePercent();
		}

		public override string ToString()
		{
			return String.Format("Cell({0}) [{1}{2}] {3} {4}%",
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