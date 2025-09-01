using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface IPuzzlePieceRenderer
    {
        void SetImage(Sprite sprite);
        void SetEnabled();
        void SetDisabled();
        void SetDebug(bool enabled, int order = 0);
    }
}