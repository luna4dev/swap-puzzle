namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// High-level save management that coordinates persistence and unlock services
    /// </summary>
    public interface ISaveManager
    {
        /// <summary>
        /// Gets the progress persistence service
        /// </summary>
        IProgressPersistence ProgressPersistence { get; }

        /// <summary>
        /// Gets the illustration unlock service
        /// </summary>
        IIllustrationUnlockService IllustrationUnlockService { get; }

        /// <summary>
        /// Saves the current game progress and handles any related unlock checks
        /// </summary>
        /// <param name="data">The progress data to save</param>
        void SaveProgress(IProgressData data);

        /// <summary>
        /// Loads the saved game progress
        /// </summary>
        /// <returns>The loaded progress data</returns>
        IProgressData LoadProgress();

        /// <summary>
        /// Completes a level and handles unlock logic
        /// </summary>
        /// <param name="levelId">The ID of the completed level</param>
        /// <param name="progressData">Current progress data</param>
        void CompleteLevel(int levelId, IProgressData progressData);
    }
} 