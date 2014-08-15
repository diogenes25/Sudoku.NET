using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DE.Onnen.Sudoku;

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