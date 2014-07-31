using System.Collections.Generic;
using System.ComponentModel;
using de.onnen.Sudoku.SudokuExternal;

namespace de.onnen.Sudoku
{
    public abstract class CellBase : ICellBase, INotifyPropertyChanged
    {
        protected int baseValue = 0;

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

        public int BaseValue
        {
            get { return this.baseValue; }
            internal set { SetField(ref this.baseValue, value, "BaseValue"); }
        }

        internal abstract bool SetDigit(int digit, SudokuLog sudokuResult);

        //public abstract SudokuLog SetDigit(int digit);

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