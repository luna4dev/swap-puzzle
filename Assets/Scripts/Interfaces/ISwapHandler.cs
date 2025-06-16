namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Handles piece selection and swapping mechanics
    /// </summary>
    public interface ISwapHandler
    {
        /// <summary>
        /// Initiates a swap between two pieces
        /// </summary>
        /// <param name="piece1">The first piece</param>
        /// <param name="piece2">The second piece</param>
        void InitiateSwap(IPuzzlePiece piece1, IPuzzlePiece piece2);

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
    }
} 