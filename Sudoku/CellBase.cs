using de.onnen.Sudoku.SudokuExternal;

namespace de.onnen.Sudoku
{
	public abstract class CellBase : ICellBase
	{
		public HouseType HType { get { return this.houseType; } }

		public int ID { get { return this.id; } }

		protected int id;
		internal HouseType houseType;

		public int BaseValue { get { return this.baseValue; } }

		internal int baseValue = 0;

		public abstract bool SetDigit(int digit, SudokuLog sudokuResult);
	}
}