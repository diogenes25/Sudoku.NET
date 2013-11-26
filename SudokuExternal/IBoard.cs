namespace de.onnen.Sudoku.SudokuExternal
{
	/// <summary>
	/// sudoku puzzle. 
	/// <remarks>
	/// It contains the 81 constituent cells, lined up in 9 rows and 9 columns, with a distinct border around the boxes.
	/// </remarks>
	/// </summary>
	public interface IBoard
	{
		/// <summary>
		/// 81 cells of the board.
		/// </summary>
		ICell[] Cells { get; }

		/// <summary>
		/// Returns a specific house.
		/// </summary>
		/// <param name="houseType"></param>
		/// <param name="idx"></param>
		/// <returns></returns>
		IHouse GetHouse(HouseType houseType, int idx);
	}

	public static class Consts
	{
		public const int Dimension = 3;
		public const int DimensionSquare = Dimension * Dimension;
	}
}