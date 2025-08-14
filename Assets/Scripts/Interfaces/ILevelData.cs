using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface ILevelData
    {
        int LevelId { get; }
        Sprite Illustration { get; }
        int GridSize { get; }
        int PresolvedPieces { get; }
        bool IsVariation { get; }
        int BaseLevel { get; }
        string LevelName { get; }
    }
}