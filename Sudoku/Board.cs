using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechniques;

namespace de.onnen.Sudoku
{
	/// <summary>
	/// https://github.com/diogenes25/Sudoku
	/// http://diogenes25.github.io/Sudoku/
	/// http://sudoku-solutions.com/
	/// http://hodoku.sourceforge.net/de/index.php
	/// http://forum.enjoysudoku.com/
	/// http://www.sadmansoftware.com/sudoku/solvingtechniques.htm
	/// http://www.setbb.com/phpbb/viewtopic.php?t=379&mforum=sudoku
	/// http://www.playr.co.uk/sudoku/dictionary.php
	/// http://www.sudocue.net/glossary.php
	/// </summary>
	public class Board : ICloneable, IBoard, ISudokuHost
	{
		public List<SudokuHistoryItem> History { get { return history; } }

		public List<Cell> Givens { get; private set; }

		private const int ROW_CONTAINERTYPE = 0;
		private const int COL_CONTAINERTYPE = 1;
		private const int BLOCK_CONTAINERTYPE = 2;
		private List<SudokuHistoryItem> history;
		private Cell[] cells = new Cell[Consts.DimensionSquare * Consts.DimensionSquare];
		private House[][] container = new House[Consts.DimensionSquare][];
		private double solvePercentBase = 0;
		private IList<ASolveTechnique> solveTechniques = new List<ASolveTechnique>();

		public IList<ISolveTechnique> SolveTechniques { get { return this.solveTechniques.Select(x => (ISolveTechnique)x).ToList<ISolveTechnique>(); } }

		public IHouse GetHouse(HouseType houseType, int idx)
		{
			return this.container[idx][(int)houseType];
		}

		public delegate void BoardChanged(IBoard board, SudokuEvent sudokuEvent);

		public event BoardChanged boardChangeEvent;

		public double SolvePercent()
		{
			double currSolvePercent = 0;
			foreach (Cell c in cells)
			{
				currSolvePercent += (Consts.DimensionSquare - c.Candidates.Count);
			}

			return (currSolvePercent / this.solvePercentBase) * 100;
		}

		public bool IsComplete
		{
			get
			{
				bool ready = true;
				for (int containerType = 0; containerType < 3; containerType++)
				{
					for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
					{
						ready &= container[containerIdx][containerType].Complete;
					}
				}
				return ready;
			}
		}

		/// <summary>
		/// Sämtliche Zellen
		/// </summary>
		public ICell[] Cells
		{
			get { return this.cells; }
		}

		/// <summary>
		///
		/// </summary>
		internal void ReCheck()
		{
			this.reCheck = true;
		}

		private bool reCheck = false;
		private string filepath;

		public Board()
			: this(null)
		{
		}

		public Board(string filepath)
		{
			this.filepath = filepath;
			this.Givens = new List<Cell>();
			this.history = new List<SudokuHistoryItem>();
			this.solvePercentBase = Math.Pow(Consts.DimensionSquare, 3.0);
			for (int i = 0; i < Consts.DimensionSquare * Consts.DimensionSquare; i++)
			{
				cells[i] = new Cell(this, i);
			}

			Cell[][][] fieldcontainer;
			fieldcontainer = new Cell[3][][];
			fieldcontainer[ROW_CONTAINERTYPE] = new Cell[Consts.DimensionSquare][]; // Row
			fieldcontainer[COL_CONTAINERTYPE] = new Cell[Consts.DimensionSquare][]; // Col
			fieldcontainer[BLOCK_CONTAINERTYPE] = new Cell[Consts.DimensionSquare][]; // Block

			for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
			{
				fieldcontainer[ROW_CONTAINERTYPE][containerIdx] = new Cell[Consts.DimensionSquare];
				fieldcontainer[COL_CONTAINERTYPE][containerIdx] = new Cell[Consts.DimensionSquare];
				fieldcontainer[BLOCK_CONTAINERTYPE][containerIdx] = new Cell[Consts.DimensionSquare];

				for (int t = 0; t < Consts.DimensionSquare; t++)
				{
					// Row 0,1,2,3,4,5,6,7,8
					fieldcontainer[ROW_CONTAINERTYPE][containerIdx][t] = cells[t + (containerIdx * Consts.DimensionSquare)];
					// Col 0,9,18,27,36
					fieldcontainer[COL_CONTAINERTYPE][containerIdx][t] = cells[(t * Consts.DimensionSquare) + containerIdx];
				}

				// Block 0,1,2, 9,10,11, 18,19,20
				int blockCounter = 0;
				for (int zr = 0; zr < Consts.Dimension; zr++)
				{
					for (int zc = 0; zc < Consts.Dimension; zc++)
					{
						int b = (containerIdx * Consts.Dimension) + (zc + (zr * Consts.DimensionSquare)) + ((containerIdx / Consts.Dimension) * Consts.DimensionSquare * 2);
						fieldcontainer[BLOCK_CONTAINERTYPE][containerIdx][blockCounter++] = cells[b];
					}
				}

				container[containerIdx] = new House[3];
				for (int containerType = 0; containerType < 3; containerType++)
				{
					container[containerIdx][containerType] = new House(this, fieldcontainer[containerType][containerIdx], (HouseType)containerType, containerIdx);
					foreach (Cell f in fieldcontainer[containerType][containerIdx])
					{
						f.Fieldcontainters[containerType] = container[containerIdx][containerType];
					}
				}
			}

			LoadSolveTechnics();
		}

