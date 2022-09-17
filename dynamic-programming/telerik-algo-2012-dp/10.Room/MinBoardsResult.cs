namespace Room
{
    class MinBoardsResult
    {
        public MinBoardsResult(int boardCount = 0, string solution = "")
        {
            this.BoardCount = boardCount;
            this.Solution = solution;
        }

        public int BoardCount { get; set; }

        public string Solution { get; set; }

        public MinBoardsResult Clone()
        {
            return new MinBoardsResult(this.BoardCount, this.Solution);
        }
    }
}