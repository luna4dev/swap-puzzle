using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// A gallery data. This will be equivalent to single page in gallary
    /// It will hold informations to form up the gallery page including illustrations, a dialogue, effect etc.
    /// TODO: add dialogue, effects
    /// </summary>
    public interface IGalleryData
    {
        string Name { get; }
        List<IIllustrationData> Illustrations { get; }
    }
}