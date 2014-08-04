using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		private const int ROW_CONTAINERTYPE = 0;
		private const int COL_CONTAINERTYPE = 1;
		private const int BLOCK_CONTAINERTYPE = 2;
		private List<SudokuHistoryItem> history;
		private Cell[] cells = new Cell[Consts.DimensionSquare * Consts.DimensionSquare];
		private House[][] container = new House[Consts.DimensionSquare][];
		private double solvePercentBase = 0;
		private IList<ASolveTechnique> solveTechniques = new List<ASolveTechnique>();

		public List<SudokuHistoryItem> History { get { return history; } }

		/// <summary>
		/// Cells where Digits where set by SetDigit.
		/// </summary>
		public List<Cell> Givens { get; private set; }

		/// <summary>
		/// Loaded solvetechniques.
		/// </summary>
		public IList<ISolveTechnique> SolveTechniques { get { return this.solveTechniques.Select(x => (ISolveTechnique)x).ToList<ISolveTechnique>(); } }


		public IHouse GetHouse(HouseType houseType, int idx)
		{
			return this.container[idx][(int)houseType];
		}

		public delegate void BoardChanged(IBoard board, SudokuEvent sudokuEvent);

		public event BoardChanged boardChangeEvent;

		/// <summary>
		/// Percent
		/// </summary>
		/// <returns>Percent</returns>
		public double SolvePercent()
		{
			double currSolvePercent = 0;
			foreach (Cell c in cells)
			{
				currSolvePercent += (Consts.DimensionSquare - c.Candidates.Count);
			}

			return (currSolvePercent / this.solvePercentBase) * 100;
		}

		/// <inheritdoc />
		public bool IsComplete
		{
			get
			{
				bool ready = true;
				for (int containerType = 0; containerType < 3; containerType++)
				{
					for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
					{
						ready &= this.container[containerIdx][containerType].Complete;
					}
				}
				return ready;
			}
		}


		/// <inheritdoc />
		public ICell[] Cells
		{
			get { return this.cells; }
		}

		/// <summary>
		/// Some changes happend while solving. Another check is needed.
		/// </summary>
		internal void ReCheck()
		{
			this.reCheck = true;
		}

		private bool reCheck = false;

		public Board(IList<ASolveTechnique> solveTechniques)
		{
			Init();
			this.solveTechniques = solveTechniques;
			foreach (ASolveTechnique st in this.solveTechniques)
			{
				st.SetBoard(this);
			}
		}

		public Board()
			: this("..\\..\\SolveTechnics\\")
		{
		}

		public Board(string filepath)
		{
			Init();
			LoadSolveTechnics(filepath);
		}

		private void Init()
		{
			this.Givens = new List<Cell>();
			this.history = new List<SudokuHistoryItem>();
			this.solvePercentBase = Math.Pow(Consts.DimensionSquare, 3.0);
			for (int i = 0; i < Consts.DimensionSquare * Consts.DimensionSquare; i++)
			{
				cells[i] = new Cell(i);
				cells[i].PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Cell_PropertyChanged);
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

				this.container[containerIdx] = new House[3];
				for (int containerType = 0; containerType < 3; containerType++)
				{
					this.container[containerIdx][containerType] = new House(fieldcontainer[containerType][containerIdx], (HouseType)containerType, containerIdx);
					foreach (Cell f in fieldcontainer[containerType][containerIdx])
					{
						f.fieldcontainters[containerType] = container[containerIdx][containerType];
					}
				}
			}
		}

		private void Cell_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.ReCheck();
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

		public SudokuLog SetDigit(int cellid, int digit)
		{
			return this.SetDigit(cellid, digit, false);
		}

		/// <summary>
		/// Set a digit at cell.
		/// </summary>
		/// <param name="cellid">ID of cell</param>
		/// <param name="digit">Set Digit to Cell</param>
		/// <param name="withSolve">true = Start solving with every solvetechnique (without traceback) after digit was set.</param>
		public SudokuLog SetDigit(int cellid, int digit, bool withSolve)
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
				this.cells[cellid].RemoveCandidate(digit, sudokuResult);
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
				if (otherBoard.Cells[i].Digit > 0)
				{
					this.cells[i].Digit = otherBoard.Cells[i].Digit;
				}
				else
				{
					this.cells[i].CandidateValue = otherBoard.Cells[i].CandidateValue;
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
					try
					{
						this.cells[i].Digit = this.history[historyId].BoardInt[i] * -1;
					}
					catch (Exception ex)
					{
						//Console.WriteLine(ex.Message);
						continue;
					}
				}
				else
				{
					this.cells[i].CandidateValue = this.history[historyId].BoardInt[i];
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
						if (container[containerIdx][containerType].Complete)
							continue;
						foreach (ASolveTechnique st in solveTechniques)
						{
							if (st.IsActive)
							{
								if (!container[containerIdx][containerType].ReCheck && st.CellView == ECellView.OnlyHouse)
									continue;
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

		/// <summary>
		/// Reset Board.
		/// </summary>
		public void Reset()
		{
			this.Givens.Clear();
			this.history.Clear();
			for (int i = 0; i < Consts.DimensionSquare * Consts.DimensionSquare; i++)
			{
				cells[i].Digit = 0;
			}

			for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
			{
				for (int containerType = 0; containerType < 3; containerType++)
				{
					this.container[containerIdx][containerType].CandidateValue = (1 << Consts.DimensionSquare) - 1;
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
					//System.Threading.Tasks.Parallel.ForEach(posDigit, x=>
					{
						Board newBoard = (Board)board.Clone();
						SudokuLog result = newBoard.SetDigit(i, x, true);
						if (boardChangeEvent != null)
							boardChangeEvent(newBoard, new SudokuEvent() { Action = CellAction.SetDigitInt, ChangedCellBase = newBoard.Cells[i] });
						//Thread.Sleep(300);
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
								this.cells[s].Digit = newBoard.cells[s].Digit;
								//this.cells[s].BaseValue = 0;
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

		/// <summary>
		/// Clone Board.
		/// </summary>
		/// <returns>copy of board</returns>
		public object Clone()
		{
			//Board cloneboard = new Board(this.solveTechniques.Where(x => !x.Info.Caption.Equals("LockedCandidates")).ToList());
			Board cloneboard = new Board(this.solveTechniques);

			for (int i = 0; i < cells.Length; i++)
			{
				if (cells[i].Digit > 0)
					cloneboard.cells[i].Digit = cells[i].Digit;
				else
					cloneboard.cells[i].CandidateValue = cells[i].CandidateValue;
			}

			return cloneboard;
		}

		#endregion ICloneable Members

		private void LoadSolveTechnics(string filepath)
		{
			//"d:\\Develop\\SolveTechniques"
			if (String.IsNullOrWhiteSpace(filepath))
				return;

			List<string> files = new List<string>(Directory.GetFiles(filepath, "*.dll"));
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