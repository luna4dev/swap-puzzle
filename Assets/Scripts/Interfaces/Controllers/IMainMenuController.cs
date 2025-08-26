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

    }
} 