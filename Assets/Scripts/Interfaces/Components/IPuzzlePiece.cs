using UnityEngine;

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

        bool IsSolved { get; }

        void SetPrestine();

        void SetSolved();

        void SetLevelCompleted();

        void SetImage(Sprite image);

        void SetDebug(bool debug);
    }
} 