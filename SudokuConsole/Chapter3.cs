using System;
using System.Collections.Generic;

namespace DE.Onnen.Sudoku.SudokuConsole
{
    class Chapter3
    {
        public void ReadFromFile()
        {
            //IList<string> boards = SudokuHelper.ReadBoardFromFileTop(@"..\..\..\SudokuTest\TestData\top95.txt");
            //IList<string> boards = SudokuHelper.ReadBoardFromFile(@"..\..\..\SudokuTest\TestData\sudoku.txt");
            IList<string> boards = SudokuHelper.ReadBoardFromFileTop(@"..\..\..\SudokuTest\TestData\HardestDatabase110626.txt"); // http://forum.enjoysudoku.com/the-hardest-sudokus-new-thread-t6539.html
            //IList<string> boards = SudokuHelper.ReadBoardFromFile(@"..\..\..\SudokuTest\TestData\Simple.txt");

            Console.WriteLine(boards[0]);
            IBoard board = new Board("..\\..\\..\\Sudoku\\SolveTechnics\\");
            int[] emh = new int[3];
            DateTime totalStartTime = DateTime.Now;
            foreach (string line in boards)
            {
                board.SetCellsFromString(line);
                DateTime start = DateTime.Now;
                if (board.IsComplete)
                {
                    Console.WriteLine("Easy CalcTime: " + DateTime.Now.Subtract(start).TotalMilliseconds);
                    emh[0] += 1;
                    //board.Show(true);
                }
                else
                {
                    SudokuLog result = new SudokuLog();
                    board.Solve(result);
                    if (board.IsComplete)
                    {
                        Console.WriteLine("Medium CalcTime: " + DateTime.Now.Subtract(start).TotalMilliseconds);
                        emh[1] += 1;
                    }
                    else
                    {
                        //board.Show(true);
                        board.Backtracking(result);
                        if (!board.IsComplete)
                        {
                            Console.WriteLine("########## " + result.ErrorMessage + "###################");
                        }
                        else
                        {
                            //Console.Write('.');
                            Console.WriteLine("HARD CalcTime: " + DateTime.Now.Subtract(start).TotalMilliseconds);
                            emh[2] += 1;
                            //board.Show(true);
                        }
                        //Console.ReadKey();
                    }
                }
            }
            Console.WriteLine("AVG CalcTime: " + (DateTime.Now.Subtract(totalStartTime).TotalMilliseconds / boards.Count));
            Console.WriteLine("Easy: " + emh[0] + " Medium: " + emh[1] + " Hard: " + emh[2]);
        }
    }
}