using de.onnen.Sudoku.SudokuExternal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DE.Onnen.Sudoku
{
	public class ACellCollection : ICellCollection
	{
		protected Cell[] cells;

		/// <summary>
		/// Reset Board.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				cells[i].Digit = 0;
			}
		}

		#region IList<ICell> Members

		public ICell this[int index]
		{
			get { return this.cells[index]; }
		}

		#endregion IList<ICell> Members

		#region ICollection<ICell> Members

		public void Add(ICell item)
		{
			throw new NotImplementedException();
		}

		public bool Contains(ICell item)
		{
			return this.cells.Contains(item);
		}

		public void CopyTo(ICell[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return this.cells.Count(); }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public bool Remove(ICell item)
		{
			throw new NotImplementedException();
		}

		#endregion ICollection<ICell> Members

		#region IEnumerable<ICell> Members

		public IEnumerator<ICell> GetEnumerator()
		{
			return this.cells.Select(x => (ICell)x).GetEnumerator();
		}

		#endregion IEnumerable<ICell> Members

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.cells.GetEnumerator();
		}

		#endregion IEnumerable Members
	}
}