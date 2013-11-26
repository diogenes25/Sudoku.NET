namespace de.onnen.Sudoku.SudokuExternal
{
	public interface IHouse : ICellBase
	{
		bool Complete { get; }

		ICell[] Peers { get; }
	}

	public enum HouseType
	{
		Row = 0,
		Col = 1,
		Box = 2,
		Cell = 3,
	}
}