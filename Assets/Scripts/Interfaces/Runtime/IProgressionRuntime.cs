using System;

namespace SwapPuzzle.Interfaces
{
    public interface IProgressionRuntime
    {
        string LevelProgressionName { get; set; }
        string LevelName { get; set; }
        DateTime LastPlayTime { get; set; }
    }
}