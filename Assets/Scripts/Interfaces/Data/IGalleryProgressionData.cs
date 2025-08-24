using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// An order data of galleries
    /// This data will be used to form the index scene.
    /// </summary>
    public interface IGalleryProgressionData
    {
        string Name { get; }
        List<IGalleryData> Galleries { get; }
    }
}