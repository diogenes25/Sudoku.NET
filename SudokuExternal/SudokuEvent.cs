using System;

namespace de.onnen.Sudoku.SudokuExternal
{
	public delegate void CellEventHandler(SudokuEvent eventInfo);

	public class SudokuEvent
	{
		public ICellBase ChangedCellBase { get; set; }

		public CellAction Action { get; set; }

		public int value = 0;
		public String SolveTechnik;

		public override string ToString()
		{
			return String.Format("{0}, A:{1}, Val:{2}, T:{3}", ChangedCellBase, Action, value, SolveTechnik);
		}
	}

	public enum CellAction
	{
		SetDigitInt = 1,
		RemPoss = 2,
	}
}