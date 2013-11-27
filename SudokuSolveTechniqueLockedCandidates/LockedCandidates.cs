﻿using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechniques;

namespace de.onnen.Sudoku.SolveTechniques
{
	public class LockedCandidates : ASolveTechnique
	{
		public LockedCandidates()
		{
			this.Info.Caption = "LockedCandidates";
		}

		public override void SolveHouse(IHouse house, SudokuResult sudokuResult)
		{
			DigitInBlock(house, sudokuResult);
			if (house.HType == HouseType.Box)
				DigitInBlock(house, sudokuResult, true);
		}

		private void DigitInBlock(IHouse house, SudokuResult sudokuResult, bool verticalBlock = false)
		{
			int[] valueOnlyInOnePart = CheckInBlockpartOfRowCol(house, verticalBlock);
			for (int x = 0; x < 3; x++)
			{
				if (valueOnlyInOnePart[x] > 0)
				{
					int houseIdx = 0;
					HouseType cellInContainertype = house.HType;
					string st = "LockedCandidatesClaiming";
					switch (house.HType)
					{
						case HouseType.Row:
							houseIdx = ((int)(house.ID / Consts.Dimension)) * 3 + x;
							cellInContainertype = HouseType.Box;
							break;

						case HouseType.Col:
							houseIdx = x * 3 + (house.ID / Consts.Dimension);
							cellInContainertype = HouseType.Box;
							break;

						case HouseType.Box:
							st = "LockedCandidatesPointing";
							houseIdx = (verticalBlock) ? (house.ID % 3) * 3 + x : (house.ID / 3) * 3 + x;
							cellInContainertype = (verticalBlock) ? HouseType.Col : HouseType.Row;
							break;
					}

					int pos = -1;
					foreach (ICell c in board.GetHouse(cellInContainertype, houseIdx).Peers) // Cells des Row
					{
						pos++;
						if (((house.HType == HouseType.Row)
										&& (house.ID % Consts.Dimension) == (pos / Consts.Dimension)) ||
								 ((house.HType == HouseType.Col)
										&& (house.ID % Consts.Dimension) == (pos % Consts.Dimension)) ||
								 ((house.HType == HouseType.Box)
										&& ((!verticalBlock
												&& (house.ID % Consts.Dimension) == (pos / Consts.Dimension)) ||
										 (verticalBlock
												&& (house.ID / Consts.Dimension) == (pos / Consts.Dimension))))
								)
						{
							continue;
						}

						if (!RemoveMultiValue(c, valueOnlyInOnePart[x], sudokuResult, house, st))
							return;
					}
				}
			}
		}

		private bool RemoveMultiValue(ICell c, int removeValue, SudokuResult sudokuResult, ICellBase resultContainer, string solveTechnik)
		{
			for (int dc = 0; dc < Consts.DimensionSquare; dc++)
			{
				if ((removeValue & (1 << dc)) > 0)
				{
					if ((c.BaseValue & (1 << dc)) > 0)
					{
						SudokuResult child = sudokuResult.CreateChildResult();
						if (c.RemovePossibleDigit(dc + 1, child))
						{
							child.EventInfoInResult = new SudokuEvent()
							{
								ChangedCellBase = resultContainer,
								Action = CellAction.RemPoss,
								SolveTechnik = solveTechnik,
								value = dc + 1,
							};
						}
						else
						{
							sudokuResult.ChildSudokuResult.Remove(child);
						}
						if (!sudokuResult.Successful)
							return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Sucht Nummern die sich nur in einem Drittel der Container-Cells enthalten sind.
		/// </summary>
		/// <remarks>
		/// Die Nummern sind als |-Verknüpfung in dem Int-Array enthalten.<br/>
		/// Das Array enthält drei Elemente. Je nachdem ob die Digit im ersten (0) zweiten (1) oder drittem Drittel (2) enthalten sind.
		/// </remarks>
		/// <returns>Drei Int-Werte [erster, zweiter oder dritter Part] der gefundenen Nummern |-Verknüpft.</returns>
		private int[] CheckInBlockpartOfRowCol(IHouse house, bool verticalBlock = false)
		{
			int[] valueInRow = new int[Consts.Dimension];
			int[] valueOnlyInOneRow = new int[3];

			// Fügt alle noch möglichen Nummer pro Part zusammen.
			for (int p = 0; p < Consts.DimensionSquare; p++)
			{
				if (verticalBlock)
				{
					valueInRow[(p % Consts.Dimension)] |= house.Peers[p].BaseValue;
				}
				else
				{
					valueInRow[p / Consts.Dimension] |= house.Peers[p].BaseValue;
				}
			}

			// Finde die Nummern die nur in einem Part enthalten sind
			for (int part = 0; part < 3; part++)
			{
				int addVal = 0; // BaseValue der anderen beiden Parts.
				// Addiere die Nummern der beiden anderen Parts.
				for (int p = 0; p < 3; p++)
				{
					if (part == p)
						continue;
					addVal |= valueInRow[p];
				}
				valueOnlyInOneRow[part] = valueInRow[part] ^ addVal; // XOR-Verknüpfung um nur die unterschiedlichen Nummern zu erhalten.
				valueOnlyInOneRow[part] &= valueInRow[part]; // UND-Verknüpfung um nur die Werte zu erhalten auch zuvor in dem PArt waren.
			}
			return valueOnlyInOneRow;
		}
	}
}