using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls.Ribbon;
using de.onnen.Sudoku.SudokuExternal.SolveTechnics;
using System.Windows;

namespace de.onnen.Sudoku.SudokuWpf
{
    public
        class RibbonGroupSolveTechnic : RibbonGroup
    {
        private ISolveTechnic solveTechnic;
        public RibbonGroupSolveTechnic(ISolveTechnic solveTechnic)
            : base()
        {
            this.solveTechnic = solveTechnic;
            Header = solveTechnic.Info.Caption;
            RibbonButton eb = new RibbonButton() { Label = "Info" };
            eb.Click += new RoutedEventHandler(eb_Click);
            this.Items.Add(eb);
            RibbonCheckBox rcb = new RibbonCheckBox()
            {
                Label = "Aktive",
                IsChecked = solveTechnic.IsActive,
            };
            rcb.Checked += new RoutedEventHandler(rcb_Checked);
            rcb.Unchecked += new RoutedEventHandler(rcb_UnChecked);
            this.Items.Add(rcb);


            //this.board.boardChangeEvent += new Board.BoardChanged(board_boardChangeEvent);
        }

        void rcb_UnChecked(object sender, RoutedEventArgs e)
        {
            this.solveTechnic.Deactivate();
        }

        void rcb_Checked(object sender, RoutedEventArgs e)
        {
            this.solveTechnic.Activate();
        }

        void eb_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("XXX");
        }

    }
}
