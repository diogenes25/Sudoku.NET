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
		public static int[] CreateSimpleBoard(Board board)
		{
			int[] retLst = new int[81];
			for (int i = 0; i < board.Cells.Length; i++)
			{
				if (board.Cells[i].Digit > 0)
				{
					retLst[i] = board.Cells[i].Digit * -1;
				}
				else
				{
					retLst[i] = board.Cells[i].BaseValue;
				}
			}
			return retLst;
		}

		public static IList<Board> ReadBoardFromFileTop(string file)
		{
			IList<Board> retList = new List<Board>();
			TextReader tr = new StreamReader(file);
			string line;
			while ((line = tr.ReadLine()) != null)
			{
				if (line.Length < 81)
					continue;
				Board board = new Board();
				for (int x = 0; x < 81; x++)
				{
					char currChar = line[x];
					if (!currChar.Equals('.'))
					{
						SudokuResult result = board.SetDigit(x, Convert.ToInt32(currChar) - 48);
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
		}

		public static IList<Board> ReadBoardFromFile(string file)
		{
			IList<Board> retList = new List<Board>();
			TextReader tr = new StreamReader(file);
			while (true)
			{
				tr.ReadLine();
				Board board = new Board();
				for (int y = 0; y < 9; y++)
				{
					string line = tr.ReadLine();
					if (line == null)
					{
						tr.Close();
						return retList;
					}
					for (int x = 0; x < 9; x++)
					{
						char currChar = line[x];
						if (currChar.Equals('0'))
							continue;
						SudokuResult result = board.SetDigit(y - 1, x, Convert.ToInt32(currChar) - 48);
						if (!result.Successful)
						{
							Debug.WriteLine(y + " " + x);
						}
					}
				}
				retList.Add(board);
			}
		}

		public static string PrintSudokuResult(SudokuResult sudokuResult)
		{
			StringBuilder sb = new StringBuilder();
			PrintSudokuResult(sudokuResult, sb, "");
			//Console.WriteLine(sb);
			return sb.ToString();
		}

		private static void PrintSudokuResult(SudokuResult sudokuResult, StringBuilder sb, string cap)
		{
			sb.Append(cap);
			sb.Append(sudokuResult.ToString());
			foreach (SudokuResult sr in sudokuResult.ChildSudokuResult)
			{
				PrintSudokuResult(sr, sb, cap + " ");
			}
		}
	}
}