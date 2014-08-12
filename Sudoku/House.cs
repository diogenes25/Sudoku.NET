using DE.Onnen.Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DE.Onnen.Sudoku
{
    public class HouseCollection : ACellBase, IHouse
    {
        private ICell[] cells;

        /// <summary>
        /// true = some cells
        /// </summary>
        internal bool ReCheck { set; get; }

        /// <summary>
        /// Every Cell inside this House has a Digit.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                int retval = 0;
                foreach (Cell c in this.cells)
                {
                    retval |= c.CandidateValue;
                }
                return retval == 0;
            }
        }

        internal HouseCollection(ICell[] cells, HouseType containerType, int containerIdx)
        {
            this.CandidateValue = Consts.BaseStart;
            this.cells = cells;
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

        /// <summary>
        /// A Digit in one of his peers is set.
        /// </summary>
        /// <remarks>
        /// Check if digit is possible in this house.
        /// </remarks>
        /// <param name="digit"></param>
        /// <param name="sudokuResult"></param>
        /// <returns></returns>
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
            foreach (ICell cell in this.cells)
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

            if (!ok)
            {
                SudokuLog resultError = sudokuResult.CreateChildResult();
                resultError.EventInfoInResult = sudokuEvent;
                resultError.Successful = false;
                resultError.ErrorMessage = "Digit " + digit + " is in CellContainer not possible";
                return true;
            }

            this.candidateValueInternal = newBaseValue;

            SudokuLog result = sudokuResult.CreateChildResult();
            result.EventInfoInResult = new SudokuEvent()
            {
                ChangedCellBase = this,
                Action = CellAction.SetDigitInt,
                SolveTechnik = "None",
                value = digit,
            };

            foreach (Cell cell in cells)
            {
                cell.RemoveCandidate(digit, result);
            }
            return true;
        }

        public override string ToString()
        {
            return this.HType + "(" + this.ID + ") " + this.CandidateValue;
        }

        #region IList<ICell> Members

        public ICell this[int index]
        {
            get { return this.cells[index]; }
        }

        #endregion IList<ICell> Members

        #region ICollection<ICell> Members

        public void Add(ICell item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(ICell item)
        {
            return this.cells.Contains(item);
        }

        public void CopyTo(ICell[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.cells.Length; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(ICell item)
        {
            throw new NotImplementedException();
        }

        #endregion ICollection<ICell> Members

        #region IEnumerable<ICell> Members

        public IEnumerator<ICell> GetEnumerator()
        {
            return this.cells.Select(x => (ICell)x).GetEnumerator();
        }

        #endregion IEnumerable<ICell> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cells.GetEnumerator();
        }

        #endregion IEnumerable Members


        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}