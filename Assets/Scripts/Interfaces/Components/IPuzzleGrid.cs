namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Unified interface for puzzle grid management and piece swapping mechanics
    /// </summary>
    public interface IPuzzleGrid
    {
        int GridSize { get; }
        // Grid Management
        /// <summary>
        /// Initializes the grid with the specified size
        /// </summary>
        /// <param name="gridSize">The size of the grid (e.g., 3 for a 3x3 grid)</param>
        void InitializeGrid(IPuzzleController controller, int gridSize);

        void ClearGrid();

        /// <summary>
        /// Gets the puzzle piece at the specified grid position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>The puzzle piece at that position, or null if empty</returns>
        IPuzzlePiece GetPieceAt(int x, int y);

        /// <summary>
        /// Sets a puzzle piece at the specified grid position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="piece">The piece to place</param>
        void SetPieceAt(int x, int y, IPuzzlePiece piece);

        // Swap Mechanics
        /// <summary>
        /// Initiates a swap between two pieces
        /// </summary>
        /// <param name="piece1">The first piece</param>
        /// <param name="piece2">The second piece</param>
        void InitiateSwap(IPuzzlePiece piece1, IPuzzlePiece piece2);
    }
}