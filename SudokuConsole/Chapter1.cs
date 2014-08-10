using System;

namespace DE.Onnen.Sudoku.SudokuConsole
{
    public class Chapter1
    {
        /// <summary>
        /// Erstelle ein Sudoku und setze eine Zahl über die Methode SetDigit(int, int)
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        public void Excample1()
        {
            IBoard board = new Board();
            // Setze ins Feld A1 den Wert 5.
            board.SetDigit(0, 5);
            Console.WriteLine(board.Matrix());
        }

        /// <summary>
        ///
        /// </summary>
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