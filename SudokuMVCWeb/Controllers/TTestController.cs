using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SudokuMVCWeb.Controllers
{
	public class TTestController : Controller
	{
		// GET: TTest
		public string Index()
		{
			return "Hallo Tjark";
		}

		public string Welcome(string name, int numTimes=1)
		{
			return HttpUtility.HtmlEncode("Willkommen "+name+", NumTimes is: "+numTimes);
		}
	}
}