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
            this.BaseValue = Consts.BaseStart;
            this.board = board;
            this.peers = cells;
            this.houseType = containerType;
            this.ID = containerIdx;
            this.ReCheck = false;
        }

        internal void RecalcBaseValue()
        {
            this.BaseValue = (1 << Consts.DimensionSquare) - 1;
            foreach (Cell c in this.peers)
            {
                if (c.Digit > 0)
                    this.BaseValue -= (1 << (c.Digit - 1));
            }
        }

        /// <summary>
        /// Removes the digit as a possible digit in every cell in this container.
        /// </summary>
        /// <param name="digit"></param>
        public override SudokuLog SetDigit(int digit)
        {
            return null;
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

            if ((this.BaseValue & (1 << (digit - 1))) != (1 << (digit - 1)))
            {
                SudokuLog resultError = sudokuResult.CreateChildResult();
                resultError.EventInfoInResult = sudokuEvent;
                resultError.Successful = false;
                resultError.ErrorMessage = "Digit " + digit + " is in CellContainer not possible";
                return true;
            }

            int tmpBaseValue = (1 << (digit - 1));
            int tmp = this.BaseValue ^ tmpBaseValue;
            int newBaseValue = this.BaseValue & tmp;
            if (newBaseValue == this.BaseValue)
                return false;
            this.BaseValue = newBaseValue;

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
                valueOnlyInOneRow[part] &= valueInRow[part]; // UND-Verknüpfung um nur die Werte zu erhalten die auch zuvor in dem Part waren.
            }
            return valueOnlyInOneRow;
        }

        public override string ToString()
        {
            return this.houseType + "(" + this.ID + ") " + this.BaseValue;
        }
    }
}