		/// <summary>
		/// Set a digit at cell.
		/// </summary>
		/// <param name="row">Row</param>
		/// <param name="col">Column</param>
		/// <param name="digit">Digit</param>
		public SudokuLog SetDigit(int row, int col, int digit)
		{
			int cellid = (row * 9) + col;
			return this.SetDigit(cellid, digit);
		}

		/// <summary>
		/// Set a digit at cell.
		/// </summary>
		/// <param name="cell">ID of cell</param>
		/// <param name="digit">Digit</param>
		public SudokuLog SetDigit(int cellid, int digit, bool withSolve = false)
		{
			SudokuLog sudokuResult = new SudokuLog();
			sudokuResult.EventInfoInResult = new SudokuEvent()
			{
				ChangedCellBase = null,
				Action = CellAction.SetDigitInt,
				SolveTechnik = "SetDigit",
			};

			if (cellid < 0 || cellid > cells.Length)
			{
				sudokuResult.Successful = false;
				sudokuResult.ErrorMessage = "Cell " + cellid + " is not in range";
				return sudokuResult;
			}
			sudokuResult.EventInfoInResult.ChangedCellBase = this.cells[cellid];

			this.cells[cellid].SetDigit(digit, sudokuResult);
			if (sudokuResult.Successful)
			{
				this.Givens.Add(this.cells[cellid]);
				if (withSolve)
				{
					this.Solve(sudokuResult);
				}
			}

			if (sudokuResult.Successful)
			{
				this.history.Add(new SudokuHistoryItem(this, this.cells[cellid], sudokuResult));
			}
			else
			{
				SetHistory(this.history.Count - 1);
				this.cells[cellid].RemovePossibleDigit(digit, sudokuResult);
				//if (withSolve)
				//{
				//    this.Solve(sudokuResult);
				//}
			}
			return sudokuResult;
		}

		public void SetBoard(IBoard otherBoard)
		{
			for (int i = 0; i < otherBoard.Cells.Length; i++)
			{
				this.cells[i].digit = otherBoard.Cells[i].Digit;
				this.cells[i].baseValue = otherBoard.Cells[i].BaseValue;
			}
			for (int containerType = 0; containerType < 3; containerType++)
			{
				for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
				{
					this.container[containerIdx][containerType].RecalcBaseValue();
				}
			}
		}

		public void SetHistory(int historyId)
		{
			if (historyId < 0 || historyId >= this.history.Count)
				return;
			for (int i = 0; i < this.history[historyId].BoardInt.Length; i++)
			{
				if (this.history[historyId].BoardInt[i] < 0)
				{
					this.cells[i].digit = this.history[historyId].BoardInt[i] * -1;
					this.cells[i].baseValue = 0;
				}
				else
				{
					this.cells[i].baseValue = this.history[historyId].BoardInt[i];
					this.cells[i].digit = 0;
				}
			}
			for (int containerType = 0; containerType < 3; containerType++)
			{
				for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
				{
					this.container[containerIdx][containerType].RecalcBaseValue();
				}
			}
			//this.Solve(new SudokuResult());
		}

		public void Solve(SudokuLog sudokuResult, bool forceSolve = false)
		{
			do
			{
				this.reCheck = false;
				for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
				{
					for (int containerType = 0; containerType < 3; containerType++)
					{
						//if (container[containerIdx][containerType].Complete || (!container[containerIdx][containerType].ReCheck && !forceSolve))
						if (container[containerIdx][containerType].Complete)
							continue;
						foreach (ASolveTechnique st in solveTechniques)
						{
							if (st.IsActive)
							{
								st.SolveHouse(container[containerIdx][containerType], sudokuResult);
								if (!sudokuResult.Successful)
									return;
							}
						}
						container[containerIdx][containerType].ReCheck = false;
					}
				}
			} while (this.reCheck);
		}

