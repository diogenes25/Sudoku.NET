using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using de.onnen.Sudoku.SudokuExternal;

namespace de.onnen.Sudoku
{
	public static class SudokuHelper
	{
		private static int countCell = Consts.DimensionSquare * Consts.DimensionSquare;
		public static int[] CreateSimpleBoard(Board board)
		{
			int[] retLst = new int[countCell];
			for (int i = 0; i < board.Cells.Length; i++)
			{
				if (board.Cells[i].Digit > 0)
				{
					retLst[i] = board.Cells[i].Digit * -1;
				}
				else
				{
					retLst[i] = board.Cells[i].CandidateValue;
				}
			}
			return retLst;
		}

		/// <summary>
		/// Load Sudokus from file.
		/// </summary>
		/// <remarks>format:<br/>
		/// 123...405<br />
		/// ...456...<br />
		/// 45......7 (etc.)
		/// </remarks>
		/// <param name="file">Path/File to textfile</param>
		/// <returns>List of Boards</returns>
		public static IList<Board> ReadBoardFromFileTop(string file)
		{
			IList<Board> retList = new List<Board>();
			TextReader tr = new StreamReader(file);
			string line;
			while ((line = tr.ReadLine()) != null)
			{
				if (line.Length < countCell)
					continue;
				Board board = new Board();
				for (int x = 0; x < countCell; x++)
				{
					char currChar = line[x];
					if (!currChar.Equals('.'))
					{
						int digit = Convert.ToInt32(currChar) - 48;
						SudokuLog result = board.SetDigit(x, digit);
						if (!result.Successful)
						{
							Debug.WriteLine(x);
						}
					}
				}
				retList.Add(board);
			}
			tr.Close();
			return retList;
			//return ReadBoardFromFile(file, '.');
		}

		/// <summary>
		/// Load Sudokus from file.
		/// </summary>
		/// <remarks>format:<br/>
		/// Header<br />
		/// 123000405<br />
		/// 000456000<br />
		/// 450008007 (etc.)
		/// </remarks>
		/// <param name="file">Path/File to textfile</param>
		/// <returns>List of Boards</returns>
		public static IList<Board> ReadBoardFromFile(string file, char zero = '0')
		{
			IList<Board> retList = new List<Board>();
			TextReader tr = new StreamReader(file);
			while (true)
			{
				tr.ReadLine();
				Board board = new Board();
				for (int y = 0; y < Consts.DimensionSquare; y++)
				{
					string line = tr.ReadLine();
					if (String.IsNullOrWhiteSpace(line))
					{
						tr.Close();
						return retList;
					}
					for (int x = 0; x < Consts.DimensionSquare; x++)
					{
						char currChar = line[x];
						if (currChar.Equals(zero))
							continue;
						int digit = Convert.ToInt32(currChar) - 48;
						SudokuLog result = board.SetDigit(y, x, digit);
						if (!result.Successful)
						{
							Debug.WriteLine(y + " " + x);
						}
					}
				}
				retList.Add(board);
			}
		}

		public static string PrintSudokuResult(SudokuLog sudokuResult)
		{
			StringBuilder sb = new StringBuilder();
			PrintSudokuResult(sudokuResult, sb, "");
			//Console.WriteLine(sb);
			return sb.ToString();
		}

		private static void PrintSudokuResult(SudokuLog sudokuResult, StringBuilder sb, string cap)
		{
			sb.Append(cap);
			sb.Append(sudokuResult.ToString());
			foreach (SudokuLog sr in sudokuResult.ChildSudokuResult)
			{
				PrintSudokuResult(sr, sb, cap + " ");
			}
		}
	}
}