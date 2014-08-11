using DE.Onnen.Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
