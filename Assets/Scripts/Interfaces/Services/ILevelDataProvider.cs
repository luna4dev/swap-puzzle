namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Provides level data and information
    /// </summary>
    public interface ILevelDataProvider
    {
        /// <summary>
        /// Gets the data for a specific level
        /// </summary>
        /// <param name="levelId">The ID of the level</param>
        /// <returns>The level data</returns>
        ILevelData GetLevelData(int levelId);

        /// <summary>
        /// Gets data for all levels
        /// </summary>
        /// <returns>Array of all level data</returns>
        ILevelData[] GetAllLevels();

        /// <summary>
        /// Gets the total number of levels
        /// </summary>
        /// <returns>The total number of levels</returns>
        int GetTotalLevels();

        /// <summary>
        /// Checks if a level is a variation of another level
        /// </summary>
        /// <param name="levelId">The ID of the level to check</param>
        /// <returns>True if the level is a variation, false otherwise</returns>
        bool IsVariationLevel(int levelId);
    }
} 