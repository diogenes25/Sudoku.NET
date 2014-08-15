using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DE.Onnen.Sudoku
{
	/// <summary>
	/// Extension-Methods for IBoard.
	/// </summary>
	/// <remarks>
	///
	/// </remarks>
	public static class BoardExtensions
	{
		public static void SetCellsFromString(this IBoard board, string line, char zero = '0')
		{
			board.Clear();
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
		/// Nice Output.
		/// </summary>
		/// <remarks>
		/// &nbsp;&nbsp;123 456 789 <br/>
		/// &nbsp;┌───┬───┬───┐ <br/>
		/// A│579│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│<br/>
		/// B│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// C│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// &nbsp;├───┼───┼───┤ <br/>
		/// D│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// E│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// F│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// &nbsp;├───┼───┼───┤ <br/>
		/// G│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// H│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		/// I│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│&nbsp;&nbsp;&nbsp;│ <br/>
		///  └───┴───┴───┘ <br/>
		/// Complete: 11,1111111111111 %
		/// </remarks>
		/// <param name="board"></param>
		/// <param name="onlyGiven"></param>
		/// <returns></returns>
		public static string Matrix(this IBoard board, bool onlyGiven = false)
		{
			// ╔═╦═╗
			// ║ ║ ║
			// ╠═╬═╣
			// ╚═╩═╝
			StringBuilder sb = new StringBuilder();
			List<int> givenID = null;
			if (onlyGiven)
			{
				givenID = board.Givens.Select(x => x.ID).ToList();
			}
			int id = 0;
			sb.Append("  123 456 789");
			sb.Append(Environment.NewLine);
			sb.Append(" ┌───┬───┬───┐");
			sb.Append(Environment.NewLine);
			for (int i = 0; i < Consts.DimensionSquare; i++)
			{
				if (i > 0 && i % 3 == 0)
				{
					sb.Append(" ├───┼───┼───┤");
					sb.Append(Environment.NewLine);
				}
				IHouse house = board.GetHouse(HouseType.Row, i);
				sb.Append((char)(i + 65));
				for (int x = 0; x < house.Count; x++)
				{
					if (x % 3 == 0)
					{
						sb.Append("│");
					}
					if (onlyGiven)
					{
						sb.Append(((givenID.Contains(id)) ? board[id].Digit.ToString() : " "));
					}
					else
					{
						sb.Append(((house[x].Digit > 0) ? house[x].Digit.ToString() : " "));
					}
					id++;
				}
				sb.Append("│");
				sb.Append(Environment.NewLine);
			}
			sb.Append(" └───┴───┴───┘");
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
									string v = board[cellID].Digit.ToString();
									if (digit == 5 && board[cellID].Digit > 0)
									{
										v = board[cellID].Digit.ToString();
									}
									else
									{
										v = (board[cellID].Candidates.Contains(digit)) ? digit.ToString() : " ";
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

		public static string htmlTable(this IBoard board, bool onlyGiven = false)
		{
			StringBuilder sb = new StringBuilder();
			List<int> givenID = null;
			if (onlyGiven)
			{
				givenID = board.Givens.Select(x => x.ID).ToList();
			}
			int id = 0;
			sb.Append("<table class=\"sudokutbl\">");
			sb.Append(Environment.NewLine);
			for (int i = 0; i < Consts.DimensionSquare; i++)
			{
				sb.Append(Environment.NewLine);
				sb.Append("<tr class=\"sudokurow\">");
				sb.Append(Environment.NewLine);
				sb.Append("\t");
				IHouse house = board.GetHouse(HouseType.Row, i);
				for (int x = 0; x < house.Count; x++)
				{
					sb.Append("<td class=\"sudokucell\" id=\"cell[");
					sb.Append(id);
					sb.Append("]\" />");
					if (onlyGiven)
					{
						sb.Append(((givenID.Contains(id)) ? board[id].Digit.ToString() : "&nbsp;"));
					}
					else
					{
						sb.Append(((house[x].Digit > 0) ? house[x].Digit.ToString() : "&nbsp;"));
					}
					sb.Append("</td>");
					id++;
				}
				sb.Append(Environment.NewLine);
				sb.Append("</tr>");
			}
			sb.Append(Environment.NewLine);
			sb.Append("</table>");
			sb.Append(Environment.NewLine);
			//sb.Append("Complete: ");
			//sb.Append(board.SolvePercent);
			//sb.Append(" %");
			//sb.Append(Environment.NewLine);
			return sb.ToString();
		}
	}
}