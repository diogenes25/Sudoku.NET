using System.Windows;
using DE.ONNEN.Sudoku.SudokuExternal.SolveTechniques;
using Microsoft.Windows.Controls.Ribbon;

namespace DE.ONNEN.Sudoku.SudokuWpf
{
    public
        class RibbonGroupSolveTechnic : RibbonGroup
    {
        private ISolveTechnique solveTechnic;

        public RibbonGroupSolveTechnic(ISolveTechnique solveTechnic)
            : base()
        {
            this.solveTechnic = solveTechnic;
            Header = solveTechnic.Info.Caption;
            RibbonButton eb = new RibbonButton() { Label = "Info" };
            eb.Click += new RoutedEventHandler(eb_Click);
            this.Items.Add(eb);
            RibbonCheckBox rcb = new RibbonCheckBox()
            {
                Label = "Active",
                IsChecked = solveTechnic.IsActive,
            };
            rcb.Checked += new RoutedEventHandler(rcb_Checked);
            rcb.Unchecked += new RoutedEventHandler(rcb_UnChecked);
            this.Items.Add(rcb);

            //this.board.boardChangeEvent += new Board.BoardChanged(board_boardChangeEvent);
        }

        private void rcb_UnChecked(object sender, RoutedEventArgs e)
        {
            this.solveTechnic.Deactivate();
        }

        private void rcb_Checked(object sender, RoutedEventArgs e)
        {
            this.solveTechnic.Activate();
        }

        private void eb_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.solveTechnic.Info.Caption);
        }
    }
}