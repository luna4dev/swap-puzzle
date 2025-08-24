using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public enum EDifficultyType
    {
        None = -1,
        Easy = 0,
        Medium,
        Difficult,
        Max
    }
    public interface ILevelProgressionData
    {
        string Name { get; }
        bool IsTest { get; }
        EDifficultyType Difficulty { get; }
        List<ILevelData> Levels { get; }
    }
}