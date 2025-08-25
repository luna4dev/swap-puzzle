using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface ILevelProgressionData
    {
        string Name { get; }
        bool IsTest { get; }
        List<ILevelData> Levels { get; }
    }
}