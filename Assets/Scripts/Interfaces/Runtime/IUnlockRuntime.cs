using System;
using System.Collections.Generic;

namespace SwapPuzzle.Interfaces
{
    public interface IUnlockRuntime
    {
        List<IUnlockLog> UnlockLogs { get; }
        void LogUnlock(IIllustrationData data);
    }

    public interface IUnlockLog
    {
        string IllustrationName { get; }
        DateTime Timestamp { get; }
    }
}