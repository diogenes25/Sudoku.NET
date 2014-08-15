using DE.Onnen.Sudoku;
using System;
using System.Web.Mvc;

namespace SudokuMVCWeb.Controllers
{
	public class SudokuController : Controller
	{
		// GET: Sudoku
		//public ActionResult Index()
		//{
		//	return View(new Board());
		//}

		public ActionResult Index(string s)
		{
			Board board = new Board();
			if (!String.IsNullOrWhiteSpace(s))
			{
				board.SetCellsFromString(s);
			}
			return View(board);
		}
	}
}