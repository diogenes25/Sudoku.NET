using de.onnen.Sudoku.SudokuExternal;

namespace de.onnen.Sudoku
{
	public class House : CellBase, IHouse
	{
		private ICell[] peers;
		private Board board;

		public ICell[] Peers { get { return peers; } }

		internal bool ReCheck { set; get; }

		public bool Complete
		{
			get
			{
				int retval = 0;
				foreach (Cell c in this.peers)
				{
					retval |= c.BaseValue;
				}
				return retval == 0;
			}
		}

		internal House(Board board, Cell[] cells, HouseType containerType, int containerIdx)
		{
			this.baseValue = (1 << Consts.DimensionSquare) - 1;
			this.board = board;
			this.peers = cells;
			this.houseType = containerType;
			this.id = containerIdx;
			this.ReCheck = false;
		}

		internal void RecalcBaseValue()
		{
			this.baseValue = (1 << Consts.DimensionSquare) - 1;
			foreach (Cell c in this.peers)
			{
				if (c.digit > 0)
					this.baseValue -= (1 << (c.digit - 1));
			}
		}

		/// <summary>
		/// Removes the digit as a possible digit in every cell in this container.
		/// </summary>
		/// <param name="digit"></param>
		public override bool SetDigit(int digit, SudokuResult sudokuResult)
		{
			SudokuEvent sudokuEvent = new SudokuEvent()
							{
								value = digit,
								SolveTechnik = "SetDigit",
								ChangedCellBase = this,
								Action = CellAction.SetDigitInt
							};

			if ((this.baseValue & (1 << (digit - 1))) != (1 << (digit - 1)))
			{
				SudokuResult resultError = sudokuResult.CreateChildResult();
				resultError.EventInfoInResult = sudokuEvent;
				resultError.Successful = false;
				resultError.ErrorMessage = "Digit " + digit + " is in CellContainer not possible";
				return true;
			}

			int tmpBaseValue = (1 << (digit - 1));
			int tmp = this.baseValue ^ tmpBaseValue;
			int newBaseValue = this.baseValue & tmp;
			if (newBaseValue == this.baseValue)
				return false;
			this.baseValue = newBaseValue;

			SudokuResult result = sudokuResult.CreateChildResult();
			result.EventInfoInResult = new SudokuEvent()
			{
				ChangedCellBase = this,
				Action = CellAction.SetDigitInt,
				SolveTechnik = "None",
				value = digit,
			};

			foreach (Cell cell in peers)
			{
				cell.RemovePossibleDigit(digit, result);
			}
			return true;
		}

		//internal void Check(SudokuResult sudokuResult)
		//{
		//    if (this.Complete || !this.ReCheck)
		//        return;
		//    CheckHiddenTwinTripleQuad(sudokuResult);
		//    if (sudokuResult.Successful)
		//        CheckTwinTripleQuad(sudokuResult);
		//    this.ReCheck = false;
		//}

		//private void CheckTwinTripleQuad(SudokuResult sudokuResult)
		//{
		//    // Key = BaseValue, Anz Possible
		//    Dictionary<int, List<Cell>> nakedMore = new Dictionary<int, List<Cell>>();
		//    foreach (Cell c in this.peers)
		//    {
		//        int val = c.BaseValue;
		//        if (!nakedMore.ContainsKey(val))
		//            nakedMore.Add(val, new List<Cell>());
		//        nakedMore[val].Add(c);
		//    }

		//    foreach (KeyValuePair<int, List<Cell>> kv in nakedMore)
		//    {
		//        if (kv.Value.Count > 5)
		//            return;
		//        int count = kv.Value.First().Candidates.Count;
		//        if (kv.Value.Count == count)
		//        {
		//            SolveTechnik st = SolveTechnik.Naked2;
		//            switch (count)
		//            {
		//                case 2: st = SolveTechnik.Naked2; break;
		//                case 3: st = SolveTechnik.Naked3; break;
		//                case 4: st = SolveTechnik.Naked4; break;
		//            }
		//            SudokuResult cresult = sudokuResult.CreateChildResult();
		//            cresult.EventInfoInResult = new SudokuEvent()
		//            {
		//                value = 0,
		//                cell = this,
		//                cellaction = CellAction.RemPoss,
		//                solveTechnik = st,
		//            };
		//            bool found = false;
		//            foreach (Cell c in this.peers)
		//            {
		//                if (kv.Value.Contains(c))
		//                {
		//                    continue;
		//                }
		//                Cell kvc = kv.Value.First();
		//                foreach (int d in kvc.Candidates)
		//                {
		//                    found |= c.RemovePossibleDigit(d, cresult);
		//                }
		//                //if (c.RemovePossibleBaseValue(kv.Value.First().BaseValue, sudokuResult))
		//                //{
		//                //board.FireEvent(c, CellAction.RemovePossible, kv.Value.First().BaseValue, this.containerType, this.containerIdx, st);
		//                //}
		//            }
		//            if (!found)
		//            {
		//                sudokuResult.ChildSudokuResult.Remove(cresult);
		//            }
		//        }
		//    }
		//}

