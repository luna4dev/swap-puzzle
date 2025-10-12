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

        void ClearPuzzle();

        void HandlePuzzlePieceDrop();
    }
} 