namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Interface for puzzle piece shuffling strategies
    /// </summary>
    public interface IShuffler
    {
        /// <summary>
        /// Shuffles puzzle pieces with control over how many pieces remain solved
        /// </summary>
        /// <param name="puzzleGrid">The puzzle grid to shuffle</param>
        /// <param name="targetSolvedCount">Number of pieces that should remain in correct positions after shuffle</param>
        void Shuffle(IPuzzleGrid puzzleGrid, int targetSolvedCount = 0);
    }
}