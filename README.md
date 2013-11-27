Sudoku
======

Sudoku Solver (with plugable solvetechniques)

To create your "own" solvetechnique you just have to:

1.  derive from ASolveTechnique:

        public class OwnSolveTechnique : ASolveTechnique

2.  Implement the "SolveHouse" method:

		public override void SolveHouse(IHouse house, SudokuResult sudokuResult)


