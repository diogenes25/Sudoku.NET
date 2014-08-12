using DE.Onnen.Sudoku;
using DE.Onnen.Sudoku.SolveTechniques;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudoku.SolveTechniques
{
	[TestClass]
	public class HiddenPairTripleQuadTest
	{
		private static IList<ASolveTechnique> solveTechniques;

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

		#endregion Additional test attributes

		private static IList<ASolveTechnique> GetSolveTechniques()
		{
			IList<ASolveTechnique> st = new List<ASolveTechnique>();
			st.Add(new DE.Onnen.Sudoku.SolveTechniques.HiddenPairTripleQuad());
			return st;
		}

		/// <summary>
		///A test for Backtracking
		///</summary>
		[TestMethod()]
		public void Backtracking_solve_without_any_digit_and_HiddenPairTripleQuad()
		{
			Board target = new Board(solveTechniques);
			SudokuLog log = new SudokuLog();
			target.Backtracking(log);
			Assert.IsTrue(log.Successful);
			for (int i = 0; i < Consts.DimensionSquare; i++)
			{
				Assert.AreEqual((i + 1), target[i].Digit);
			}
		
		
		}

		/// <summary>
		/// 8,9  aus Cell[30] bis Cell[35] löschen.
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
		[TestMethod]
		public void NakedPairTrippleQuadTest_in_Box()
		{
			IBoard board = new Board(solveTechniques);
			board.SetCellsFromString("123000000456000000700000000000000000000000000000000000000000000000000000000000000");
			// 8,9 sind in der Row[2] komplett gesetzt, obwohl diese beiden Digit nur in den Cellen 28 und 29 sein können.
			for (int i = 3; i < Consts.DimensionSquare; i++)
			{
				Assert.IsTrue(board.GetHouse(HouseType.Row, 2)[i].Candidates.Contains(8));
				Assert.IsTrue(board.GetHouse(HouseType.Row, 2)[i].Candidates.Contains(9));
			}
			board.Solve(new SudokuLog());
			// 8,9 sind in jetzt aus Cell[30] bis Cell[35].
			for (int i = 3; i < Consts.DimensionSquare; i++)
			{
				Assert.IsFalse(board.GetHouse(HouseType.Row, 2)[i].Candidates.Contains(8));
				Assert.IsFalse(board.GetHouse(HouseType.Row, 2)[i].Candidates.Contains(9));
			}
		}

	}
}