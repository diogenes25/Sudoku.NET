using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechniques;
using System.Collections.Generic;
using System.Linq;

namespace de.onnen.Sudoku.SolveTechniques
{
	/// <summary>
	/// Hidden Pair / Triple/ Quad
	/// </summary>
	/// <remarks>
	/// This technique is very similar to naked subsets, but instead of affecting other cells with the same row, column or block, candidates are eliminated from the cells that hold the subset. If there are N cells, with N candidates between them that don't appear elsewhere in the same row, column or block, then any other candidates for those cells can be eliminated.
	/// </remarks>
	public class HiddenPairTripleQuad : ASolveTechnique
	{
		public HiddenPairTripleQuad()
		{
			this.Info.Caption = "Hidden TwinTripleQuad";
			this.Info.Description = "This technique is very similar to naked subsets, but instead of affecting other cells with the same row, column or block, candidates are eliminated from the cells that hold the subset. If there are N cells, with N candidates between them that don't appear elsewhere in the same row, column or block, then any other candidates for those cells can be eliminated.";
		}

		public override void SolveHouse(IHouse house, SudokuLog sudokuResult)
		{
			// Digit 1:n Cell (contains Digit)
			// Key = Digit - Value = Alle Zellen die dieses Enthalten
			Dictionary<int, List<ICell>> digitInCell = new Dictionary<int, List<ICell>>();

			for (int i = 0; i < Consts.DimensionSquare; i++)
			{
				if (house.Peers[i].Digit == 0)
				{
					List<int> posDigit = house.Peers[i].Candidates;
					foreach (int num in posDigit)
					{
						if (!digitInCell.ContainsKey(num))
						{
							digitInCell.Add(num, new List<ICell>());
						}
						digitInCell[num].Add(house.Peers[i]);
					}
				}
			}

			// Key = Anzahl der Zellen Value = Liste der Zellen Key = Digit
			Dictionary<int, Dictionary<int, List<ICell>>> countDigitInCell = new Dictionary<int, Dictionary<int, List<ICell>>>();
			foreach (KeyValuePair<int, List<ICell>> kv in digitInCell)
			{
				// Wenn ein Key/Digit nur in eine einzigen Zelle enthalten ist, muss dieses Digit gesetzt werden.
				if (kv.Value.Count == 1 && kv.Value.First().CandidateValue > 0)
				{
					//SudokuLog cresult = sudokuResult.CreateChildResult();
					//cresult.EventInfoInResult = new SudokuEvent()
					//{
					//    value = kv.Key,
					//    ChangedCellBase = house,
					//    Action = CellAction.SetDigitInt,
					//    SolveTechnik = "FullHouse",
					//};

					SudokuLog cresult = kv.Value.First().SetDigit(kv.Key);
					if (sudokuResult.Successful)
					{
						sudokuResult.ChildSudokuResult.Add(cresult);
						cresult.ParentSudokuResult = sudokuResult;
					}
					if (!sudokuResult.Successful)
					{
						return;
					}
				}
				else
					if (kv.Value.Count < 5)
					{
						if (!countDigitInCell.ContainsKey(kv.Value.Count))
						{
							countDigitInCell.Add(kv.Value.Count, new Dictionary<int, List<ICell>>());
						}
						countDigitInCell[kv.Value.Count].Add(kv.Key, kv.Value);
					}
			}

			foreach (KeyValuePair<int, Dictionary<int, List<ICell>>> kv in countDigitInCell)
			{
				int countCells = kv.Key; // Anzahl der Zellen in denen ein Digit enthalten ist
				if (kv.Value.Count < 2 || kv.Value.Count != countCells)
					continue;
				string st = "Hidden2";
				switch (countCells)
				{
					case 2: st = "Hidden2"; break;
					case 3: st = "Hidden3"; break;
					case 4: st = "Hidden4"; break;
				}

				bool eq = true;
				List<ICell> last = null;
				foreach (KeyValuePair<int, List<ICell>> kv2 in kv.Value)
				{
					if (last == null)
					{
						last = kv2.Value;
						continue;
					}
					for (int i = 0; i < kv.Key; i++)
					{
						eq &= last[i].Equals(kv2.Value[i]);
					}
					last = kv2.Value;
				}

				if (eq)
				{
					SudokuLog cresult = sudokuResult.CreateChildResult();
					cresult.EventInfoInResult = new SudokuEvent()
					{
						ChangedCellBase = house,
						Action = CellAction.RemPoss,
						value = 999999999,
						SolveTechnik = st,
					};
					bool found = false;
					List<ICell> cc = kv.Value.Values.First();
					foreach (ICell c in cc)
					{
						for (int i = 1; i < Consts.DimensionSquare + 1; i++)
						{
							if (kv.Value.Keys.Contains(i))
								continue;
							found |= c.RemoveCandidate(i, cresult);
						}
					}
					if (!found)
					{
						sudokuResult.ChildSudokuResult.Remove(cresult);
					}
				}
			}
		}

		public new ECellView CellView
		{
			get { return ECellView.OnlyHouse; }
		}
	}
}