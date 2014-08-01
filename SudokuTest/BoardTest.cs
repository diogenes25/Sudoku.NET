using de.onnen.Sudoku;
using de.onnen.Sudoku.SudokuExternal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sudoku
{
    /// <summary>
    ///This is a test class for BoardTest and is intended
    ///to contain all BoardTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BoardTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigitTest()
        {
            Board target = new Board();
            int digit = 1;
            int cell = 0;
            target.SetDigit(cell, digit);
            Assert.AreEqual(1, target.Cells[0].Digit);
            Assert.AreEqual(0, target.Cells[0].CandidateValue);

            int baseValue = (1 << Consts.DimensionSquare) - 1;
            int expected = baseValue - (1 << 0);
            for (int c = 0; c < 2; c++)
            {
                for (int i = 1; i < 9; i++)
                {
                    if (c == 1)
                    {
                        Assert.AreEqual(0, target.Cells[i * 9].Digit);
                        Assert.AreEqual(expected, target.Cells[i * 9].CandidateValue);
                    }
                    else
                    {
                        Assert.AreEqual(0, target.Cells[i].Digit);
                        Assert.AreEqual(expected, target.Cells[i].CandidateValue);
                    }
                }
            }

            int containerIdx = 0;
            for (int zr = 0; zr < Consts.Dimension; zr++)
            {
                for (int zc = 0; zc < Consts.Dimension; zc++)
                {
                    int b = (containerIdx * Consts.Dimension) + (zc + (zr * Consts.DimensionSquare)) + ((containerIdx / Consts.Dimension) * Consts.DimensionSquare * 2);
                    if (b == 0)
                        continue;
                    Assert.AreEqual(0, target.Cells[b].Digit);
                    Assert.AreEqual(expected, target.Cells[b].CandidateValue);
                }
            }
        }

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigitTest1()
        {
            Board target = new Board();
            int row = 8;
            int col = 8;
            int digit = 9;
            target.SetDigit(row, col, digit);
            Assert.AreEqual(9, target.Cells[80].Digit);
            Assert.AreEqual(0, target.Cells[80].CandidateValue);
        }

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigitTest2()
        {
            Board board = new Board(); // TODO: Initialize to an appropriate value
            board.SetDigit(0, 1);
            board.SetDigit(1, 2);
            board.SetDigit(2, 3);

            board.SetDigit(9, 4);
            board.SetDigit(10, 5);
            board.SetDigit(11, 6);

            board.SetDigit(18, 7);
            Assert.AreEqual(0, board.Cells[20].Digit);
            Assert.AreEqual(((1 << 7) + (1 << 8)), board.Cells[20].CandidateValue);

            // Now Last
            board.SetDigit(19, 8);

            Assert.AreEqual(9, board.Cells[20].Digit);
            Assert.AreEqual(0, board.Cells[20].CandidateValue);
        }

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigitTest3()
        {
            Board target = new Board();
            for (int i = 0; i < 8; i++)
            {
                target.SetDigit(i, i + 1);
                if (i == 0)
                    continue;
                target.SetDigit((i * 9), 9 - i);
            }
            Assert.AreEqual(9, target.Cells[8].Digit);
            Assert.AreEqual(0, target.Cells[8].CandidateValue);
            Assert.AreEqual(9, target.Cells[72].Digit);
            Assert.AreEqual(0, target.Cells[72].CandidateValue);
        }

        [TestMethod()]
        public void SetDigitTest4()
        {
            Board board = new Board();
            board.SetDigit(0, 0, 1);
            board.SetDigit(0, 1, 2);
            board.SetDigit(0, 2, 3);
            board.SetDigit(1, 0, 4);
            board.SetDigit(1, 1, 5);
            //board.SetDigit(1, 2, 6);
            board.SetDigit(1, 3, 7);
            board.SetDigit(1, 4, 8);
            SudokuLog result = board.SetDigit(1, 5, 9);
            Assert.AreEqual(6, board.Cells[11].Digit);
            int block0Value = (1 << 6) | (1 << 7) | (1 << 8);
            Assert.AreEqual(block0Value, board.Cells[18].CandidateValue);
            Assert.AreEqual(block0Value, board.Cells[19].CandidateValue);
            Assert.AreEqual(block0Value, board.Cells[20].CandidateValue);
            int block1r2Value = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5);
            Assert.AreEqual(block1r2Value, board.Cells[21].CandidateValue);
            Assert.AreEqual(block1r2Value, board.Cells[22].CandidateValue);
            Assert.AreEqual(block1r2Value, board.Cells[23].CandidateValue);
            board.Solve(result);
            int block1r2ValueSolve = (1 << 0) | (1 << 1) | (1 << 2);
            Assert.AreEqual(block1r2ValueSolve, board.Cells[21].CandidateValue);
            Assert.AreEqual(block1r2ValueSolve, board.Cells[22].CandidateValue);
            Assert.AreEqual(block1r2ValueSolve, board.Cells[23].CandidateValue);
        }

        [TestMethod()]
        public void SetDigitTest5()
        {
            Board board = new Board();
            board.SetDigit(1, 4, 1);
            board.SetDigit(0, 6, 2);
            board.SetDigit(0, 7, 3);
            SudokuLog result = board.SetDigit(0, 8, 4);
            board.Solve(result);
            int block1r2Value = (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8);
            Assert.AreEqual(block1r2Value, board.Cells[18].CandidateValue);
            Assert.AreEqual(block1r2Value, board.Cells[19].CandidateValue);
            Assert.AreEqual(block1r2Value, board.Cells[20].CandidateValue);
        }

        [TestMethod()]
        public void SetDigitTest6()
        {
            Board board = new Board();
            board.SetDigit(4, 1, 1);
            board.SetDigit(6, 0, 2);
            board.SetDigit(7, 0, 3);
            SudokuLog result = board.SetDigit(8, 0, 4);
            board.Solve(result);
            int block1r2Value = (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8);
            Assert.AreEqual(block1r2Value, board.Cells[2].CandidateValue);
            Assert.AreEqual(block1r2Value, board.Cells[11].CandidateValue);
            Assert.AreEqual(block1r2Value, board.Cells[20].CandidateValue);
        }

        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod()]
        public void SolveTest()
        {
            Board board = new Board();
            board.SetDigit(1, 0, 2);
            board.SetDigit(1, 2, 3);
            board.SetDigit(1, 3, 6);
            board.SetDigit(1, 7, 9);
            board.SetDigit(2, 0, 9);
            board.SetDigit(2, 1, 5);
            board.SetDigit(2, 5, 3);
            board.SetDigit(2, 7, 2);
            board.SetDigit(3, 3, 5);
            board.SetDigit(3, 6, 3);
            board.SetDigit(4, 0, 7);
            board.SetDigit(4, 3, 3);
            board.SetDigit(4, 4, 8);
            board.SetDigit(4, 5, 6);
            board.SetDigit(4, 8, 1);
            board.SetDigit(5, 2, 6);
            board.SetDigit(5, 5, 7);
            board.SetDigit(6, 1, 7);
            board.SetDigit(6, 3, 1);
            board.SetDigit(6, 7, 3);
            board.SetDigit(6, 8, 4);
            board.SetDigit(7, 1, 4);
            board.SetDigit(7, 5, 9);
            board.SetDigit(7, 6, 5);
            board.SetDigit(7, 8, 7);

            SudokuLog result = board.SetDigit(0, 0, 6);
            board.Solve(result);
            Assert.AreEqual(2, board.Cells[80].Digit);
        }
    }
}