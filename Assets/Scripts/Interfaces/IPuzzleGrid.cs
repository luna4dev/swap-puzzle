namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Unified interface for puzzle grid management and piece swapping mechanics
    /// </summary>
    public interface IPuzzleGrid
    {
        // Grid Management
        /// <summary>
        /// Initializes the grid with the specified size
        /// </summary>
        /// <param name="size">The size of the grid (e.g., 3 for a 3x3 grid)</param>
        void InitializeGrid(int size);

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
        /// <param anme="piece3">A flag to emit swap event</param>
        void InitiateSwap(IPuzzlePiece piece1, IPuzzlePiece piece2, bool emitEvent);

        /// <summary>
        /// Handles the selection of a piece by the player
        /// </summary>
        /// <param name="selectedPiece">The piece that was selected</param>
        void HandlePieceSelection(IPuzzlePiece selectedPiece);

        /// <summary>
        /// Checks if two pieces can be swapped
        /// </summary>
        /// <param name="piece1">The first piece</param>
        /// <param name="piece2">The second piece</param>
        /// <returns>True if the pieces can be swapped, false otherwise</returns>
        bool CanSwapPieces(IPuzzlePiece piece1, IPuzzlePiece piece2);

        /// <summary>
        /// Clears the current piece selection
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Gets the currently selected piece
        /// </summary>
        /// <returns>The currently selected piece, or null if none selected</returns>
        IPuzzlePiece GetSelectedPiece();

        /// <summary>
        /// Gets the grid size
        /// </summary>
        /// <returns>The current grid size</returns>
        int GetGridSize();
    }
}