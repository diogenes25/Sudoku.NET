using System.Collections.Generic;
using System.Linq;
using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechnics;

namespace de.onnen.Sudoku.SolveTechnics
{
	public class TwinTripleQuad : ASolveTechnic
	{
		public TwinTripleQuad()
		{
			this.Info.Caption = "TwinTripleQuad";
		}

		public override void SolveHouse(IHouse house, SudokuResult sudokuResult)
		{
			// Key = BaseValue, Anz Possible
			Dictionary<int, List<ICell>> nakedMore = new Dictionary<int, List<ICell>>();
			foreach (ICell c in house.Peers)
			{
				int val = c.BaseValue;
				if (!nakedMore.ContainsKey(val))
					nakedMore.Add(val, new List<ICell>());
				nakedMore[val].Add(c);
			}

			foreach (KeyValuePair<int, List<ICell>> kv in nakedMore)
			{
				if (kv.Value.Count > 5)
					return;
				int count = kv.Value.First().Candidates.Count;
				if (kv.Value.Count == count)
				{
					string st = "Naked2";
					switch (count)
					{
						case 2: st = "Naked2"; break;
						case 3: st = "Naked3"; break;
						case 4: st = "Naked4"; break;
					}
					SudokuResult cresult = sudokuResult.CreateChildResult();
					cresult.EventInfoInResult = new SudokuEvent()
					{
						value = 0,
						ChangedCellBase = house,
						Action = CellAction.RemPoss,
						SolveTechnik = st,
					};
					bool found = false;
					foreach (ICell c in house.Peers)
					{
						if (kv.Value.Contains(c))
						{
							continue;
						}
						ICell kvc = kv.Value.First();
						foreach (int d in kvc.Candidates)
						{
							found |= c.RemovePossibleDigit(d, cresult);
						}
					}
					if (!found)
					{
						sudokuResult.ChildSudokuResult.Remove(cresult);
					}
				}
			}
		}
	}
}