		/// <summary>
		/// Sucht Nummern die sich nur in einem Drittel der Container-Cells enthalten sind.
		/// </summary>
		/// <remarks>
		/// Die Nummern sind als |-Verknüpfung in dem Int-Array enthalten.<br/>
		/// Das Array enthält drei Elemente. Je nachdem ob die Digit im ersten (0) zweiten (1) oder drittem Drittel (2) enthalten sind.
		/// </remarks>
		/// <returns>Drei Int-Werte [erster, zweiter oder dritter Part] der gefundenen Nummern |-Verknüpft.</returns>
		internal int[] CheckInBlockpartOfRowCol(bool verticalBlock = false)
		{
			int[] valueInRow = new int[Consts.Dimension];
			int[] valueOnlyInOneRow = new int[3];

			// Fügt alle noch möglichen Nummer pro Part zusammen.
			for (int p = 0; p < Consts.DimensionSquare; p++)
			{
				if (verticalBlock)
				{
					valueInRow[(p % Consts.Dimension)] |= this.peers[p].BaseValue;
				}
				else
				{
					valueInRow[p / Consts.Dimension] |= this.peers[p].BaseValue;
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

		//private void CheckHiddenTwinTripleQuad(SudokuResult sudokuResult)
		//{
		//    // Digit 1:n Cell (contains Digit)
		//    // Key = Digit - Value = Alle Zellen die dieses Enthalten
		//    Dictionary<int, List<Cell>> digitInCell = new Dictionary<int, List<Cell>>();

		//    for (int i = 0; i < Consts.DimensionSquare; i++)
		//    {
		//        if (this.peers[i].Digit == 0)
		//        {
		//            List<int> posDigit = this.peers[i].Candidates;
		//            foreach (int num in posDigit)
		//            {
		//                if (!digitInCell.ContainsKey(num))
		//                {
		//                    digitInCell.Add(num, new List<Cell>());
		//                }
		//                digitInCell[num].Add(this.peers[i]);
		//            }
		//        }
		//    }

		//    // Key = Anzahl der Zellen Value = Liste der Zellen Key = Digit
		//    Dictionary<int, Dictionary<int, List<Cell>>> countDigitInCell = new Dictionary<int, Dictionary<int, List<Cell>>>();
		//    foreach (KeyValuePair<int, List<Cell>> kv in digitInCell)
		//    {
		//        // Wenn ein Key/Digit nur in eine einzigen Zelle enthalten ist, muss dieses Digit gesetzt werden.
		//        if (kv.Value.Count == 1 && kv.Value.First().BaseValue > 0)
		//        {
		//            SudokuResult cresult = sudokuResult.CreateChildResult();
		//            cresult.EventInfoInResult = new SudokuEvent()
		//            {
		//                value = kv.Key,
		//                cell = this,
		//                cellaction = CellAction.SetDigitInt,
		//                solveTechnik = SolveTechnik.FullHouse,
		//            };

		//            if (!kv.Value.First().SetDigit(kv.Key, cresult))
		//            {
		//                sudokuResult.ChildSudokuResult.Remove(cresult);
		//            }
		//            if (!sudokuResult.Successful)
		//            {
		//                return;
		//            }
		//            //board.FireEvent(sudokuResult.EventInfoInResult);
		//        }
		//        else
		//            if (kv.Value.Count < 5)
		//            {
		//                if (!countDigitInCell.ContainsKey(kv.Value.Count))
		//                {
		//                    countDigitInCell.Add(kv.Value.Count, new Dictionary<int, List<Cell>>());
		//                }
		//                countDigitInCell[kv.Value.Count].Add(kv.Key, kv.Value);
		//            }
		//    }

		//    foreach (KeyValuePair<int, Dictionary<int, List<Cell>>> kv in countDigitInCell)
		//    {
		//        int countCells = kv.Key; // Anzahl der Zellen in denen ein Digit enthalten ist
		//        if (kv.Value.Count < 2 || kv.Value.Count != countCells)
		//            continue;
		//        SolveTechnik st = SolveTechnik.Hidden2;
		//        switch (countCells)
		//        {
		//            case 2: st = SolveTechnik.Hidden2; break;
		//            case 3: st = SolveTechnik.Hidden3; break;
		//            case 4: st = SolveTechnik.Hidden4; break;
		//        }

		//        bool eq = true;
		//        List<Cell> last = null;
		//        foreach (KeyValuePair<int, List<Cell>> kv2 in kv.Value)
		//        {
		//            if (last == null)
		//            {
		//                last = kv2.Value;
		//                continue;
		//            }
		//            for (int i = 0; i < kv.Key; i++)
		//            {
		//                eq &= last[i].Equals(kv2.Value[i]);
		//            }
		//            last = kv2.Value;
		//        }

		//        if (eq)
		//        {
		//            SudokuResult cresult = sudokuResult.CreateChildResult();
		//            cresult.EventInfoInResult = new SudokuEvent()
		//            {
		//                cell = this,
		//                cellaction = CellAction.RemPoss,
		//                value = 999999999,
		//                solveTechnik = st,
		//            };
		//            bool found = false;
		//            List<Cell> cc = kv.Value.Values.First();
		//            foreach (Cell c in cc)
		//            {
		//                for (int i = 1; i < Consts.DimensionSquare + 1; i++)
		//                {
		//                    if (kv.Value.Keys.Contains(i))
		//                        continue;
		//                    found |= c.RemovePossibleDigit(i, cresult);
		//                }
		//            }
		//            if (!found)
		//            {
		//                sudokuResult.ChildSudokuResult.Remove(cresult);
		//            }
		//        }
		//    }
		//}

		public override string ToString()
		{
			return this.houseType + "(" + this.id + ") " + this.baseValue;
		}
	}
}