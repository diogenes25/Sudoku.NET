using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DE.Onnen.Sudoku
{
    [TestClass]
    public class IBoardTest
    {
        /// <summary>
        /// Digit must be set.
        ///</summary>
        [TestMethod()]
        public void SetDigit_Digit_in_Cell_must_be_set_()
        {
            IBoard target = new Board();
            int digit = 1;
            int cell = 0;
            target.SetDigit(cell, digit);
            Assert.AreEqual(1, target[0].Digit);
            Assert.AreEqual(0, target[0].CandidateValue);
        }

        [TestMethod()]
        public void SetDigit_Digit_removed_as_candidate_in_col()
        {
            IBoard target = new Board();
            int digit = 1;
            int cell = 0;
            target.SetDigit(cell, digit);

            int baseValue = (1 << Consts.DimensionSquare) - 1;
            int expected = baseValue - (1 << 0);
            for (int i = 1; i < 9; i++)
            {
                Assert.AreEqual(0, target[i * 9].Digit);
                Assert.AreEqual(expected, target[i * 9].CandidateValue);
            }
        }

        [TestMethod()]
        public void SetDigit_Digit_removed_as_candidate_in_row()
        {
            IBoard target = new Board();
            int digit = 1;
            int cell = 0;
            target.SetDigit(cell, digit);

            int baseValue = (1 << Consts.DimensionSquare) - 1;
            int expected = baseValue - (1 << 0);
            for (int i = 1; i < 9; i++)
            {
                Assert.AreEqual(0, target[i].Digit);
                Assert.AreEqual(expected, target[i].CandidateValue);
            }
        }

        [TestMethod()]
        public void SetDigit_Digit_removed_as_candidate_in_box()
        {
            IBoard target = new Board();
            int digit = 1;
            int cell = 0;
            target.SetDigit(cell, digit);

            int baseValue = (1 << Consts.DimensionSquare) - 1;
            int expected = baseValue - (1 << 0);
            int containerIdx = 0;
            for (int zr = 0; zr < Consts.Dimension; zr++)
            {
                for (int zc = 0; zc < Consts.Dimension; zc++)
                {
                    int b = (containerIdx * Consts.Dimension) + (zc + (zr * Consts.DimensionSquare)) + ((containerIdx / Consts.Dimension) * Consts.DimensionSquare * 2);
                    if (b == 0)
                        continue;
                    Assert.AreEqual(0, target[b].Digit);
                    Assert.AreEqual(expected, target[b].CandidateValue);
                }
            }
        }

        [TestMethod()]
        public void SetDigit_Digit_removed_as_candidate_in_peer_row()
        {
            IBoard target = new Board();
            int digit = 1;
            int cell = 0;
            target.SetDigit(cell, digit);

            int baseValue = (1 << Consts.DimensionSquare) - 1;
            int expected = baseValue - (1 << 0);
            //foreach(target.p
            //for (int i = 1; i < 9; i++)
            //{
            //    Assert.AreEqual(0, target[i].Digit);
            //    Assert.AreEqual(expected, target[i].CandidateValue);
            //}
        }
    }
}