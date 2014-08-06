Sudoku.NET
==========

Sudoku Solver (with plugable solvetechniques).
Die Core-Komponente (Sudoku) enthält nur eine einzige rudimentären Lösungsstrategie ([Naked Single](http://sudoku-solutions.com/solvingNakedSubsets.php#nakedSingle "Naked Single")), der mit jedem Setzen einer Zahl ausgeführt wird und eine Backtracking-Funktion [Backtracking](http://en.wikipedia.org/wiki/Backtracking "Backtracking").
Es können weitere Lösungsstrategien implementiert werden, die entweder sofort zur Lösung des Sudokus führt oder (min.) die Schritte beim Backtracking reduziert.

Enthaltene Projekte
-------------------

Die Solution enthält mehrere Projekte.
* Sudoku
	* Kernkomponente.
* SudokuExternal
	* Öffentliche Interfaces und Klassen.
* SudokuTest
	* UnitTests
* SolveTechniqueHiddenPTQ
	* [Hidden X](http://sudoku-solutions.com/solvingHiddenSubsets.php "Hidden Single/Pair/Tripple/Quad")
* SolveTechniqueLockedCandidates
	* [Locked](http://sudoku-solutions.com/solvingInteractions.php "Locked Candidates (Pointing/Claiming)")
* SolveTechniqueNakedPTQ
	* [Naked X](http://sudoku-solutions.com/solvingNakedSubsets.php "Naked Single/Pair/Tripple/Quad")
* SudokuDocumentation
	* [Sandcastle-Projekt](https://shfb.codeplex.com/ "Sandcastle Help File Builder")

To create your "own" solvetechnique you just have to:

1.  derive from ASolveTechnique:

        public class OwnSolveTechnique : ASolveTechnique

2.  Implement the "SolveHouse" method:

		public override void SolveHouse(IHouse house, SudokuResult sudokuResult)


