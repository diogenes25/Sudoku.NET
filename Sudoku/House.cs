using DE.Onnen.Sudoku;
using System.Collections.Generic;

namespace DE.Onnen.Sudoku
{
	public class House : ACellBase, IHouse
	{
		private ICell[] peers;

		public ICell[] Peers { get { return peers; } }

		internal bool ReCheck { set; get; }

		public bool Complete
		{
			get
			{
				int retval = 0;
				foreach (Cell c in this.peers)
				{
					retval |= c.CandidateValue;
				}
				return retval == 0;
			}
		}

		internal House(ICell[] cells, HouseType containerType, int containerIdx)
		{
			this.CandidateValue = Consts.BaseStart;
			this.peers = cells;
			this.HType = containerType;
			this.ID = containerIdx;
			this.ReCheck = false;
			foreach (ICell c in cells)
			{
				c.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Cell_PropertyChanged);
			}
		}

		private void Cell_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Digit"))
				this.ReCheck = true;
		}

		internal override bool SetDigit(int digit, SudokuLog sudokuResult)
		{
			SudokuEvent sudokuEvent = new SudokuEvent()
											{
												value = digit,
												SolveTechnik = "SetDigit",
												ChangedCellBase = this,
												Action = CellAction.SetDigitInt
											};
			bool ok = true;
			int newBaseValue = Consts.BaseStart;
			HashSet<int> m = new HashSet<int>();
			foreach (ICell cell in this.peers)
			{
				if (cell.Digit > 0)
				{
					if (m.Contains(cell.Digit))
					{
						ok = false;
						break;
					}
					else
					{
						m.Add(cell.Digit);
						newBaseValue -= (1 << (cell.Digit - 1));
					}
				}
			}

			//if ((newBaseValue & (1 << (digit - 1))) != (1 << (digit - 1)))
			if (!ok)
			{
				SudokuLog resultError = sudokuResult.CreateChildResult();
				resultError.EventInfoInResult = sudokuEvent;
				resultError.Successful = false;
				resultError.ErrorMessage = "Digit " + digit + " is in CellContainer not possible";
				return true;
			}

			this.candidateValueInternal = newBaseValue;
			//int tmpBaseValue = (1 << (digit - 1));
			//int tmp = this.BaseValue ^ tmpBaseValue;
			//int newBaseValue = this.BaseValue & tmp;
			//if (newBaseValue == this.BaseValue)
			//    return false;
			//this.BaseValue = newBaseValue;

			SudokuLog result = sudokuResult.CreateChildResult();
			result.EventInfoInResult = new SudokuEvent()
			{
				ChangedCellBase = this,
				Action = CellAction.SetDigitInt,
				SolveTechnik = "None",
				value = digit,
			};

			foreach (Cell cell in peers)
			{
				cell.RemoveCandidate(digit, result);
			}
			return true;
		}

		public override string ToString()
		{
			return this.HType + "(" + this.ID + ") " + this.CandidateValue;
		}
	}
}