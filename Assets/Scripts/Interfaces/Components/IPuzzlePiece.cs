using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Represents a single piece in the puzzle grid
    /// </summary>
    public interface IPuzzlePiece
    {
        Vector2Int OriginalPos { get; }
        Vector2Int Pos { get; }

        bool IsSolved();

        void SetPos(Vector2Int pos);

        void SetNormalState();

        void SetDisabledState();

        void SetImage(Sprite image);

        void SetDebug(bool debug);
    }
}