using System;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Handles core persistence of game progress data
    /// </summary>
    public interface IProgressManager
    {
        /// <summary>
        /// Event triggered when progress state changes (level completion, advancement, reset)
        /// </summary>
        event Action OnProgressChange;
        
        /// <summary>
        /// Gets the current active level data
        /// </summary>
        /// <returns>The current level data</returns>
        ILevelData GetCurrentLevel();
        
        /// <summary>
        /// Marks the current level as completed and triggers progress change event
        /// </summary>
        void CompleteCurrentLevel();
        
        /// <summary>
        /// Advances to the next level in the progression
        /// </summary>
        void GoToNextLevel();
        
        /// <summary>
        /// Resets all progress back to the initial state
        /// </summary>
        void ResetProgress();
    }
}