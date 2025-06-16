namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages the display and interaction with unlocked illustrations
    /// </summary>
    public interface IIllustrationGallery
    {
        /// <summary>
        /// Loads all unlocked illustrations
        /// </summary>
        void LoadUnlockedIllustrations();

        /// <summary>
        /// Displays a specific illustration
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration to display</param>
        void DisplayIllustration(int illustrationId);

        /// <summary>
        /// Shows detailed information about an illustration
        /// </summary>
        /// <param name="data">The illustration data to display</param>
        void ShowIllustrationDetails(IIllustrationData data);

        /// <summary>
        /// Checks if an illustration is unlocked
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration to check</param>
        /// <returns>True if the illustration is unlocked, false otherwise</returns>
        bool IsIllustrationUnlocked(int illustrationId);
    }
} 