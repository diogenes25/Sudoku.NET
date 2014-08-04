using de.onnen.Sudoku.SudokuExternal;
using de.onnen.Sudoku.SudokuExternal.SolveTechniques;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace de.onnen.Sudoku.SudokuWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// https://www.iconfinder.com/search/?q=iconset%3AVistaICO_Toolbar-Icons
	/// </summary>
	public partial class MainWindow : Window
	{
		private SudokuButton[] btnSudoku = new SudokuButton[81];
		private Board board;
		private UpdateDelegate update;

		private delegate void UpdateDelegate();

		public MainWindow()
		{
			InitializeComponent();
			//board = new Board("F:\\Develop\\SolveTechnics\\");
			board = new Board("..\\..\\..\\Sudoku\\SolveTechnics\\");
			Grid[] block = new Grid[9];
			int c = 0;
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					block[c] = new Grid();
					block[c].RowDefinitions.Add(new RowDefinition());
					block[c].RowDefinitions.Add(new RowDefinition());
					block[c].RowDefinitions.Add(new RowDefinition());
					block[c].ColumnDefinitions.Add(new ColumnDefinition());
					block[c].ColumnDefinitions.Add(new ColumnDefinition());
					block[c].ColumnDefinitions.Add(new ColumnDefinition());
					Grid.SetColumn(block[c], x);
					Grid.SetRow(block[c], y);
					gridBlock.Children.Add(block[c]);
					c++;
				}
			}
			SolidColorBrush dark = new SolidColorBrush(Color.FromArgb(150, 50, 204, 250));
			block[1].Background = dark;
			block[3].Background = dark;
			block[5].Background = dark;
			block[7].Background = dark;

			block[0].Background = new SolidColorBrush(Color.FromArgb(150, 204, 204, 200));
			block[2].Background = new SolidColorBrush(Color.FromArgb(150, 204, 204, 200));
			block[4].Background = new SolidColorBrush(Color.FromArgb(150, 204, 204, 200));
			block[6].Background = new SolidColorBrush(Color.FromArgb(150, 204, 204, 200));
			block[8].Background = new SolidColorBrush(Color.FromArgb(150, 204, 204, 200));

			c = 0;
			for (int y = 0; y < 9; y++)
			{
				for (int x = 0; x < 9; x++)
				{
					btnSudoku[c] = new SudokuButton(board.Cells[c], board, this)
					{
						Margin = new Thickness(2.0, 2.0, 2.0, 2.0),
					};
					int blockId = ((int)(y / 3) * 3) + (int)(x / 3);
					Grid.SetColumn(btnSudoku[c], x % 3);
					Grid.SetRow(btnSudoku[c], y % 3);
					block[blockId].Children.Add(btnSudoku[c]);
					c++;
				}
			}

			IList<ISolveTechnique> sts = board.SolveTechniques;
			foreach (ISolveTechnique st in sts)
			{
				//this.MainRibbon.UseLayoutRoundingdd
				rbnTabSolve.Items.Add(new RibbonGroupSolveTechnic(st));
			}
			//RibbonGroup rg = new RibbonGroup
			//{
			//    Header = "xxx",
			//};
			//RibbonButton eb = new RibbonButton() { Label = "Info" };
			//eb.Click += new RoutedEventHandler(eb_Click);
			//rg.Items.Add(eb);
			//RibbonCheckBox rcb = new RibbonCheckBox()
			//{
			//    Label = "Aktive"
			//};
			//rg.Items.Add(rcb);
			//rbnTabSolve.Items.Add(rg);

			update = new UpdateDelegate(ReloadBoard);
			this.board.boardChangeEvent += new Board.BoardChanged(board_boardChangeEvent);
		}

		//void eb_Click(object sender, RoutedEventArgs e)
		//{
		//    MessageBox.Show("XXX");
		//}

		private void board_boardChangeEvent(IBoard board, SudokuEvent sudokuEvent)
		{
			this.board.SetBoard(board);
			Dispatcher.BeginInvoke(update, null);
			//MessageBox.Show("HHH");
		}

		public void AddLastCell(ICell cell)
		{
			SudokuHistoryItem lastHist = board.History[board.History.Count - 1];
			ListBoxItem lvItem = new ListBoxItem()
			{
				Content = lastHist.ToString()
			};
			lstbxHistory.Items.Add(lvItem);

			//txtblockInfo.Text = SudokuHelper.PrintSudokuResult(lastHist.SudokuResult);
			treeviewResult.Items.Clear();
			foreach (SudokuLog sre in lastHist.SudokuResults.ChildSudokuResult)
			{
				TreeViewItem cc = new TreeViewItem()
				{
					Header = sre.ToString()
				};
				SetTreeViewValue(sre, cc);
				treeviewResult.Items.Add(cc);
			}
		}

		private void SetTreeViewValue(SudokuLog sudokuReslt, TreeViewItem tvi)
		{
			tvi.Header = sudokuReslt.ToString();
			if (sudokuReslt.ChildSudokuResult != null && sudokuReslt.ChildSudokuResult.Count > 0)
			{
				foreach (SudokuLog sre in sudokuReslt.ChildSudokuResult)
				{
					TreeViewItem cc = new TreeViewItem()
					{
						Header = sudokuReslt.ToString(),
						//Foreground = new SolidColorBrush(Colors.WhiteSmoke),
					};
					SetTreeViewValue(sre, cc);
					tvi.Items.Add(cc);
				}
			}
		}

		private void BtnNew_Click(object sender, RoutedEventArgs e)
		{
			board.Reset();
			lstbxHistory.Items.Clear();
			ReloadBoard();
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.DefaultExt = ".txt";
			dlg.Filter = "Text documents (.txt)|*.txt";

			Nullable<bool> dialogResult = dlg.ShowDialog();
			if (dialogResult == true)
			{
				string filename = dlg.FileName;
				StreamWriter writer = File.CreateText(filename);
				string[] xxx = new string[81];
				for (int i = 0; i < xxx.Length; i++)
				{
					xxx[i] = "0";
				}
				foreach (Cell c in board.Givens)
				{
					xxx[c.ID] = c.Digit.ToString();
				}
				int sc = 0;
				for (int y = 0; y < 9; y++)
				{
					StringBuilder db = new StringBuilder();
					for (int x = 0; x < 9; x++)
					{
						db.Append(xxx[sc]);
						sc++;
					}
					writer.WriteLine(db.ToString());
					//db.Append(Environment.NewLine);
				}
				writer.Close();
			}
		}

		private void lstbxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int si = lstbxHistory.SelectedIndex;
			board.SetHistory(si);
			ReloadBoard();
		}

		public void ReloadBoard()
		{
			foreach (SudokuButton btn in this.btnSudoku)
			{
				btn.Reload();
			}
		}

		private void BtnLoad_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.DefaultExt = ".txt";
			dlg.Filter = "Text documents (.txt)|*.txt";

			Nullable<bool> dialogResult = dlg.ShowDialog();

			if (dialogResult == true)
			{
				string filename = dlg.FileName;
				//this.listBox1.Items.Add(new ListBoxItem() { Content = filename });
				if (File.Exists(filename))
				{
					board.Reset();
					SudokuLog result = null;
					TextReader tr = new StreamReader(filename);
					string line;
					int y = 0;
					while ((line = tr.ReadLine()) != null && y < 9)
					{
						for (int x = 0; x < 9; x++)
						{
							char currChar = line[x];
							if (!currChar.Equals('0'))
							{
								result = board.SetDigit(y, x, Convert.ToInt32(currChar) - 48);
								if (!result.Successful)
								{
									MessageBox.Show("Kann nicht geladen werden");
								}
							}
						}
						y++;
					}
					tr.Close();
					lstbxHistory.Items.Clear();
					//board.Solve(result);
					ReloadBoard();
					foreach (SudokuHistoryItem lastHist in board.History)
					{
						ListBoxItem lvItem = new ListBoxItem()
						{
							Content = lastHist.ToString()
						};
						lstbxHistory.Items.Add(lvItem);
					}
				}
			}
		}

		private void btnBacktrack_Click(object sender, RoutedEventArgs e)
		{
			SudokuLog sresult = new SudokuLog();
			//ParameterizedThreadStart pts = new ParameterizedThreadStart(board.Backtracking);
			//Thread thread = new Thread(pts);
			//thread.Start(sresult);

			//Thread workerThread = new Thread(board.Backtracking);
			//workerThread.Start();
			Task.Factory.StartNew(() => board.Backtracking());
			//board.Backtracking(sresult);
			//ReloadBoard();
		}

		private void btnReduce_Click(object sender, RoutedEventArgs e)
		{
			SudokuLog sresult = new SudokuLog();
			board.Solve(sresult, true);
		}
	}
}