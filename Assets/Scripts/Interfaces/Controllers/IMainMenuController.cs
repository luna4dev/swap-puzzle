namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Controls the main menu scene functionality and user interactions
    /// </summary>
    public interface IMainMenuController : ISceneController
    {
        /// <summary>
        /// Shows the level selection screen
        /// </summary>
        void ShowLevelSelection();

        /// <summary>
        /// Shows the settings panel
        /// </summary>
        void ShowSettings();

        /// <summary>
        /// Handles play button press for a specific level
        /// </summary>
        /// <param name="levelId">The ID of the level to play</param>
        void OnPlayButtonPressed(int levelId);

        /// <summary>
        /// Handles index button press to view the illustration gallery
        /// </summary>
        void OnIndexButtonPressed();
    }
} 