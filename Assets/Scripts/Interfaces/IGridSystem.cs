using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages the puzzle grid and piece placement
    /// </summary>
    public interface IGridSystem
    {
        /// <summary>
        /// Initializes the grid with the specified size
        /// </summary>
        /// <param name="size">The size of the grid (e.g., 3 for a 3x3 grid)</param>
        void InitializeGrid(int size);

        /// <summary>
        /// Places a puzzle piece at the specified grid position
        /// </summary>
        /// <param name="piece">The piece to place</param>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        void PlacePiece(IPuzzlePiece piece, int x, int y);

        /// <summary>
        /// Gets the piece at the specified grid position
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <returns>The piece at the position, or null if empty</returns>
        IPuzzlePiece GetPieceAt(int x, int y);

        /// <summary>
        /// Checks if a position is valid in the grid
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <returns>True if the position is valid, false otherwise</returns>
        bool IsValidPosition(int x, int y);

        /// <summary>
        /// Swaps two pieces at the specified positions
        /// </summary>
        /// <param name="pos1">The first position</param>
        /// <param name="pos2">The second position</param>
        void SwapPieces(Vector2Int pos1, Vector2Int pos2);

        /// <summary>
        /// Converts a grid position to world position
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <returns>The world position for the grid position</returns>
        Vector2 GetWorldPosition(int x, int y);
    }
} 