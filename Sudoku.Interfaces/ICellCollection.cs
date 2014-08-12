using DE.Onnen.Sudoku;
using System.Collections.Generic;

namespace de.onnen.Sudoku.SudokuExternal
{
	public interface ICellCollection : ICollection<ICell>
	{
		ICell this[int index]
		{
			get;
		}
	}
}