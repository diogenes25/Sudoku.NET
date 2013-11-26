namespace de.onnen.Sudoku.SudokuExternal
{
	public interface ICellBase
	{
		/// <summary>
		/// The type of house (or cell).
		/// </summary>
		HouseType HType { get; }

		/// <summary>
		/// ID of the Cell/House.
		/// <remarks>
		/// There are 81 Cells and 9 horizontal rows, nine vertical columns, and nine 3 x 3 blocks (also called boxes).<br />
		/// The rows are numbered 0 to 8, with the top row being 0, and the bottom row being 8. <br />
		/// The columns are similarly numbered, with the leftmost column being 0, and the rightmost being column 9. <br />
		/// The boxes start counting with 0 on the leftmost corner on top.
		/// </remarks>
		/// </summary>
		int ID { get; }
	}
}