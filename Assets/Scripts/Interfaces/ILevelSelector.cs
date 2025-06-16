namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages level selection and display in the main menu
    /// </summary>
    public interface ILevelSelector
    {
        /// <summary>
        /// Displays all available levels in the level selection screen
        /// </summary>
        void DisplayAvailableLevels();

        /// <summary>
        /// Handles level selection by the player
        /// </summary>
        /// <param name="levelId">The ID of the selected level</param>
        void OnLevelSelected(int levelId);

        /// <summary>
        /// Checks if a level is unlocked
        /// </summary>
        /// <param name="levelId">The ID of the level to check</param>
        /// <returns>True if the level is unlocked, false otherwise</returns>
        bool IsLevelUnlocked(int levelId);

        /// <summary>
        /// Updates the status of all levels (locked/unlocked)
        /// </summary>
        void UpdateLevelStatus();
    }
} 