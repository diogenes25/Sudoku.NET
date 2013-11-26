using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;

namespace Sudoku
{
    [TestClass]
    public class Wenn_ich_einen_Wert_setze
    {
        [TestMethod]
        public void Muss_dieser_Wert_als_Moeglichkeit_aus_der_Reihe_geloescht_Werden()
        {
            int baseValue = (1 << Board.DimesionSquare) - 1;
            Board board = new Board();
            board.SetDigit(0, 1);
            int expected = baseValue - (1 << 0);
            for (int c = 0; c < 1; c++)
            {
                for (int i = 1; i < 9; i++)
                {
                    Assert.AreEqual(expected, board.Cells[i].BaseValue);
                }
            }
            board.SetDigit(1, 1);
            board.SetDigit(2, 1);
            board.SetDigit(9, 1);
            board.SetDigit(10, 1);
            board.SetDigit(11, 1);
            board.SetDigit(18, 1);
            board.Solve();
            board.SetDigit(19, 1);

        }
    }
}
