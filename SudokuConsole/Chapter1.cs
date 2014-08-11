using System;

namespace DE.Onnen.Sudoku.SudokuConsole
{
	internal class Chapter1
	{
		/// <summary>
		/// Erstelle ein Sudoku und setze eine Zahl über die Methode SetDigit(int, int)
		/// </summary>
		/// <remarks>
		/// <code>
		/// IBoard board = new Board();
		/// board.SetDigit(0, 5);
		/// board.SetDigit(1, 7);
		/// board.SetDigit(2, 9);
		/// </code>
		/// output: <br/>
		///   123 456 789 <br/>
		///  ┌───┬───┬───┐ <br/>
		/// A│579│   │   │ <br/>
		/// B│   │   │   │ <br/>
		/// C│   │   │   │ <br/>
		///  ├───┼───┼───┤ <br/>
		/// D│   │   │   │ <br/>
		/// E│   │   │   │ <br/>
		/// F│   │   │   │ <br/>
		///  ├───┼───┼───┤ <br/>
		/// G│   │   │   │ <br/>
		/// H│   │   │   │ <br/>
		/// I│   │   │   │ <br/>
		///  └───┴───┴───┘ <br/>
		/// Complete: 11,1111111111111 %
		/// </remarks>
		public void Excample1()
		{
			IBoard board = new Board();
			// Setze ins Feld A1 den Wert 5.
			board.SetDigit(0, 5);
			board.SetDigit(1, 7);
			board.SetDigit(2, 9);
			Console.WriteLine(board.Matrix());
		}

		/// <summary>
		/// Setze die ersten 8 Digits -> 9 Digit wird automatisch gesetzt (Naked Single).
		/// </summary>
		/// <remarks>
		/// Im Feld [C3] (CellID: 29) verbleibt nur noch der Candidate 9 und wird gesetzt.
		/// </remarks>
		public void Excample2()
		{
			IBoard board = new Board();
			board.SetDigit(0, 1);
			board.SetDigit(1, 2);
			board.SetDigit(2, 3);
			board.SetDigit(9, 4);
			board.SetDigit(10, 5);
			board.SetDigit(11, 6);
			board.SetDigit(18, 7);
			board.SetDigit(19, 8);
			Console.WriteLine(board.Matrix());
		}
	}
}