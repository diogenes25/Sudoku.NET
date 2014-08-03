using System.Collections.Generic;
using System.ComponentModel;
using de.onnen.Sudoku.SudokuExternal;

namespace de.onnen.Sudoku
{
    public abstract class CellBase : ICellBase, INotifyPropertyChanged
    {
        protected int candidateValue = 0;

        private int id;

        //private HouseType houseType;

        public event PropertyChangedEventHandler PropertyChanged;

        public HouseType HType
        {
            get;
            protected set;
        }

        public int ID
        {
            get { return this.id; }
            protected set { this.id = value; }
        }

        public int CandidateValue
        {
            get { return this.candidateValue; }
            internal set { SetField(ref this.candidateValue, value, "CandidateValue"); }
        }

        internal abstract bool SetDigit(int digit, SudokuLog sudokuResult);

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}