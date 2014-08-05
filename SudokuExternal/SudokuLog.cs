using System.Collections.Generic;
using System.Text;

namespace DE.Onnen.Sudoku.SudokuExternal
{
	public class SudokuLog
	{
		private bool successful = true;

		public SudokuEvent EventInfoInResult { get; set; }

		public bool Successful
		{
			get
			{
				bool result = this.successful;
				foreach (SudokuLog sr in ChildSudokuResult)
				{
					result &= sr.Successful;
				}
				return result;
			}
			set { this.successful = value; }
		}

		public string ErrorMessage { get; set; }

		public SudokuLog ParentSudokuResult { get; set; }

		public List<SudokuLog> ChildSudokuResult { get; set; }

		public SudokuLog()
		{
			ChildSudokuResult = new List<SudokuLog>();
		}

		public SudokuLog CreateChildResult()
		{
			SudokuLog child = new SudokuLog();
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