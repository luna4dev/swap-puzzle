using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Represents a single piece in the puzzle grid
    /// </summary>
    public interface IPuzzlePiece
    {
        Vector2Int OriginalPos { get; }

        bool IsSolved { get; }

        void SetPrestine();

        void SetSolved();

        void SetLevelCompleted();

        void SetImage(Sprite image);

        void SetDebug(bool debug);
    }
}