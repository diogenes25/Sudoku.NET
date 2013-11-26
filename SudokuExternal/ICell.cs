namespace de.onnen.Sudoku.SudokuExternal
{
	public interface ICell : ICellBase
	{
		int BaseValue { get; }

		System.Collections.Generic.List<int> Candidates { get; }

		event de.onnen.Sudoku.SudokuExternal.CellEventHandler CellEvent;

		int Digit { get; }

		bool SetDigit(int digit, SudokuResult sudokuResult);

		bool RemovePossibleDigit(int p, SudokuResult child);
	}
}