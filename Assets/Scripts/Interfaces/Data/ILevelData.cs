using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface ILevelData
    {
        string Name { get; }
        IIllustrationData Illustration { get; }
        int GridSize { get; }
        int PresolvedPieces { get; }
    }
}