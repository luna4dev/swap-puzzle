using System;
using System.Collections.Generic;

namespace SwapPuzzle.Interfaces
{
    public enum EProgressLogType
    {
        Start,
        Complete
    }
    public interface IProgressRuntime
    {
        List<IProgressLog> ProgressLogs { get; }
        void LogProgressStart(ILevelProgressionData progressionData, ILevelData data);
        void LogProgressComplete(ILevelProgressionData progressionData, ILevelData data);
    }

    public interface IProgressLog
    {
        string LevelProgressionName { get; }
        string LevelName { get; }
        DateTime Timestamp { get; }
        EProgressLogType ProgressType { get; }
    }
}