namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Handles core persistence of game progress data
    /// </summary>
    public interface IProgressPersistence
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
        /// Checks if saved data is available
        /// </summary>
        /// <returns>True if saved data exists, false otherwise</returns>
        bool IsDataAvailable();
    }
}