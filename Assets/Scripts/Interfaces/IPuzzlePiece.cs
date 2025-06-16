namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Represents a single piece in the puzzle grid
    /// </summary>
    public interface IPuzzlePiece
    {
        /// <summary>
        /// The original X position in the grid
        /// </summary>
        int OriginalX { get; }

        /// <summary>
        /// The original Y position in the grid
        /// </summary>
        int OriginalY { get; }

        /// <summary>
        /// The current X position in the grid
        /// </summary>
        int CurrentX { get; set; }

        /// <summary>
        /// The current Y position in the grid
        /// </summary>
        int CurrentY { get; set; }

        /// <summary>
        /// Whether the piece is in its correct position
        /// </summary>
        bool IsSolved { get; }

        /// <summary>
        /// Whether the piece is locked and cannot be moved
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Sets the position of the piece in the grid
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        void SetPosition(int x, int y);

        /// <summary>
        /// Marks the piece as solved
        /// </summary>
        void MarkAsSolved();

        /// <summary>
        /// Locks the piece in place
        /// </summary>
        void LockPiece();

        /// <summary>
        /// Called when the piece is selected by the player
        /// </summary>
        void OnPieceSelected();
    }
} 