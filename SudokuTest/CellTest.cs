using DE.Onnen.Sudoku;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

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

		[TestMethod()]
		public void BaseValue_Digit_is_0_when_BaseValue_was_set()
		{
			int id = 0;
			Cell target = new Cell(id);
			int expected = 0;
			int actual;
			target.Digit = 1;
			actual = target.CandidateValue;
			Assert.AreEqual(expected, actual);
			target.CandidateValue = 3;
			actual = target.Digit;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void BaseValue_is_0_when_Digit_set()
		{
			int id = 0;
			Cell target = new Cell(id);
			int expected = 0;
			int actual;
			target.Digit = 1;
			actual = target.CandidateValue;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void BaseValue_fire_OnPropertyChanged_event_when_value_changes()
		{
			int id = 0;
			Cell target = new Cell(id);
			int expected = 3;
			int actual;
			bool propertyChangeWasDone = false;
			target.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((sender, e) =>
			{
				Assert.AreEqual("CandidateValue", e.PropertyName);
				Assert.AreEqual(sender, target);
				Assert.AreEqual(((Cell)sender).CandidateValue, expected);
				propertyChangeWasDone = true;
			});
			target.CandidateValue = expected;
			actual = target.CandidateValue;
			Assert.AreEqual(expected, actual);
			Assert.IsTrue(propertyChangeWasDone);
		}

		/// <summary>
		///A test for Candidates
		///</summary>
		[TestMethod()]
		public void Candidates_are_9()
		{
			int id = 0;
			Cell target = new Cell(id);
			ReadOnlyCollection<int> actual;
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
			ReadOnlyCollection<int> actual;
			target.RemoveCandidate(3, new SudokuLog());
			actual = target.Candidates;
			Assert.AreEqual(Consts.DimensionSquare - 1, actual.Count);
			for (int i = 1; i <= Consts.DimensionSquare; i++)
			{
				if (i != 3)
					Assert.IsTrue(actual.Contains(i));
			}
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
					target.CandidateValue = newBaseValue;
					actual = target.CheckLastDigit(sudokuResult);
					Assert.AreEqual(expected, actual);
				}
			}
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
		///A test for BaseValue
		///</summary>
		[TestMethod()]
		public void Constructor_BaseValue_is_Consts_BaseStart()
		{
			int id = 0;
			Cell target = new Cell(id);
			int expected = Consts.BaseStart;
			int actual = target.CandidateValue;
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
		public void Digit_fire_OnPropertyChanged_event_when_Digit_changes_twice()
		{
			int id = 0;
			Cell target = new Cell(id);
			int expected = 3;
			int actual;
			bool propertyChangeCandidateWasDone = false;
			bool propertyChangeDigitWasDone = false;
			target.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((sender, e) =>
			{
				if (e.PropertyName.Equals("Digit"))
				{
					Assert.AreEqual(sender, target);
					Assert.AreEqual(((Cell)sender).Digit, expected);
					propertyChangeDigitWasDone = true;
				}
				else if (e.PropertyName.Equals("CandidateValue"))
				{
					Assert.AreEqual(sender, target);
					Assert.AreEqual(((Cell)sender).CandidateValue, 0);
					propertyChangeCandidateWasDone = true;
				}
			});
			target.Digit = expected;
			actual = target.Digit;
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(0, target.CandidateValue);
			Assert.IsTrue(propertyChangeCandidateWasDone);
			Assert.IsTrue(propertyChangeDigitWasDone);
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
				Assert.AreEqual("Digit -1 is in Cell(0) [A1] 0 not possible", ex.Message);
			}
			finally
			{
				actual = target.Digit;
				Assert.AreEqual(expected, actual);
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

		[TestMethod()]
		public void Digit_set_Digit_removes_Candidates_in_Houses()
		{
			int id = 0;
			Cell target = new Cell(id);
			Cell[] row = new Cell[10];
			Cell[] col = new Cell[10];
			Cell[] box = new Cell[10];
			for (int i = 0; i < 10; i++)
			{
				row[i] = new Cell(i);
				col[i] = new Cell(i + 10);
				box[i] = new Cell(i + 20);
			}
			target.fieldcontainters[0] = new House(row, HouseType.Row, 1);
			target.fieldcontainters[1] = new House(col, HouseType.Col, 1);
			target.fieldcontainters[2] = new House(box, HouseType.Box, 1);

			int expected = Consts.BaseStart;
			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(expected, row[i].CandidateValue);
				Assert.AreEqual(expected, col[i].CandidateValue);
				Assert.AreEqual(expected, box[i].CandidateValue);
			}
			expected = Consts.BaseStart - 1;
			target.SetDigit(1);
			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(expected, row[i].CandidateValue);
				Assert.AreEqual(expected, col[i].CandidateValue);
				Assert.AreEqual(expected, box[i].CandidateValue);
			}
		}

		[TestMethod()]
		public void Digit_set_Digit_removes_Candidates_in_Houses_and_check_single_candidate()
		{
			int id = 0;
			Cell target = new Cell(id);
			Cell[] row = new Cell[2];
			Cell[] col = new Cell[2];
			Cell[] box = new Cell[2];
			for (int i = 0; i < 2; i++)
			{
				row[i] = new Cell(i);
				col[i] = new Cell(i + 10);
				box[i] = new Cell(i + 20);
			}
			target.fieldcontainters[0] = new House(row, HouseType.Row, 1);
			target.fieldcontainters[1] = new House(col, HouseType.Col, 1);
			target.fieldcontainters[2] = new House(box, HouseType.Box, 1);

			row[0].CandidateValue = 3;
			target.SetDigit(1);

			int expected = 2;
			Assert.AreEqual(expected, row[0].Digit);
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
			actual = target.CandidateValue;
			Assert.AreEqual(expected, actual);
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
		public void RemoveCandidate_BaseValue_changes_when_candidate_removes()
		{
			int id = 0;
			Cell target = new Cell(id);
			int digit = 1;
			SudokuLog sudokuResult = new SudokuLog();
			bool expected = true;
			bool actual;
			actual = target.RemoveCandidate(digit, sudokuResult);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void RemoveCandidate_checks_lastcandidate()
		{
			int id = 0;
			Cell target = new Cell(id);
			target.CandidateValue = 7;
			SudokuLog sudokuResult = new SudokuLog();
			bool expected = true;
			bool actual;
			actual = target.RemoveCandidate(1, sudokuResult);
			Assert.AreEqual(expected, actual);
			actual = target.RemoveCandidate(2, sudokuResult);
			Assert.AreEqual(expected, actual);
			expected = false;
			actual = target.RemoveCandidate(3, sudokuResult);
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(3, target.Digit);
		}

		/// <summary>
		///A test for SetDigit
		///</summary>
		[TestMethod()]
		public void SetDigit_changes_Digit_peoperty()
		{
			int id = 0;
			Cell target = new Cell(id);
			int digit = 1; // TODO: Initialize to an appropriate value
			//SudokuLog expected = null; // TODO: Initialize to an appropriate value
			SudokuLog actual;
			actual = target.SetDigit(digit);
			Assert.AreEqual(digit, target.Digit);
		}

		/// <summary>
		///A test for SetDigit
		///</summary>
		[TestMethod()]
		public void SetDigit_return_true_when_digit_was_set()
		{
			int id = 0;
			Cell target = new Cell(id);
			int digitFromOutside = 1;
			SudokuLog sudokuResult = new SudokuLog();
			bool expected = true;
			bool actual;
			actual = target.SetDigit(digitFromOutside, sudokuResult);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for SetDigit
		///</summary>
		[TestMethod()]
		public void SetDigit_set_BaseValue_to_0()
		{
			int id = 0;
			Cell target = new Cell(id);
			int digit = 1; // TODO: Initialize to an appropriate value
			//SudokuLog expected = null; // TODO: Initialize to an appropriate value
			SudokuLog actual;
			actual = target.SetDigit(digit);
			Assert.AreEqual(0, target.CandidateValue);
		}

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
	}
}