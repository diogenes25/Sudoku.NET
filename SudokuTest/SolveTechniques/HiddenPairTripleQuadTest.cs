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

		

	}
}