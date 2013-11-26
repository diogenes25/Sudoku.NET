using System.Collections.Generic;
using System.Text;

namespace de.onnen.Sudoku.SudokuExternal
{
	public class SudokuResult
	{
		private bool successful = true;

		public SudokuEvent EventInfoInResult { get; set; }

		public bool Successful
		{
			get
			{
				bool result = this.successful;
				foreach (SudokuResult sr in ChildSudokuResult)
				{
					result &= sr.Successful;
				}
				return result;
			}
			set { this.successful = value; }
		}

		public string ErrorMessage { get; set; }

		public SudokuResult ParentSudokuResult { get; set; }

		public List<SudokuResult> ChildSudokuResult { get; set; }

		public SudokuResult()
		{
			ChildSudokuResult = new List<SudokuResult>();
		}

		public SudokuResult CreateChildResult()
		{
			SudokuResult child = new SudokuResult();
			child.ParentSudokuResult = this;
			ChildSudokuResult.Add(child);
			return child;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Successful);
			if (EventInfoInResult != null)
			{
				sb.Append(", ");
				sb.Append(EventInfoInResult.ToString());
			}
			return sb.ToString();
		}
	}
}