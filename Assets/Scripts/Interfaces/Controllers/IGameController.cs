namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Controls the main game scene functionality and gameplay state
    /// </summary>
    public interface IGameController : ISceneController
    {
        /// <summary>
        /// Starts a specific level
        /// </summary>
        /// <param name="levelId">The ID of the level to start</param>
        void StartLevel(int levelId);

        /// <summary>
        /// Pauses the current game
        /// </summary>
        void PauseGame();

        /// <summary>
        /// Resumes the paused game
        /// </summary>
        void ResumeGame();

        /// <summary>
        /// Restarts the current level
        /// </summary>
        void RestartLevel();

        /// <summary>
        /// Completes the current level
        /// </summary>
        void CompleteLevel();

        /// <summary>
        /// Returns to the main menu
        /// </summary>
        void ReturnToMainMenu();
    }
} 