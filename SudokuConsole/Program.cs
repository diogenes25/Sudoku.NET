using System;

namespace DE.Onnen.Sudoku.SudokuConsole
{
	public class Program
	{
		private static void Main(string[] args)
		{
			//Chapter1 chapter1 = new Chapter1();
			//chapter1.Excample1();
			//chapter1.Excample2();
			//Chapter3 chapter3 = new Chapter3();
			//chapter3.ReadFromFile();
			Chapter2 chapter2 = new Chapter2();
			chapter2.Excample1();
			Console.ReadKey();
		}

		private static void Mainx(string[] args)
		{
			Board board = new Board();
			//board.SetDigit(1, 0, 2);
			//board.SetDigit(1, 2, 3);
			//board.SetDigit(1, 3, 6);
			//board.SetDigit(1, 7, 9);
			//board.SetDigit(2, 0, 9);
			//board.SetDigit(2, 1, 5);
			//board.SetDigit(2, 5, 3);
			//board.SetDigit(2, 7, 2);
			//board.SetDigit(3, 3, 5);
			//board.SetDigit(3, 6, 3);
			//board.SetDigit(4, 0, 7);
			//board.SetDigit(4, 3, 3);
			//board.SetDigit(4, 4, 8);
			//board.SetDigit(4, 5, 6);
			//board.SetDigit(4, 8, 1);
			//board.SetDigit(5, 2, 6);
			//board.SetDigit(5, 5, 7);
			//board.SetDigit(6, 1, 7);
			//board.SetDigit(6, 3, 1);
			//board.SetDigit(6, 7, 3);
			//board.SetDigit(6, 8, 4);
			//board.SetDigit(7, 1, 4);
			//board.SetDigit(7, 5, 9);
			//board.SetDigit(7, 6, 5);
			//board.SetDigit(7, 8, 7);
			//board.SetDigit(0, 0, 6);
			board.SetCellsFromString("000000000000000000000000000000000000000000000000000000000000000000000000000000001");
			//string xxx = board.Matrix();
			string xxx = board.htmlTable();
			Console.WriteLine(xxx);
			//xxx = board.Matrix(true);
			//Console.WriteLine(xxx);
			//xxx = board.MatrixWithCandidates();
			//Console.WriteLine(xxx);
			//Console.WriteLine(board.htmlTable());
			//Console.WriteLine(board.ToString());
			Console.ReadKey();
		}

		private static void Mainy(string[] args)
		{
			//Board board = ReadBoard();
			DateTime start = DateTime.Now;

			//OnlyInBlock(board);
			//OnlyInBlock2(board);
			//LeftInLine(board);
			//NakedTripleInBlock(board);
			//NakedTripleInRow(board);
			//Complete(board);
			//Extreme(board);
			//Hardest(board);
			//board.BruceForce();

			//board.Show(true);
			Console.WriteLine("Start solving");
			//board.CellEvent += new Board.CellEventHandler(BoardEvent);
			SudokuLog result = new SudokuLog();
			//board.Solve(result);
			//Console.WriteLine(result.ToString());
			//board.Show(true);
			//if (!board.IsComplete)
			//{
			//  Console.WriteLine("--- " + board.SolvePercent() + " % ---");
			//  Console.WriteLine("--- Hard way ---");
			//  board.Backtracking(result);
			//}
			Console.WriteLine(DateTime.Now.Subtract(start).Milliseconds);
			Console.WriteLine(result.ToString());
			//Console.WriteLine(board.MatrixWithCandidates());
			Console.ReadKey();
		}

		//		private static void Extreme(Board board)
		//		{
		//			string extreme = @"020080060
		//600057008
		//007000400
		//080000000
		//540090083
		//000000070
		//001000900
		//300920006
		//090040030";
		//			string extreme1 = @"123000000
		//456000000
		//000000000
		//009000000
		//000000000
		//000000000
		//090000000
		//000000000
		//000000000
		//";
		//			string extreme2 = @"123000000
		//456000000
		//780000000
		//000000000
		//000000000
		//000000000
		//000000000
		//000000000
		//000000000
		//";
		//			string[] lines = extreme2.Split('\n');
		//			for (int y = 0; y < 9; y++)
		//			{
		//				string line = lines[y];

		//				for (int x = 0; x < 9; x++)
		//				{
		//					char currChar = line[x];
		//					if (currChar.Equals('0'))
		//						continue;
		//					SudokuLog result = board.SetDigit(y, x, Convert.ToInt32(currChar) - 48);
		//					SudokuHelper.PrintSudokuResult(result);
		//				}
		//			}
		//		}

		//private static Board ReadBoard()
		//{
		//    int[] emh = new int[3];
		//    //IList<Board> boards = SudokuHelper.ReadBoardFromFileTop(@"..\..\..\SudokuTest\TestData\HardestDatabase110626.txt"); // http://forum.enjoysudoku.com/the-hardest-sudokus-new-thread-t6539.html
		//    //IList<Board> boards = SudokuHelper.ReadBoardFromFile(@"..\..\..\SudokuTest\TestData\Simple.txt");
		//    //IList<Board> boards = SudokuHelper.ReadBoardFromFile(@"..\..\..\SudokuTest\TestData\sudoku.txt");
		//    IList<Board> boards = SudokuHelper.ReadBoardFromFileTop<Board>(@"..\..\..\SudokuTest\TestData\top95.txt", '.');
		//    //IList<Board> boards = SudokuHelper.ReadBoardFromFileTop(@"..\..\..\SudokuTest\TestData\ElevensHardestSudoku.txt"); // https://sites.google.com/site/sudoeleven/
		//    DateTime totalStartTime = DateTime.Now;
		//    foreach (Board board in boards)
		//    {
		//        DateTime start = DateTime.Now;
		//        if (board.IsComplete)
		//        {
		//            Console.WriteLine("Easy CalcTime: " + DateTime.Now.Subtract(start).TotalMilliseconds);
		//            emh[0] += 1;
		//            //board.Show(true);
		//        }
		//        else
		//        {
		//            SudokuLog result = new SudokuLog();
		//            board.Solve(result);
		//            if (board.IsComplete)
		//            {
		//                Console.WriteLine("Medium CalcTime: " + DateTime.Now.Subtract(start).TotalMilliseconds);
		//                emh[1] += 1;
		//            }
		//            else
		//            {
		//                //board.Show(true);
		//                board.Backtracking(result);
		//                if (!board.IsComplete)
		//                {
		//                    Console.WriteLine("########## " + result.ErrorMessage + "###################");
		//                }
		//                else
		//                {
		//                    //Console.Write('.');
		//                    Console.WriteLine("HARD CalcTime: " + DateTime.Now.Subtract(start).TotalMilliseconds);
		//                    emh[2] += 1;
		//                    //board.Show(true);
		//                }
		//                //Console.ReadKey();
		//            }
		//        }
		//    }
		//    Console.WriteLine("AVG CalcTime: " + (DateTime.Now.Subtract(totalStartTime).TotalMilliseconds / boards.Count));
		//    Console.WriteLine("Easy: " + emh[0] + " Medium: " + emh[1] + " Hard: " + emh[2]);
		//    return boards[0];
		//}
	}
}