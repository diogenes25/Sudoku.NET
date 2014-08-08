using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace DE.Onnen.Sudoku
{
	public static class SudokuHelper
	{

		private static int countCell = Consts.DimensionSquare * Consts.DimensionSquare;

		/// <summary>
		/// Convert a Board to a int-Array
		/// </summary>
		/// <remarks>
		/// Positiv value = Candidates as bitmask.<br/>
		/// Negativ value = Digit.
		/// </remarks>
		/// <param name="board"></param>
		/// <returns></returns>
		public static int[] CreateSimpleBoard(Board board)
		{
			if (board == null || board.Cells == null || board.Cells.Length < 1)
			{
				return null;
			}

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
		public static IList<Board> ReadBoardFromFileTop(string file, char zero = '0')
		{
			IList<Board> retList = new List<Board>();
			TextReader tr = new StreamReader(file);
			string line;
			while ((line = tr.ReadLine()) != null)
			{
				if (line.Length < countCell)
					continue;
				//Board board = new Board();
				Board board = new Board("..\\..\\..\\Sudoku\\SolveTechnics\\");
			
				board.SetCellsFromString(line, zero);
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
		/// <param name="zero">char that marks 'No Digit'</param>
		/// <returns>List of Boards</returns>
		public static IList<Board> ReadBoardFromFile(string file, char zero = '0')
		{
			IList<Board> retList = new List<Board>();
			TextReader tr = new StreamReader(file);
			while (true)
			{
				tr.ReadLine();
				Board board = new Board("..\\..\\..\\Sudoku\\SolveTechnics\\");
				for (int y = 0; y < Consts.DimensionSquare; y++)
				{
					string line = tr.ReadLine();
					if (String.IsNullOrWhiteSpace(line))
					{
						tr.Close();
						//tr.Dispose();
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