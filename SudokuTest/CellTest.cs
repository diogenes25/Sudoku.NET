using System;
using System.Collections.Generic;
using de.onnen.Sudoku;
using de.onnen.Sudoku.SudokuExternal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sudoku
{
    /// <summary>
    ///This is a test class for CellTest and is intended
    ///to contain all CellTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CellTest
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
        ///A test for Cell Constructor
        ///</summary>
        [TestMethod()]
        public void Constructor_id_is_ID_Property()
        {
            for (int id = 0; id < Consts.DimensionSquare; id++)
            {
                Cell target = new Cell(id);
                Assert.AreEqual(id, target.ID);
            }
        }

        [TestMethod()]
        public void BaseValue_is_0_when_Digit_set()
        {
            int id = 0;
            Cell target = new Cell(id);
            int expected = 0;
            int actual;
            target.Digit = 1;
            actual = target.BaseValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CheckLastDigit
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Sudoku.exe")]
        public void CheckLastDigit_returns_false_when_not_last_candidate_after_create()
        {
            Cell_Accessor target = new Cell_Accessor(0);
            SudokuLog sudokuResult = null;
            bool expected = false;
            bool actual = target.CheckLastDigit(sudokuResult);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CheckLastDigit
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Sudoku.exe")]
        public void CheckLastDigit_returns_false_when_not_last_candidate()
        {
            Cell_Accessor target = new Cell_Accessor(0);
            SudokuLog sudokuResult = null;
            bool expected = false;
            bool actual = false; target.CheckLastDigit(sudokuResult);
            int newBaseValue = 0;
            for (int x = 0; x < Consts.DimensionSquare - 3; x++)
            {
                for (int y = x + 1; y < Consts.DimensionSquare - 2; y++)
                {
                    newBaseValue = (1 << x) | (1 << y);
                    target.BaseValue = newBaseValue;
                    actual = target.CheckLastDigit(sudokuResult);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void Equal_is_false_when_compare_with_null()
        {
            int id = 0;
            Cell target = new Cell(id);
            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void Equal_is_true_when_ID_is_equal()
        {
            int id = 0;
            Cell target = new Cell(id);
            object obj = new Cell(id);
            bool expected = true;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCode_is_always_unique()
        {
            Cell[] cells = new Cell[Consts.DimensionSquare * Consts.DimensionSquare];
            for (int id = 0; id < Consts.DimensionSquare * Consts.DimensionSquare; id++)
            {
                cells[id] = new Cell(id);
            }

            for (int id = 0; id < Consts.DimensionSquare * Consts.DimensionSquare; id++)
            {
                for (int id2 = 0; id2 < Consts.DimensionSquare * Consts.DimensionSquare; id2++)
                {
                    Cell target = cells[id];
                    if (id == id2)
                    {
                        Assert.AreEqual(cells[id].GetHashCode(), cells[id2].GetHashCode());
                    }
                    else
                    {
                        Assert.AreNotEqual(cells[id].GetHashCode(), cells[id2].GetHashCode());
                    }
                }
            }
        }

        /// <summary>
        ///A test for RemovePossibleDigit
        ///</summary>
        [TestMethod()]
        public void RemovePossibleDigit_BaseValue_changes_when_candidate_removes()
        {
            int id = 0;
            Cell target = new Cell(id);
            int digit = 1;
            SudokuLog sudokuResult = new SudokuLog();
            bool expected = true;
            bool actual;
            actual = target.RemovePossibleDigit(digit, sudokuResult);
            Assert.AreEqual(expected, actual);
        }

        ///// <summary>
        /////A test for SetDigit
        /////</summary>
        //[TestMethod()]
        //public void SetDigit_changes_Digit_peoperty()
        //{
        //    int id = 0;
        //    Cell target = new Cell(id);
        //    int digit = 1; // TODO: Initialize to an appropriate value
        //    //SudokuLog expected = null; // TODO: Initialize to an appropriate value
        //    SudokuLog actual;
        //    actual = target.SetDigit(digit);
        //    Assert.AreEqual(digit, target.Digit);
        //}

        ///// <summary>
        /////A test for SetDigit
        /////</summary>
        //[TestMethod()]
        //public void SetDigit_set_BaseValue_to_0()
        //{
        //    int id = 0;
        //    Cell target = new Cell(id);
        //    int digit = 1; // TODO: Initialize to an appropriate value
        //    //SudokuLog expected = null; // TODO: Initialize to an appropriate value
        //    SudokuLog actual;
        //    actual = target.SetDigit(digit);
        //    Assert.AreEqual(0, target.BaseValue);
        //}

        ///// <summary>
        /////A test for SetDigit
        /////</summary>
        //[TestMethod()]
        //public void SetDigitTest1()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    Cell target = new Cell(id); // TODO: Initialize to an appropriate value
        //    int digitFromOutside = 0; // TODO: Initialize to an appropriate value
        //    SudokuLog sudokuResult = null; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.SetDigit(digitFromOutside, sudokuResult);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            Cell target = new Cell(id); // TODO: Initialize to an appropriate value
            string expected = "Cell(0) [A1] 0";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BaseValue
        ///</summary>
        [TestMethod()]
        public void Constructor_BaseValue_is_Consts_BaseStart()
        {
            int id = 0;
            Cell target = new Cell(id);
            int expected = Consts.BaseStart;
            int actual = target.BaseValue;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Constructor_HType_eq_HouseType_Cell()
        {
            int id = 0;
            Cell target = new Cell(id);
            HouseType expected = HouseType.Cell;
            HouseType actual = target.HType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Candidates
        ///</summary>
        [TestMethod()]
        public void Candidates_are_9()
        {
            int id = 0;
            Cell target = new Cell(id);
            List<int> actual;
            actual = target.Candidates;
            Assert.AreEqual(Consts.DimensionSquare, actual.Count);
            for (int i = 0; i < Consts.DimensionSquare; i++)
            {
                Assert.AreEqual(actual[i], i + 1);
            }
        }

        /// <summary>
        ///A test for Candidates
        ///</summary>
        [TestMethod()]
        public void Candidates_changes_with_removePossibleDigit()
        {
            int id = 0;
            Cell target = new Cell(id);
            List<int> actual;
            target.RemovePossibleDigit(3, new SudokuLog());
            actual = target.Candidates;
            Assert.AreEqual(Consts.DimensionSquare - 1, actual.Count);
            for (int i = 1; i <= Consts.DimensionSquare; i++)
            {
                if (i != 3)
                    Assert.IsTrue(actual.Contains(i));
            }
        }

        /// <summary>
        ///A test for Digit
        ///</summary>
        [TestMethod()]
        public void Digit_set_Digit()
        {
            int id = 0;
            Cell target = new Cell(id);
            int expected = 1;
            int actual;
            target.Digit = expected;
            actual = target.Digit;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Digit
        ///</summary>
        [TestMethod()]
        public void Digit_set_Digit_changes_BaseValue_to_0()
        {
            int id = 0;
            Cell target = new Cell(id);
            int expected = 0;
            int actual;
            target.Digit = 3;
            actual = target.BaseValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Digit
        ///</summary>
        [TestMethod()]
        public void Digit_does_not_set_Digit_when_not_in_range()
        {
            int id = 0;
            Cell target = new Cell(id);
            int expected = 0;
            int actual;
            try
            {
                target.Digit = -1;
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Digit -1 is in Cell 0 not possible", ex.Message);
            }
            finally
            {
                actual = target.Digit;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}