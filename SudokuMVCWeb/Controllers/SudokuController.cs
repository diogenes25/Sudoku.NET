using DE.Onnen.Sudoku;
using DE.Onnen.Sudoku.SolveTechniques;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SudokuMVCWeb.Controllers
{
	public class SudokuController : Controller
	{
        private IList<ASolveTechnique> st;

        public SudokuController()
        {
            st = new List<ASolveTechnique>();
            st.Add(new DE.Onnen.Sudoku.SolveTechniques.NakedPairTrippleQuad());
            st.Add(new DE.Onnen.Sudoku.SolveTechniques.HiddenPairTripleQuad());
            st.Add(new DE.Onnen.Sudoku.SolveTechniques.LockedCandidates());
        }
		// GET: Sudoku
		//public ActionResult Index()
		//{
		//	return View(new Board());
		//}

		public ActionResult Index(string s)
		{
			Board board = new Board(st);
			if (!String.IsNullOrWhiteSpace(s))
			{
				board.SetCellsFromString(s);
			}
            board.Solve(new SudokuLog());
			return View(board);
		}
	}
}