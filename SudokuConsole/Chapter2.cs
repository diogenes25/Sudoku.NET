using DE.Onnen.Sudoku.SolveTechniques;
using System;
using System.Collections.Generic;

namespace DE.Onnen.Sudoku.SudokuConsole
{
	internal class Chapter2
	{
		/// <summary>
		/// Board um weitere Solvetechniques NakedPairTrippleQuad erweitert.
		/// </summary>
		/// <remarks>
		/// Setze folgendes Sudoku
		/// 123000000
		/// 456000000
		/// 700000000
		/// 000000000
		/// 000000000
		/// 000000000
		/// 000000000
		/// 000000000
		/// 000000000
		/// </remarks>
		public void Excample1()
		{
			IList<ASolveTechnique> st = new List<ASolveTechnique>();
			st.Add(new DE.Onnen.Sudoku.SolveTechniques.NakedPairTrippleQuad());
			IBoard board = new Board(st);
			// Extension-Method SetCellsFromString(string)
			board.SetCellsFromString("123000000456000000700000000000000000000000000000000000000000000000000000000000000");
			// 8,9 sind in der Row[2] komplett gesetzt, obwohl diese beiden Digit nur in den Cellen 28 und 29 sein können.
			Console.WriteLine(board.MatrixWithCandidates());
			// 8,9 sind in jetzt aus Cell[30] bis Cell[35].
			board.Solve(new SudokuLog());
			Console.WriteLine(board.MatrixWithCandidates());
		}
	}
}