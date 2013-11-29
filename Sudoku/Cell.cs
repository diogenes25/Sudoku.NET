using System.Collections.Generic;
using System.Diagnostics;
using de.onnen.Sudoku.SudokuExternal;
using System.ComponentModel;

namespace de.onnen.Sudoku
{
	[DebuggerDisplay("Cell-ID {id}")]
	public class Cell : CellBase, ICell
	{
		private Board board;

		internal int digit = 0;
		internal House[] Fieldcontainters = new House[3];

		public event de.onnen.Sudoku.SudokuExternal.CellEventHandler CellEvent;

		public int Digit { get { return this.digit; } }

		public int BaseValue { get { return this.baseValue; } }

		internal Cell(Board board, int id)
		{
			this.houseType = HouseType.Cell;
			this.board = board;
			this.id = id;
			this.baseValue = (1 << Consts.DimensionSquare) - 1;
		}

		internal void FireEvent(SudokuEvent eventInfo)
		{
			if (this.CellEvent != null)
			{
				this.CellEvent(eventInfo);
			}
		}

		public override bool Equals(object obj)
		{
			return this.id == ((Cell)obj).id;
		}

		public bool RemovePossibleDigit(int digit, SudokuLog sudokuResult)
		{
			//if (((1 << (digit - 1)) & this.baseValue) == 0)
			// 0110101 Abziehen
			// 0011010 Original
			// 0101111
			// -------
			// 0001010 Ergebnis
			//int tmp = this.baseValue ^ remBaseValue;
			//int newBaseValue = this.baseValue & tmp;
			//if (newBaseValue == this.baseValue)
			//    return false;
			if ((this.baseValue & (1 << (digit - 1))) == 0)
				return false;

			this.baseValue -= (1 << (digit - 1));

			SudokuEvent eventInfoInResult = new SudokuEvent()
			{
				ChangedCellBase = this,
				Action = CellAction.RemPoss,
				SolveTechnik = "SetDigit",
				value = digit,
			};
			SudokuLog nakeResult = sudokuResult.CreateChildResult();
			nakeResult.EventInfoInResult = eventInfoInResult;

			CheckLastDigit(sudokuResult);

			if (!sudokuResult.Successful)
			{
				nakeResult.Successful = false;
				nakeResult.ErrorMessage = "RemovePossibleBaseValue";
				return true;
			}
			this.board.ReCheck();
			for (int i = 0; i < 3; i++)
			{
				this.Fieldcontainters[i].ReCheck = true;
			}

			FireEvent(nakeResult.EventInfoInResult);

			return true;
		}

		/// <summary>
		/// Check if there is only one possible Digit to choose left.
		/// </summary>
		private bool CheckLastDigit(SudokuLog sudokuResult)
		{
			int ret = -1;
			for (int i = 0; i < Consts.DimensionSquare; i++)
			{
				if (((1 << (i)) & this.baseValue) > 0)
				{
					if (ret > -1)
						return false;
					ret = i;
				}
			}
			SudokuLog sresult = sudokuResult.CreateChildResult();
			sresult.EventInfoInResult = new SudokuEvent()
			{
				value = ret + 1,
				ChangedCellBase = this,
				Action = CellAction.RemPoss,
				SolveTechnik = "LastdDigit",
			};

			return SetDigit(ret + 1, sresult);
		}

		/// <summary>
		/// Set digit in cell.
		/// </summary>
		/// <param name="digit"></param>
		public override bool SetDigit(int digit, SudokuLog sudokuResult)
		{
			if (this.digit == digit)
				return false; ;

			SudokuLog result = sudokuResult.CreateChildResult();
			result.EventInfoInResult = new SudokuEvent()
			{
				ChangedCellBase = this,
				Action = CellAction.SetDigitInt,
				SolveTechnik = "None",
				value = digit,
			};

			if (digit < 1 || digit > Consts.DimensionSquare || this.digit > 0 || (this.baseValue & (1 << (digit - 1))) != (1 << (digit - 1)))
			{
				sudokuResult.Successful = false;
				sudokuResult.ErrorMessage = "Digit " + digit + " is in Cell " + this.id + " not possible";
				return true;
			}

			this.board.ReCheck();
			this.baseValue = 0;
			this.digit = digit;
			RaisePropertyChanged("Digit");

			for (int i = 0; i < 3; i++)
			{
				this.Fieldcontainters[i].ReCheck = true;
				this.Fieldcontainters[i].SetDigit(digit, sudokuResult);
				if (!sudokuResult.Successful)
				{
					sudokuResult.Successful = false;
					sudokuResult.ErrorMessage = "Digit " + digit + " is in Cell (FieldContainer) " + this.id + " not possible";
					return true;
				}
			}

			FireEvent(result.EventInfoInResult);

			return true;
		}

		public override string ToString()
		{
			return this.houseType + "(" + this.id + ") [" + ((char)(int)((this.id / Consts.DimensionSquare) + 65)) + "" + ((this.id % Consts.DimensionSquare) + 1) + "] " + this.digit;
		}

		public override int GetHashCode()
		{
			return this.id;
		}

		public List<int> Candidates
		{
			get
			{
				List<int> retInt = new List<int>();
				for (int i = 0; i < Consts.DimensionSquare; i++)
				{
					if (((1 << i) & this.BaseValue) > 0)
						retInt.Add(i + 1);
				}
				return retInt;
			}
		}

		public int ID { get { return this.id; } }

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			// take a copy to prevent thread issues
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}