using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Represents a single piece in the puzzle grid
    /// </summary>
    public interface IPuzzlePiece
    {
        IPuzzlePieceRenderer Renderer { get; }
        /// <summary>
        /// The original X position in the grid
        /// </summary>
        int OriginalX { get; }

        /// <summary>
        /// The original Y position in the grid
        /// </summary>
        int OriginalY { get; }

        /// <summary>
        /// Whether the piece is in its correct position
        /// </summary>
        bool IsSolved { get; set; }

        /// <summary>
        /// Marks the piece as solved
        /// </summary>
        void MarkAsSolved();
    }
} 