namespace de.onnen.Sudoku.SudokuExternal
{
	public interface IBoard
	{
		ICell[] Cells { get; }

		IHouse GetHouse(HouseType houseType, int idx);
	}

	public static class Consts
	{
		public const int Dimension = 3;
		public const int DimensionSquare = Dimension * Dimension;
	}
}