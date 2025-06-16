using System.Collections.Generic;

namespace SwapPuzzle.Interfaces
{
    public enum ESceneType
    {
        MainMenu,
        Game,
        Index
    }

    public interface ISceneTransitionData
    {
        ESceneType SourceScene { get; }
        ESceneType TargetScene { get; }
        Dictionary<string, object> Parameters { get; }
    }
}