using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DE.Onnen.Sudoku
{
	public static class SudokuHelper
	{
		private static int countCell = Consts.DimensionSquare * Consts.DimensionSquare;

		/// <summary>
		/// Read Sudokus from file.
		/// </summary>
		/// <remarks>format:<br/>
		/// 123456789123456789123456789123456789 usw
		/// </remarks>
		/// <param name="file">Path/File to textfile</param>
		/// <returns>List of Boards</returns>
		public static IList<string> ReadBoardFromFileTop(string file)
		{
			IList<string> retList = new List<string>();
			TextReader tr = new StreamReader(file);
			string line;
			while ((line = tr.ReadLine()) != null)
			{
				if (line.Length < countCell)
					continue;
				string sudokuLine = line.Replace('.', '0');
				retList.Add(sudokuLine);
			}
			tr.Close();
			return retList;
		}

		public static IList<string> ReadBoardFromFile(string file)
		{
			IList<string> retList = new List<string>();
			TextReader tr = new StreamReader(file);
			while (true)
			{
				tr.ReadLine();
				StringBuilder sb = new StringBuilder();
				for (int y = 0; y < Consts.DimensionSquare; y++)
				{
					string line = tr.ReadLine();
					if (String.IsNullOrWhiteSpace(line))
					{
						tr.Close();
						//tr.Dispose();
						return retList;
					}
					sb.Append(line);
				}
				string sudokuLine = sb.ToString().Replace('.', '0');
				retList.Add(sudokuLine);
			}
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
		//public static IList<T> ReadBoardFromFileTop<T>(string file, char zero = '0') where T :  IBoard,new()
		//{
		//    IList<T> retList = new List<T>();
		//    TextReader tr = new StreamReader(file);
		//    string line;
		//    while ((line = tr.ReadLine()) != null)
		//    {
		//        if (line.Length < countCell)
		//            continue;
		//        T board = new T();
		//        //T board = new T("..\\..\\..\\Sudoku\\SolveTechnics\\");

		//        board.SetCellsFromString(line, zero);
		//        retList.Add(board);
		//    }
		//    tr.Close();
		//    return retList;
		//    //return ReadBoardFromFile(file, '.');
		//}

		///// <summary>
		///// Load Sudokus from file.
		///// </summary>
		///// <remarks>format:<br/>
		///// Header<br />
		///// 123000405<br />
		///// 000456000<br />
		///// 450008007 (etc.)
		///// </remarks>
		///// <param name="file">Path/File to textfile</param>
		///// <param name="zero">char that marks 'No Digit'</param>
		///// <returns>List of Boards</returns>
		//public static IList<T> ReadBoardFromFileX<T>(string file, char zero = '0') where T : IBoard, new()
		//{
		//    IList<T> retList = new List<T>();
		//    TextReader tr = new StreamReader(file);
		//    while (true)
		//    {
		//        tr.ReadLine();
		//        T board = new T();
		//        //Board board = new Board("..\\..\\..\\Sudoku\\SolveTechnics\\");

		//                int cellid = 0;
		//        for (int y = 0; y < Consts.DimensionSquare; y++)
		//        {
		//            string line = tr.ReadLine();
		//            if (String.IsNullOrWhiteSpace(line))
		//            {
		//                tr.Close();
		//                //tr.Dispose();
		//                return retList;
		//            }
		//            for (int x = 0; x < Consts.DimensionSquare; x++)
		//            {
		//                char currChar = line[x];
		//                if (currChar.Equals(zero))
		//                    continue;
		//                int digit = Convert.ToInt32(currChar) - 48;

		//                SudokuLog result = board.SetDigit(cellid, digit);
		//                if (!result.Successful)
		//                {
		//                    Debug.WriteLine(y + " " + x);
		//                }
		//                cellid++;
		//            }

		//        }
		//        retList.Add(board);
		//    }
		//}

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