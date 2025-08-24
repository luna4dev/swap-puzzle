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
        void SaveProgress();

        /// <summary>
        /// Loads the saved game progress
        /// </summary>
        /// <returns>The loaded progress data</returns>
        IProgressionRuntime LoadProgress();

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