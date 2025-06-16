using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface ILevelData
    {
        int LevelId { get; }
        Texture2D Illustration { get; }
        int GridSize { get; }
        int PreSolvedPieces { get; }
        bool IsVariation { get; }
        int BaseLevel { get; }
        string LevelName { get; }
    }
}