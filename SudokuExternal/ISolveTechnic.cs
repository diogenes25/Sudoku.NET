﻿namespace de.onnen.Sudoku.SudokuExternal.SolveTechnics
{
	public interface ISolveTechnic
	{
		SolveTechnicInfo Info { get; set; }

		ISudokuHost Host { get; set; }

		bool IsActive { get; }

		void Activate();

		void Deactivate();

		void SolveHouse(IHouse house, SudokuResult sudokuResult);
	}
}