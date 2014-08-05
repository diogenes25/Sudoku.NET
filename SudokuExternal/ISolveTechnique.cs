namespace DE.Onnen.Sudoku.SudokuExternal.SolveTechniques
{
	public interface ISolveTechnique
	{
		SolveTechniqueInfo Info { get; set; }

		ISudokuHost Host { get; set; }

		bool IsActive { get; }

		void Activate();

		void Deactivate();

		void SolveHouse(IHouse house, SudokuLog sudokuResult);

		ECellView CellView { get; }
	}
}