namespace de.onnen.Sudoku.SudokuExternal.SolveTechnics
{
	public abstract class ASolveTechnic : ISolveTechnic
	{
		private bool isActive = true;
		private ISudokuHost host;
		protected IBoard board;

		public ASolveTechnic()
		{
			this.Info = new SolveTechnicInfo()
			{
				Caption = "Not Defined"
			};
		}

		public void SetBoard(IBoard board)
		{
			this.board = board;
		}

		#region ISolveTechnic Members

		public abstract void SolveHouse(IHouse house, SudokuResult sudokuResult);

		public SolveTechnicInfo Info
		{
			get;
			set;
		}

		public ISudokuHost Host
		{
			get { return this.host; }
			set
			{
				this.host = value;
				this.host.Register(this);
			}
		}

		public void Activate()
		{
			this.IsActive = true;
		}

		public void Deactivate()
		{
			this.IsActive = false;
		}

		public bool IsActive
		{
			get { return this.isActive; }
			private set
			{
				this.isActive = value; this.Info.Active = this.isActive;
			}
		}

		#endregion ISolveTechnic Members
	}
}