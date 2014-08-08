using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DE.Onnen.Sudoku
{
	public static class BoardExtensions
	{

		public static void SetCellsFromString(this IBoard board, string line, char zero = '0')
		{
			int max = Consts.DimensionSquare * Consts.DimensionSquare;
			if (line.Length < max)
				throw new Exception("string is to short"); ;
			for (int x = 0; x < max; x++)
			{
				char currChar = line[x];
				if (!currChar.Equals(zero))
				{
					int digit = Convert.ToInt32(currChar) - 48;
					SudokuLog result = board.SetDigit(x, digit);
					if (!result.Successful)
					{
						throw new Exception("Digit : " + digit + " could not be set");
					}
				}
			}
		}

		/// <summary>
		/// ╔═╦═╗
		/// ║ ║ ║
		/// ╠═╬═╣
		/// ╚═╩═╝
		/// </summary>
		/// <param name="board"></param>
		public static string Matrix(this IBoard board, bool onlyGiven = false)
		{
			StringBuilder sb = new StringBuilder();
			List<int> givenID = null;
			if (onlyGiven)
			{
				givenID = board.Givens.Select(x => x.ID).ToList();
			}
			int id = 0;
			sb.Append("┌───┬───┬───┐");
			sb.Append(Environment.NewLine);
			for (int i = 0; i < Consts.DimensionSquare; i++)
			{
				if (i > 0 && i % 3 == 0)
				{
					sb.Append("├───┼───┼───┤");
					sb.Append(Environment.NewLine);
				}
				IHouse house = board.GetHouse(HouseType.Row, i);
				for (int x = 0; x < house.Peers.Length; x++)
				{
					if (x % 3 == 0)
					{
						sb.Append("│");
					}
					if (onlyGiven)
					{
						sb.Append(((givenID.Contains(id)) ? board.Cells[id].Digit.ToString() : " "));
					}
					else
					{
						sb.Append(((house.Peers[x].Digit > 0) ? house.Peers[x].Digit.ToString() : " "));
					}
					id++;
				}
				sb.Append("│");
				sb.Append(Environment.NewLine);
			}
			sb.Append("└───┴───┴───┘");
			sb.Append(Environment.NewLine);
			sb.Append("Complete: ");
			sb.Append(board.SolvePercent);
			sb.Append(" %");
			sb.Append(Environment.NewLine);
			return sb.ToString();
		}

		public static string MatrixWithCandidates(this IBoard board)
		{
			StringBuilder sb = new StringBuilder();
			//sb.Append("┌─────────────┬─────────────┬─────────────┐");
			//sb.Append(Environment.NewLine);
			int LineID = 0;
			for (int boxX = 0; boxX < 3; boxX++)
			{
				sb.Append("┌───┬───┬───┐┌───┬───┬───┐┌───┬───┬───┐");
				sb.Append(Environment.NewLine);
				for (int boxY = 0; boxY < 3; boxY++)
				{
					int cellIDY = 0;
					for (int l = 0, m = (Consts.Dimension); l < m; l++)
					{
						//sb.Append("│");
						for (int line = 0, maxline = (Consts.Dimension); line < maxline; line++)
						{
							sb.Append("│");
							for (int partX = 0, maxpartX = (Consts.Dimension); partX < maxpartX; partX++)
							{
								for (int part = 0, maxpart = (Consts.Dimension); part < maxpart; part++)
								{
									int digit = part + (cellIDY * 3) + 1;
									int cellID = (LineID * Consts.DimensionSquare) + partX + (line * 3);
									string v = board.Cells[cellID].Digit.ToString();
									if (digit == 5 && board.Cells[cellID].Digit > 0)
									{
										v = board.Cells[cellID].Digit.ToString();
									}
									else
									{
										v = (board.Cells[cellID].Candidates.Contains(digit)) ? digit.ToString() : " ";
									}
									sb.Append(v);
								}
								sb.Append("│");
							}
							//sb.Append("│");
						}
						//sb.Append("");
						sb.Append(Environment.NewLine);
						cellIDY++;
					}
					LineID++;
					if (boxY < 2)
					{
						sb.Append("├───┼───┼───┤├───┼───┼───┤├───┼───┼───┤");
						sb.Append(Environment.NewLine);
					}
					else
					{
						sb.Append("└───┴───┴───┘└───┴───┴───┘└───┴───┴───┘");
						sb.Append(Environment.NewLine);
					}
				}
				//if (boxX < 3)
				//{
				//	//sb.Append("├─────────────┼─────────────┼─────────────┤");
				//}
				//else
				//{
				//	sb.Append("└───┴───┴───┘└───┴───┴───┘└───┴───┴───┘");
				//}
			}
			//sb.Append("└─────────────┴─────────────┴─────────────┘");
			sb.Append(Environment.NewLine);
			sb.Append("Complete: ");
			sb.Append(board.SolvePercent);
			sb.Append(" %");
			sb.Append(Environment.NewLine);
			return sb.ToString();
		}
	}
}
