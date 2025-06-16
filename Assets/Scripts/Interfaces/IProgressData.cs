using System;

namespace SwapPuzzle.Interfaces
{
    public interface IProgressData
    {
        int CurrentLevel { get; set; }
        bool[] UnlockedLevels { get; set; }
        bool[] UnlockedIllustrations { get; set; }
        DateTime LastPlayTime { get; set; }
    }
}