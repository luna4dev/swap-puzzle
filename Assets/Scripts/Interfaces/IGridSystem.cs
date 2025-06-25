using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages the puzzle grid and piece placement
    /// </summary>
    public interface IGridSystem
    {
        /// <summary>
        /// Initializes the grid with the specified size
        /// </summary>
        /// <param name="size">The size of the grid (e.g., 3 for a 3x3 grid)</param>
        void InitializeGrid(int size);
    }
} 