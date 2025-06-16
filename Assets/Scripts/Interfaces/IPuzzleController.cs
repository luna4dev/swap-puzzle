namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Controls the core puzzle mechanics and game logic
    /// </summary>
    public interface IPuzzleController
    {
        /// <summary>
        /// Initializes a new puzzle with the given level data
        /// </summary>
        /// <param name="levelData">The data for the level to initialize</param>
        void InitializePuzzle(ILevelData levelData);

        /// <summary>
        /// Shuffles the puzzle pieces to start the game
        /// </summary>
        void ShufflePieces();

        /// <summary>
        /// Checks if the current puzzle state is a solution
        /// </summary>
        void CheckSolution();

        /// <summary>
        /// Checks if the current level is complete
        /// </summary>
        /// <returns>True if the level is complete, false otherwise</returns>
        bool IsLevelComplete();

        /// <summary>
        /// Gets the count of pieces that are in their correct positions
        /// </summary>
        /// <returns>The number of solved pieces</returns>
        int GetSolvedPiecesCount();
    }
} 