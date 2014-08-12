using System.Collections.Generic;

namespace DE.Onnen.Sudoku
{
	/// <summary>
	/// A group of 9 cells.
	/// <remarks>
	/// Each cell must contain a different digit in the solution.
	/// In standard sudoku, a house can be a row, a column or a box. There are 27 houses in a standard sudoku grid.
	/// </remarks>
	/// </summary>
	public interface IHouse : ICellBase, ICollection<ICell>
	{
		/// <summary>
		/// 81 cells of the board.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ICell this[int index]
		{
			get;
		}

		/// <summary>
		/// true == Every digit ist set.
		/// </summary>
		bool IsComplete { get; }

		///// <summary>
		///// Cells (buddies) of this house.
		///// </summary>
		//ICell[] Peers { get; }
	}

	/// <summary>
	/// Type of house.
	/// </summary>
	public enum HouseType
	{
		/// <summary>
		/// Row
		/// </summary>
		Row = 0,

		/// <summary>
		/// Column
		/// </summary>
		Col = 1,

		/// <summary>
		/// Box (or Block)
		/// </summary>
		Box = 2,

		Cell = 3,
	}
}