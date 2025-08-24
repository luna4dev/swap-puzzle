using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface IIllustrationData
    {
        string Name { get; }
        Sprite Illustration { get; }
    }
}