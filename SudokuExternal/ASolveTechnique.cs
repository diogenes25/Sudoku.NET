namespace de.onnen.Sudoku.SudokuExternal.SolveTechniques
{
	public abstract class ASolveTechnique : ISolveTechnique
	{
		private bool isActive = true;
		private ISudokuHost host;
		protected IBoard board;

		public ASolveTechnique()
		{
			this.Info = new SolveTechniqueInfo()
			{
				Caption = "Not Defined"
			};
		}

		public void SetBoard(IBoard board)
		{
			this.board = board;
		}

		#region ISolveTechnic Members

		public abstract void SolveHouse(IHouse house, SudokuLog sudokuResult);

		public SolveTechniqueInfo Info
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

		public virtual ECellView CellView
		{
			get { return ECellView.GlobalView; }
		}

		#endregion ISolveTechnic Members
	}

	public enum ECellView
	{
		OnlyHouse,
		GlobalView,
	}
}