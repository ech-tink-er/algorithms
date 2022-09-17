namespace Room
{
    using System.Text;

    class FloorBuilder
    {
        private string floor;

        private int colsCount;

        private MinBoardsResult[,] cache;

        public FloorBuilder(char[,] floor)
        {
            this.SetFloor(floor);
            this.colsCount = floor.GetLength(1);

            this.cache = new MinBoardsResult[this.floor.Length, Utils.PowOf2(this.colsCount)];
        }

        public char[,] SetMinBoards()
        {
            MinBoardsResult result = this.FindMinBoards(index: 0, previous: 0);

            return Utils.ToMatrix(result.Solution.ToCharArray(), this.colsCount);
        }

        public int FindMinBoards()
        {
            MinBoardsResult result = this.FindMinBoards(index: 0, previous: 0);

            return result.BoardCount;
        }

        // The result for an index depends on the previous row of values, whether they are horizontal or vertical.
        // Previous is a bit array holding the previous row of values. (0 - horizontal, 1 - vertical)
        private MinBoardsResult FindMinBoards(int index, int previous)
        {
            if (index >= this.floor.Length)
            {
                return new MinBoardsResult();
            }

            if (this.floor[index] == Utils.Pillar)
            {
                MinBoardsResult res = this.FindMinBoards(index + 1, Utils.InsertLeft(previous, false, this.colsCount));

                res.Solution = Utils.Pillar + res.Solution;

                return res;
            }

            if (this.cache[index, previous] != null)
            {
                return this.cache[index, previous].Clone();
            }

            var horizontal = this.FindMinBoards(index + 1, Utils.InsertLeft(previous, false, this.colsCount));
            horizontal.Solution = Utils.Horizontal + horizontal.Solution;

            var vertical = this.FindMinBoards(index + 1, Utils.InsertLeft(previous, true, this.colsCount));
            vertical.Solution = Utils.Vertical + vertical.Solution;


            bool connectedHorizontal = (index % this.colsCount) > 0 && this.floor[index - 1] != Utils.Pillar && !Utils.GetBit(previous, 0);
            
            int lastVerticalIndex = index - this.colsCount;
            bool connectedVertical = lastVerticalIndex >= 0 && this.floor[lastVerticalIndex] != Utils.Pillar && Utils.GetBit(previous, this.colsCount - 1);

            // If disconnected then a new board starts here.
            if (!connectedHorizontal)
            {
                horizontal.BoardCount++;
            }

            if (!connectedVertical)
            {
                vertical.BoardCount++;
            }

            MinBoardsResult result = this.ChooseBest(horizontal, vertical, index, connectedHorizontal, connectedVertical);

            this.cache[index, previous] = result.Clone();
            return result;
        }

        private MinBoardsResult ChooseBest(MinBoardsResult horizontal, MinBoardsResult vertical, int index, bool connectedHorizontal, bool connectedVertical)
        {
            // The result with the least boards is always better.
            if (horizontal.BoardCount < vertical.BoardCount)
            {
                return horizontal;
            }
            else if (horizontal.BoardCount > vertical.BoardCount)
            {
                return vertical;
            }
            /* If both results have the same amount of boards, we need to choose the one that will be 
             * first lexicographicly when the solution is labeled.
            */
            // The previous vertical board label will always be before the horizontal.
            else if (connectedVertical)
            {
                return vertical;
            }
            else if (connectedHorizontal)
            {
                return horizontal;
            }
            // If we can't connect either vertically or horizontally we choose horizontal only if the next cell is also horizontal.
            else if (((index % this.colsCount) < this.colsCount - 1) && horizontal.Solution[1] == Utils.Horizontal)
            {
                return horizontal;
            }
            // In all other cases vertical is best.
            else
            {
                return vertical;
            }
        }

        private void SetFloor(char[,] floor)
        {
            StringBuilder result = new StringBuilder();

            for (int row = 0; row < floor.GetLength(0); row++)
            {
                for (int col = 0; col < floor.GetLength(1); col++)
                {
                    result.Append(floor[row, col]);
                }
            }

            this.floor = result.ToString();
        }
    }
}