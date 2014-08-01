namespace de.onnen.Sudoku.SudokuExternal
{
    public interface ICellBase
    {
        /// <summary>
        /// A bitmask of every candidate
        /// </summary>
        /// <remarks>
        /// A possible solution for an unsolved cell.
        /// Each candidate represents a digit.
        /// Solving a sudoku puzzle is mainly done by elimination of candidates.
        /// When a cell contains a digit, the remaining values are no longer considered candidates for that cell.
        /// In addition, all peers of that cell lose their candidates for that digit, because each house can only contain one instance of each digit.
        /// @see RemovePossibleDigit(int candidate, SudokuResult child)
        /// </remarks>
        int CandidateValue { get; }

        /// <summary>
        /// The type of house (or cell).
        /// </summary>
        HouseType HType { get; }

        /// <summary>
        /// ID of the Cell/House.
        /// </summary>
        /// <remarks>
        /// There are 81 Cells and 9 horizontal rows, nine vertical columns, and nine 3 x 3 blocks (also called boxes).<br />
        /// The rows are numbered 0 to 8, with the top row being 0, and the bottom row being 8. <br />
        /// The columns are similarly numbered, with the leftmost column being 0, and the rightmost being column 9. <br />
        /// The boxes start counting with 0 on the leftmost corner on top.
        /// </remarks>
        int ID { get; }
    }
}