		public void Backtracking()
		{
			this.Backtracking(new SudokuLog());
		}

		public void Backtracking(SudokuLog sudokuResult)
		{
			if (!BacktrackingContinue((Board)this.Clone()))
			{
				sudokuResult.Successful = false;
				sudokuResult.ErrorMessage = "Sudoku hat keine Lösung";
			}
		}

		public void Reset()
		{
			this.Givens.Clear();
			this.history.Clear();
			for (int i = 0; i < Consts.DimensionSquare * Consts.DimensionSquare; i++)
			{
				cells[i].digit = 0;
				cells[i].baseValue = (1 << Consts.DimensionSquare) - 1;
			}

			for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
			{
				for (int containerType = 0; containerType < 3; containerType++)
				{
					container[containerIdx][containerType].baseValue = (1 << Consts.DimensionSquare) - 1;
				}
			}
		}

		private bool BacktrackingContinue(Board board)
		{
			if (board.IsComplete)
				return true;

			for (int i = 0; i < cells.Length; i++)
			{
				if (board.Cells[i].Digit == 0)
				{
					List<int> posDigit = board.Cells[i].Candidates;
					foreach (int x in posDigit)
					{
						Board newBoard = (Board)board.Clone();
						SudokuLog result = newBoard.SetDigit(i, x, true);
						if (boardChangeEvent != null)
							boardChangeEvent(newBoard, new SudokuEvent() { Action = CellAction.SetDigitInt, ChangedCellBase = newBoard.Cells[i] });
						Thread.Sleep(300);
						if (!result.Successful)
						{
							//Console.WriteLine(board.Cells[i].ToString() + " X SetDigit " + x);
							continue;
						}
						if (newBoard.IsComplete)
						{
							//newBoard.Show();
							for (int s = 0; s < cells.Length; s++)
							{
								this.cells[s].digit = newBoard.cells[s].Digit;
								this.cells[s].baseValue = 0;
							}
							return true;
						}
						if (BacktrackingContinue(newBoard))
						{
							//Console.WriteLine("OK " + board.Cells[i].ToString() + " : " + x);
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		public void Show(bool withPos = false)
		{
			for (int x = 0; x < cells.Length; x++)
			{
				int row = (x / Consts.DimensionSquare);
				int col = (x % Consts.DimensionSquare);

				if ((x % Consts.DimensionSquare) == 0)
				{
					if (row % 3 == 0)
						Console.WriteLine("\n----------------------------------------------------------------");
					else
					{
						Console.WriteLine("\n");
					}
				}
				if (this.cells[x].Digit > 0)
				{
					Console.Write("[ {0} ]", this.cells[x].Digit);
				}
				else
				{
					List<int> posDigit = this.cells[x].Candidates; ;
					if (withPos)
					{
						Console.Write("(");
						foreach (int pn in posDigit)
						{
							Console.Write(pn);
						}
						Console.Write(")");
					}
					else
						Console.Write("- {0} -", posDigit.Count);
					//Console.Write(this.cells[x].BaseValue + " | ");
				}
				if ((col + 1) % 3 == 0)
					Console.Write(" | ");
			}
			Console.WriteLine("\n---------- Complete: " + this.IsComplete + " - " + this.SolvePercent() + " % ----------------------");
		}

		#region ICloneable Members

		public object Clone()
		{
			Board cloneboard = new Board();
			for (int i = 0; i < cells.Length; i++)
			{
				cloneboard.cells[i].digit = cells[i].Digit;
				cloneboard.cells[i].baseValue = cells[i].BaseValue;
			}
			for (int containerType = 0; containerType < 3; containerType++)
			{
				for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
				{
					cloneboard.container[containerIdx][containerType].RecalcBaseValue();
				}
			}
			return cloneboard;
		}

		#endregion ICloneable Members

		private void LoadSolveTechnics()
		{
			//"d:\\Develop\\SolveTechniques"
			if (String.IsNullOrWhiteSpace(this.filepath))
				return;

			List<string> files = new List<string>(Directory.GetFiles(this.filepath, "*.dll"));
			foreach (string file in files)
			{
				ASolveTechnique st = SudokuSolveTechniqueLoader.LoadSolveTechnic(file, this);
				solveTechniques.Add(st);
				st.SetBoard(this);
			}
		}

		#region ISudokuHost Members

		public void Register(ISolveTechnique solveTechnic)
		{
		}

		#endregion ISudokuHost Members
	}
}