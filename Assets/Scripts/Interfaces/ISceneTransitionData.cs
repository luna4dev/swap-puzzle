using System.Collections.Generic;

namespace SwapPuzzle.Interfaces
{
    public interface ISceneTransitionData
    {
        ESceneType SourceScene { get; }
        ESceneType TargetScene { get; }
        Dictionary<string, object> Parameters { get; }
    }
}