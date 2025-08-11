using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface IPuzzlePieceRenderer
    {
        void SetImage(Sprite sprite);
        void SetSolvedState(bool isSolved);
        void SetDebugText(bool enabled, int order = 0);
    }
}