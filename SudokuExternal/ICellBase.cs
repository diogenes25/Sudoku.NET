namespace de.onnen.Sudoku.SudokuExternal
{
	public interface ICellBase
	{
		HouseType ContainerType { get; }

		int ID { get; }
	}
}