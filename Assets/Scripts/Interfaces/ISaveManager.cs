namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages saving and loading game progress
    /// </summary>
    public interface ISaveManager
    {
        /// <summary>
        /// Saves the current game progress
        /// </summary>
        /// <param name="data">The progress data to save</param>
        void SaveProgress(IProgressData data);

        /// <summary>
        /// Loads the saved game progress
        /// </summary>
        /// <returns>The loaded progress data</returns>
        IProgressData LoadProgress();

        /// <summary>
        /// Saves the completion status of a level
        /// </summary>
        /// <param name="levelId">The ID of the completed level</param>
        void SaveLevelCompletion(int levelId);

        /// <summary>
        /// Unlocks an illustration
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration to unlock</param>
        void UnlockIllustration(int illustrationId);

        /// <summary>
        /// Checks if saved data is available
        /// </summary>
        /// <returns>True if saved data exists, false otherwise</returns>
        bool IsDataAvailable();
    }
} 