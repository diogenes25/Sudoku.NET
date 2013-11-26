﻿using System.Collections.Generic;
using System.Linq;
using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechniques;

namespace de.onnen.Sudoku.SolveTechniques
{
	/// <summary>
	/// Naked Pair, Triplet, Quad (Locked Set, Naked Subset, Disjoint Subset)
	/// <remarks>
	/// If two cells in the same house (row, column or block) have only the same two candidates, 
	/// then those candidates can be removed from other cells in that house (row, column or block).
	/// This technique is known as "naked pair" if two candidates are involved, "naked triplet" if three, or "naked quad" if four.
	/// </remarks>
	/// </summary>
	public class NakedPairTrippleQuad : ASolveTechnique
	{
		public NakedPairTrippleQuad()
		{
			this.Info.Caption = "Naked PairTripleQuad";
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
					string st = "Naked" + count;
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