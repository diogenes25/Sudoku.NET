using DE.ONNEN.Sudoku.SudokuExternal;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DE.ONNEN.Sudoku.SudokuWpf
{
	/// <summary>
	/// Interaction logic for SudokuButton.xaml
	/// </summary>
	public partial class SudokuButton : UserControl
	{
		private Button[] buttons = new Button[9];
		private ICell cell;
		private Board board;
		private Dictionary<Button, int> cellInt = new Dictionary<Button, int>();
		private System.Windows.RoutedEventHandler routingEvent;
		private Grid grid;
		private Label lbl;
		private MainWindow mwin;

		public SudokuButton(ICell cell, Board board, MainWindow mwin)
		{
			this.cell = cell;
			this.board = board;
			this.cell.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(cell_PropertyChanged);
			this.mwin = mwin;
			routingEvent = new System.Windows.RoutedEventHandler(SudokuButton_Click);
			lbl = new Label()
			{
				FontSize = 32,
				FontStyle = FontStyles.Normal,
				FontWeight = FontWeights.Heavy,
				VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
				HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center
			};

			InitializeComponent();
			Style style = this.FindResource("smallButtonStyle") as Style;

			this.grid = sudokuButtonGrid;
			int c = 0;
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					buttons[c] = new Button()
					{
						Content = c + 1,
						Name = "btn" + c,
						FontSize = 9.0,
						Style = style,
						Background = new SolidColorBrush(Color.FromRgb(204, 204, 250)),
					};
					cellInt.Add(buttons[c], c);
					buttons[c].Click += routingEvent;
					Grid.SetColumn(buttons[c], x);
					Grid.SetRow(buttons[c], y);
					sudokuButtonGrid.Children.Add(buttons[c]);
					c++;
				}
			}
		}

		private void cell_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Digit")
			{
				return;
			}

			if (((ICell)sender).Digit == 0)
			{
				List<int> c = ((ICell)sender).Candidates;
				for (int v = 0; v < Consts.DimensionSquare; v++)
				{
					buttons[v].Visibility = c.Contains(v + 1) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
				}
			}
			else
			{
				lbl.Content = ((ICell)sender).Digit;
				this.borderMain.Child = lbl;
			}
			mwin.progressbarBoard.Value = board.SolvePercent();
		}

		private void SudokuButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			//SudokuLog result = new SudokuLog();
			//this.cell.SetDigit(cellInt[((Button)sender)] + 1, result);
			SudokuLog result = this.board.SetDigit(this.cell.ID, cellInt[((Button)sender)] + 1, true);
			if (result.Successful)
			{
				mwin.AddLastCell(this.cell);
			}
			else
			{
				MessageBox.Show("AAAAARG2");
				mwin.ReloadBoard();
			}
		}

		public void BoardEvent(SudokuEvent eventInfo)
		{
			if (eventInfo.Action == CellAction.RemPoss)
			{
				buttons[eventInfo.value - 1].Visibility = System.Windows.Visibility.Hidden;
			}
			else
			{
				lbl.Content = eventInfo.value;
				this.borderMain.Child = lbl;
			}
			mwin.progressbarBoard.Value = board.SolvePercent();
		}

		public void Reload()
		{
			if (cell.Digit > 0)
			{
				lbl.Content = cell.Digit;
				//this.Content = lbl;
				this.borderMain.Child = lbl;
			}
			else
			{
				//this.Content = this.grid;
				this.borderMain.Child = this.grid;
				List<int> p = cell.Candidates;
				for (int i = 0; i < 9; i++)
				{
					buttons[i].Visibility = p.Contains(i + 1) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
				}
			}
		}
	}
}