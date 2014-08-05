namespace DE.ONNEN.Sudoku.SudokuExternal
{
	/// <summary>
	/// Sudoku puzzle.
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

		/// <summary>
		/// Return true when Sudoku is completed.
		/// </summary>
		bool IsComplete { get; }

		/// <summary>
		/// Set a digit at cell.
		/// </summary>
		/// <param name="cell">ID of cell</param>
		/// <param name="digit">Digit</param>
		SudokuLog SetDigit(int cellid, int digit);
	}

	/// <summary>
	/// Constant values.
	/// </summary>
	/// <remarks>
	///
	/// </remarks>
	public static class Consts
	{
		/// <summary>
		/// Kantenlänge einer Box.<br />
		/// </summary>
		/// <remarks>
		/// Im normalen Sudoku = 3.<br />
		/// Letztlich beziehen sich alle anderen Konstanden auf diesen Wert.
		/// </remarks>
		public const int Dimension = 3;

		/// <summary>
		/// Gesamte Kantenlänge des Sudoku.<br />
		/// </summary>
		/// <remarks>
		/// Im normalen Sudoku = 9 (3*3).
		/// </remarks>
		public const int DimensionSquare = Dimension * Dimension;

		/// <summary>
		/// Startwert (Bitmask) der Kandidaten.
		/// </summary>
		/// <remarks>
		/// Im normalen Sudoku = (9 Bit) = 2^9 = 511
		/// </remarks>
		public const int BaseStart = (1 << Consts.DimensionSquare) - 1;
	}
}