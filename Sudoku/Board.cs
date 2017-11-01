using DE.Onnen.Sudoku.SolveTechniques;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DE.Onnen.Sudoku
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
	/// http://walter.bislins.ch/projekte/index.asp?page=Sudoku
	/// </summary>
	public class Board : ACellCollection, ICloneable, IBoard, ISudokuHost
	{
		private const int ROW_CONTAINERTYPE = 0;
		private const int COL_CONTAINERTYPE = 1;
		private const int BLOCK_CONTAINERTYPE = 2;
		private List<SudokuHistoryItem> history;

		//private List<ICell> givens;
		private HouseCollection[][] container = new HouseCollection[Consts.DimensionSquare][];

		private double solvePercentBase = 0;
		private IList<ASolveTechnique> solveTechniques = new List<ASolveTechnique>();

		public ReadOnlyCollection<SudokuHistoryItem> History { get { return this.history.AsReadOnly(); } }

		/// <inheritdoc />
		public ReadOnlyCollection<ICell> Givens { get { return this.cells.Where(x => x.IsGiven).Select(x => (ICell)x).ToList().AsReadOnly(); } }

		/// <summary>
		/// Loaded solvetechniques.
		/// </summary>
		public IList<ISolveTechnique> SolveTechniques { get { return this.solveTechniques.Select(x => (ISolveTechnique)x).ToList<ISolveTechnique>(); } }

		public IHouse GetHouse(HouseType houseType, int idx)
		{
			return this.container[idx][(int)houseType];
		}

		/// <summary>
		/// Changes in board
		/// </summary>
		public event System.EventHandler<SudokuEvent> BoardChangeEvent;

		/// <summary>
		/// Percent
		/// </summary>
		/// <returns>Percent</returns>
		public double SolvePercent
		{
			get
			{
				double currSolvePercent = 0;
				foreach (Cell c in cells)
				{
					currSolvePercent += (Consts.DimensionSquare - c.Candidates.Count);
				}

				return (currSolvePercent / this.solvePercentBase) * 100;
			}
		}

		/// <inheritdoc />
		public bool IsComplete
		{
			get
			{
				//bool ready = true;
				//for (int containerType = 0; containerType < 3; containerType++)
				//{
				int containerType = 0;
				for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
				{
					//ready = this.container[containerIdx][containerType].Complete;
					if (!this.container[containerIdx][containerType].IsComplete)
						return false;
				}
				//}
				//return ready;
				return true;
			}
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
			if (solveTechniques != null && solveTechniques.Count > 0)
			{
				foreach (ASolveTechnique st in this.solveTechniques)
				{
					st.SetBoard(this);
				}
			}
		}

		public Board()
		//: this("..\\..\\..\\Sudoku\\SolveTechnics\\")
		{
			Init();
		}

		public Board(string filePath)
		{
			Init();
			LoadSolveTechniques(filePath);
		}

		private void Init()
		{
			this.cells = new Cell[Consts.DimensionSquare * Consts.DimensionSquare];
			//this.givens = new List<ICell>();
			this.history = new List<SudokuHistoryItem>();
			this.solvePercentBase = Math.Pow(Consts.DimensionSquare, 3.0);
			for (int i = 0; i < Consts.DimensionSquare * Consts.DimensionSquare; i++)
			{
				cells[i] = new Cell(i);
				cells[i].PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Cell_PropertyChanged);
			}

			ICell[][][] fieldcontainer;
			fieldcontainer = new ICell[3][][];
			fieldcontainer[ROW_CONTAINERTYPE] = new ICell[Consts.DimensionSquare][]; // Row
			fieldcontainer[COL_CONTAINERTYPE] = new ICell[Consts.DimensionSquare][]; // Col
			fieldcontainer[BLOCK_CONTAINERTYPE] = new ICell[Consts.DimensionSquare][]; // Block

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

				this.container[containerIdx] = new HouseCollection[3];
				for (int containerType = 0; containerType < 3; containerType++)
				{
					this.container[containerIdx][containerType] = new HouseCollection(fieldcontainer[containerType][containerIdx], (HouseType)containerType, containerIdx);
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
			int cellid = 0;
			checked
			{
				cellid = (row * 9) + col;
			}
			return this.SetDigit(cellid, digit);
			//else
			//{
			//	return new SudokuLog()
			//	{
			//		ErrorMessage = "Overflow",
			//		Successful = false,
			//	};
			//}
		}

		public SudokuLog SetDigit(int cellID, int digitToSet)
		{
			return this.SetDigit(cellID, digitToSet, false);
		}

		/// <summary>
		/// Set a digit at cell.
		/// </summary>
		/// <param name="cellID">ID of cell</param>
		/// <param name="digitToSet">Set Digit to Cell</param>
		/// <param name="withSolve">true = Start solving with every solvetechnique (without backtrack) after digit was set.</param>
		public SudokuLog SetDigit(int cellID, int digitToSet, bool withSolve)
		{
			SudokuLog sudokuResult = new SudokuLog();
			sudokuResult.EventInfoInResult = new SudokuEvent()
			{
				ChangedCellBase = null,
				Action = CellAction.SetDigitInt,
				SolveTechnik = "SetDigit",
			};

			if (cellID < 0 || cellID > cells.Length)
			{
				sudokuResult.Successful = false;
				sudokuResult.ErrorMessage = "Cell " + cellID + " is not in range";
				return sudokuResult;
			}
			sudokuResult.EventInfoInResult.ChangedCellBase = this.cells[cellID];

			this.cells[cellID].SetDigit(digitToSet, sudokuResult);
			if (sudokuResult.Successful)
			{
				this.cells[cellID].IsGiven = true;
				//this.givens.Add(this.cells[cellID]);
				if (withSolve)
				{
					this.Solve(sudokuResult);
				}
			}

			if (sudokuResult.Successful)
			{
				this.history.Add(new SudokuHistoryItem(this, this.cells[cellID], sudokuResult));
			}
			else
			{
				SetHistory(this.history.Count - 1);
				this.cells[cellID].RemoveCandidate(digitToSet, sudokuResult);
				//if (withSolve)
				//{
				//    this.Solve(sudokuResult);
				//}
			}
			return sudokuResult;
		}

		/// <summary>
		/// Set Cells from otherBoard to this Board.
		/// </summary>
		/// <param name="otherBoard">Another Board</param>
		public void SetBoard(IBoard otherBoard)
		{
			if (otherBoard == null || otherBoard == null)
			{
				return;
			}

			for (int i = 0; i < otherBoard.Count; i++)
			{
				if (otherBoard[i].Digit > 0)
				{
					this.cells[i].Digit = otherBoard[i].Digit;
				}
				else
				{
					this.cells[i].CandidateValue = otherBoard[i].CandidateValue;
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
					catch
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

		/// <summary>
		/// Solves Sudoku with SolveTechniques (no Backtracking).
		/// </summary>
		/// <param name="sudokuResult">Log</param>
		public void Solve(SudokuLog sudokuResult)
		{
			if (sudokuResult == null)
			{
				sudokuResult = new SudokuLog();
			}

			do
			{
				this.reCheck = false;
				for (int containerIdx = 0; containerIdx < Consts.DimensionSquare; containerIdx++)
				{
					for (int containerType = 0; containerType < 3; containerType++)
					{
						if (container[containerIdx][containerType].IsComplete)
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
			if (sudokuResult == null)
			{
				sudokuResult = new SudokuLog()
				{
					Successful = false,
					ErrorMessage = "SudokuLog is null"
				};
			}
			else if (!BacktrackingContinue((Board)this.Clone()))
			{
				sudokuResult.Successful = false;
				sudokuResult.ErrorMessage = "Sudoku hat keine Lösung";
			}
		}

		/// <summary>
		/// Reset Board.
		/// </summary>
		public void Clear()
		{
			//this.givens.Clear();
			this.history.Clear();
			base.Clear();
			//for (int i = 0; i < Consts.DimensionSquare * Consts.DimensionSquare; i++)
			//{
			//	cells[i].Digit = 0;
			//}

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
				if (board[i].Digit == 0)
				{
					ReadOnlyCollection<int> posDigit = board[i].Candidates;
					foreach (int x in posDigit)
					//System.Threading.Tasks.Parallel.ForEach(posDigit, x=>
					{
						Board newBoard = (Board)board.Clone();
						SudokuLog result = newBoard.SetDigit(i, x, true);
						if (BoardChangeEvent != null)
							BoardChangeEvent(newBoard, new SudokuEvent() { Action = CellAction.SetDigitInt, ChangedCellBase = newBoard[i] });
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

		private void LoadSolveTechniques(string filePath)
		{
			//"d:\\Develop\\SolveTechniques"
			if (String.IsNullOrWhiteSpace(filePath))
				return;

			List<string> files = new List<string>(Directory.GetFiles(filePath, "*.dll"));
			foreach (string file in files)
			{
				ASolveTechnique st = SudokuSolveTechniqueLoader.LoadSolveTechnic(file, this);
				this.solveTechniques.Add(st);
				st.SetBoard(this);
			}
		}

		#region ISudokuHost Members

		public void Register(ISolveTechnique solveTechnic)
		{
		}

		#endregion ISudokuHost Members

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (ICell cell in this)
			{
				sb.Append(cell.Digit);
			}
			return sb.ToString();
		}

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
		public static int[] CreateSimpleBoard(IBoard board)
		{
			if (board == null || board.Count < 1)
			{
				return null;
			}

			int[] retLst = new int[countCell];
			for (int i = 0; i < board.Count; i++)
			{
				if (board[i].Digit > 0)
				{
					retLst[i] = board[i].Digit * -1;
				}
				else
				{
					retLst[i] = board[i].CandidateValue;
				}
			}
			return retLst;
		}

		public override bool Equals(object obj)
		{
			bool retVal = false;
			if (obj != null && obj is IBoard)
			{
				IBoard nb = (IBoard)obj;
				retVal = true;
				for (int i = 0; i < Consts.DimensionSquare; i++)
				{
					retVal &= this[i].Digit == nb[i].Digit;
				}
			}
			return retVal;
		}

		public override int GetHashCode()
		{
			int retHash = 0;
			foreach (ICell c in this)
			{
				retHash += (c.Digit + (1 << (c.ID % 9)));
			}
			return retHash;
		}
	}
}