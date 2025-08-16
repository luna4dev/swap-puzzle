namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Handles illustration unlock logic and validation
    /// </summary>
    public interface IIllustrationUnlockService
    {
        /// <summary>
        /// Unlocks an illustration if conditions are met
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration to unlock</param>
        /// <param name="progressData">Current player progress</param>
        /// <returns>True if successfully unlocked, false if conditions not met</returns>
        bool UnlockIllustration(int illustrationId, IProgressData progressData);

        /// <summary>
        /// Checks if an illustration can be unlocked based on current progress
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration to check</param>
        /// <param name="progressData">Current player progress</param>
        /// <returns>True if unlock conditions are met</returns>
        bool CanUnlockIllustration(int illustrationId, IProgressData progressData);

        /// <summary>
        /// Gets the unlock requirements for a specific illustration
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration</param>
        /// <returns>Description of unlock requirements</returns>
        string GetUnlockRequirements(int illustrationId);
    }
}