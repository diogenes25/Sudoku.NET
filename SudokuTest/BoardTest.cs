using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DE.Onnen.Sudoku.SolveTechniques;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DE.Onnen.Sudoku
{
    /// <summary>
    ///This is a test class for BoardTest and is intended
    ///to contain all BoardTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BoardTest
    {
        private TestContext testContextInstance;
        private static IList<ASolveTechnique> solveTechniques;

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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            solveTechniques = GetSolveTechniques();
        }

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

        private static IList<ASolveTechnique> GetSolveTechniques()
        {
            IList<ASolveTechnique> st = new List<ASolveTechnique>();
            st.Add(new DE.Onnen.Sudoku.SolveTechniques.HiddenPairTripleQuad());
            st.Add(new DE.Onnen.Sudoku.SolveTechniques.LockedCandidates());
            st.Add(new DE.Onnen.Sudoku.SolveTechniques.NakedPairTrippleQuad());
            return st;
        }

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigit_with_coordinates()
        {
            Board target = new Board(solveTechniques);
            int row = 8;
            int col = 8;
            int digit = 9;
            Assert.AreEqual(0, target[80].Digit);
            target.SetDigit(row, col, digit);
            Assert.AreEqual(9, target[80].Digit);
            Assert.AreEqual(0, target[80].CandidateValue);
        }

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigitTest2()
        {
            Board board = new Board(solveTechniques);
            board.SetDigit(0, 1);
            board.SetDigit(1, 2);
            board.SetDigit(2, 3);

            board.SetDigit(9, 4);
            board.SetDigit(10, 5);
            board.SetDigit(11, 6);

            board.SetDigit(18, 7);
            Assert.AreEqual(0, board[20].Digit);
            Assert.AreEqual(((1 << 7) + (1 << 8)), board[20].CandidateValue);

            // Now Last
            board.SetDigit(19, 8);

            Assert.AreEqual(9, board[20].Digit);
            Assert.AreEqual(0, board[20].CandidateValue);
        }

        /// <summary>
        ///A test for SetDigit
        ///</summary>
        [TestMethod()]
        public void SetDigitTest3()
        {
            Board target = new Board(solveTechniques);
            for (int i = 0; i < 8; i++)
            {
                target.SetDigit(i, i + 1);
                if (i == 0)
                    continue;
                target.SetDigit((i * 9), 9 - i);
            }
            Assert.AreEqual(9, target[8].Digit);
            Assert.AreEqual(0, target[8].CandidateValue);
            Assert.AreEqual(9, target[72].Digit);
            Assert.AreEqual(0, target[72].CandidateValue);
        }

        [TestMethod()]
        public void SetDigitTest4()
        {
            Board board = new Board(solveTechniques);
            board.SetDigit(0, 0, 1);
            board.SetDigit(0, 1, 2);
            board.SetDigit(0, 2, 3);
            board.SetDigit(1, 0, 4);
            board.SetDigit(1, 1, 5);
            //board.SetDigit(1, 2, 6);
            board.SetDigit(1, 3, 7);
            board.SetDigit(1, 4, 8);
            SudokuLog result = board.SetDigit(1, 5, 9);
            Assert.AreEqual(6, board[11].Digit);
            int block0Value = (1 << 6) | (1 << 7) | (1 << 8);
            Assert.AreEqual(block0Value, board[18].CandidateValue);
            Assert.AreEqual(block0Value, board[19].CandidateValue);
            Assert.AreEqual(block0Value, board[20].CandidateValue);
            int block1r2Value = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5);
            Assert.AreEqual(block1r2Value, board[21].CandidateValue);
            Assert.AreEqual(block1r2Value, board[22].CandidateValue);
            Assert.AreEqual(block1r2Value, board[23].CandidateValue);
            board.Solve(result);
            int block1r2ValueSolve = (1 << 0) | (1 << 1) | (1 << 2);
            Assert.AreEqual(block1r2ValueSolve, board[21].CandidateValue);
            Assert.AreEqual(block1r2ValueSolve, board[22].CandidateValue);
            Assert.AreEqual(block1r2ValueSolve, board[23].CandidateValue);
        }

        [TestMethod()]
        public void SetDigitTest5()
        {
            Board board = new Board(solveTechniques);
            board.SetDigit(1, 4, 1);
            board.SetDigit(0, 6, 2);
            board.SetDigit(0, 7, 3);
            SudokuLog result = board.SetDigit(0, 8, 4);
            board.Solve(result);
            int block1r2Value = (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8);
            Assert.AreEqual(block1r2Value, board[18].CandidateValue);
            Assert.AreEqual(block1r2Value, board[19].CandidateValue);
            Assert.AreEqual(block1r2Value, board[20].CandidateValue);
        }

        [TestMethod()]
        public void SetDigitTest6()
        {
            Board board = new Board(solveTechniques);
            board.SetDigit(4, 1, 1);
            board.SetDigit(6, 0, 2);
            board.SetDigit(7, 0, 3);
            SudokuLog result = board.SetDigit(8, 0, 4);
            board.Solve(result);
            int block1r2Value = (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8);
            Assert.AreEqual(block1r2Value, board[2].CandidateValue);
            Assert.AreEqual(block1r2Value, board[11].CandidateValue);
            Assert.AreEqual(block1r2Value, board[20].CandidateValue);
        }

        [TestMethod()]
        public void Solve_Sudoku_solved_with_3_solvetechniques_and_without_backtracking()
        {
            Board board = new Board(solveTechniques);
            string simpleSudoku = @"030050040
008010500
460000012
070502080
000603000
040109030
250000098
001020600
080060020";

            string[] lines = simpleSudoku.Split('\n');
            for (int y = 0; y < 9; y++)
            {
                string line = lines[y];

                for (int x = 0; x < 9; x++)
                {
                    char currChar = line[x];
                    if (currChar.Equals('0'))
                        continue;
                    SudokuLog result = board.SetDigit(y, x, Convert.ToInt32(currChar) - 48);
                }
            }
            SudokuLog sudokuResult = new SudokuLog();
            Assert.IsFalse(board.IsComplete);
            Assert.IsTrue(sudokuResult.Successful);
            board.Solve(sudokuResult);
            Assert.IsTrue(board.IsComplete);
            Assert.IsTrue(sudokuResult.Successful);
        }

        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod()]
        public void SolveTest()
        {
            Board board = new Board(solveTechniques);
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
            Assert.AreEqual(2, board[80].Digit);
        }

        /// <summary>
        ///A test for Board Constructor
        ///</summary>
        [TestMethod()]
        public void BoardConstructor_whith_null_techniques_cells_must_be_set()
        {
            IList<ASolveTechnique> solveTechniques = null; // TODO: Initialize to an appropriate value
            Board target = new Board(solveTechniques);
            CheckBoard(target);
        }

        public void BoardConstructor_whith_techniques_cells_must_be_set()
        {
            Board target = new Board(solveTechniques);
            CheckBoard(target);
        }

        private static void CheckBoard(Board target)
        {
            Assert.AreEqual(Consts.DimensionSquare * Consts.DimensionSquare, target.Count);
            foreach (ICell cell in target)
            {
                Assert.AreEqual(Consts.BaseStart, cell.CandidateValue);
                Assert.AreEqual(0, cell.Digit);
            }
        }

        /// <summary>
        ///A test for Board Constructor
        ///</summary>
        [TestMethod()]
        public void Constructor_empty_cells_must_be_set()
        {
            Board target = new Board();
            CheckBoard(target);
        }

        /// <summary>
        ///A test for Backtracking
        ///</summary>
        [TestMethod()]
        public void Backtracking_solve_without_any_digit()
        {
            Board target = new Board();
            SudokuLog log = new SudokuLog();
            target.Backtracking(log);
            Assert.IsTrue(log.Successful);
            for (int i = 0; i < Consts.DimensionSquare; i++)
            {
                Assert.AreEqual((i + 1), target[i].Digit);
            }
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void Clear_not_digit_is_set()
        {
            Board target = new Board();
            for (int i = 0; i < Consts.DimensionSquare; i++)
            {
                target.SetDigit(i, i, i + 1);
                Assert.AreEqual(i + 1, target[i + (i * Consts.DimensionSquare)].Digit);
            }
            target.Clear();
            for (int i = 0; i < Consts.DimensionSquare; i++)
            {
                Assert.AreEqual(0, target[i].Digit);
                Assert.AreEqual(0, target[i + (i * Consts.DimensionSquare)].Digit);
                Assert.AreEqual(0, target[Consts.DimensionSquare - (i + (Consts.DimensionSquare - (i * Consts.DimensionSquare)))].Digit);
            }
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        public void Clone_Digit_and_Candidates_are_equal_to_clone()
        {
            Board target = new Board();
            Board expected = new Board();
            for (int i = 0; i < Consts.DimensionSquare; i++)
            {
                target.SetDigit(i, i, i + 1);
                expected.SetDigit(i, i, i + 1);
            }
            object actual;
            actual = target.Clone();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateSimpleBoard
        ///</summary>
        [TestMethod()]
        public void CreateSimpleBoard_creates_int_with()
        {
            IBoard board = new Board();
            for (int i = 0; i < Consts.DimensionSquare; i++)
            {
                board.SetDigit(i, i + 1);
            }
            int[] expected = new int[Consts.DimensionSquare * Consts.DimensionSquare] { -1, -2, -3, -4, -5, -6, -7, -8, -9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] actual;
            actual = Board.CreateSimpleBoard(board);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            IEnumerator<ICell> expected = null; // TODO: Initialize to an appropriate value
            IEnumerator<ICell> actual;
            actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetHouse
        ///</summary>
        [TestMethod()]
        public void GetHouseTest()
        {
            IBoard target = new Board(); // TODO: Initialize to an appropriate value
            HouseType houseType = new HouseType(); // TODO: Initialize to an appropriate value
            int idx = 0; // TODO: Initialize to an appropriate value
            IHouse expected = null; // TODO: Initialize to an appropriate value
            IHouse actual;
            actual = target.GetHouse(houseType, idx);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetBoard
        ///</summary>
        [TestMethod()]
        public void SetBoardTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            IBoard otherBoard = null; // TODO: Initialize to an appropriate value
            target.SetBoard(otherBoard);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetHistory
        ///</summary>
        [TestMethod()]
        public void SetHistoryTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            int historyId = 0; // TODO: Initialize to an appropriate value
            target.SetHistory(historyId);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod()]
        public void SolveTest1()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            SudokuLog sudokuResult = null; // TODO: Initialize to an appropriate value
            target.Solve(sudokuResult);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for System.Collections.IEnumerable.GetEnumerator
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Sudoku.dll")]
        public void GetEnumeratorTest1()
        {
            IEnumerable target = new Board(); // TODO: Initialize to an appropriate value
            IEnumerator expected = null; // TODO: Initialize to an appropriate value
            IEnumerator actual;
            actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToString_is_string_with_every_digit()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            string expected = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        public void Count_is_length_of_cells()
        {
            Board target = new Board();
            int actual;
            actual = target.Count;
            Assert.AreEqual(Consts.DimensionSquare * Consts.DimensionSquare, actual);
        }

        /// <summary>
        ///A test for Givens
        ///</summary>
        [TestMethod()]
        public void GivensTest()
        {
            Board target = new Board();
            ReadOnlyCollection<ICell> actual;
            actual = target.Givens;
            Assert.AreEqual(0, actual.Count);
        }

        /// <summary>
        ///A test for History
        ///</summary>
        [TestMethod()]
        public void HistoryTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            ReadOnlyCollection<SudokuHistoryItem> actual;
            actual = target.History;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsComplete
        ///</summary>
        [TestMethod()]
        public void IsCompleteTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsComplete;
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        [TestMethod()]
        public void IsReadOnlyTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsReadOnly;
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            ICell actual;
            actual = target[index];
            Assert.AreEqual(0, actual.ID);
        }

        /// <summary>
        ///A test for SolvePercent
        ///</summary>
        [TestMethod()]
        public void SolvePercentTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            double actual;
            actual = target.SolvePercent;
            Assert.AreEqual(0.0, actual);
        }

        /// <summary>
        ///A test for SolveTechniques
        ///</summary>
        [TestMethod()]
        public void SolveTechniquesTest()
        {
            Board target = new Board(); // TODO: Initialize to an appropriate value
            IList<ISolveTechnique> actual;
            actual = target.SolveTechniques;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}