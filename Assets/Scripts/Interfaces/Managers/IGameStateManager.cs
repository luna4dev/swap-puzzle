namespace SwapPuzzle.Interfaces
{
    public enum EGameState
    {
        MainMenu,
        Playing,
        Paused,
        LevelComplete,
        ViewingGallery
    }

    /// <summary>
    /// Manages the global game state and state transitions
    /// </summary>
    public interface IGameStateManager
    {
        /// <summary>
        /// The current state of the game
        /// </summary>
        EGameState CurrentState { get; }

        /// <summary>
        /// Changes the game state
        /// </summary>
        /// <param name="newState">The new state to transition to</param>
        void ChangeState(EGameState newState);

        /// <summary>
        /// Saves the current game state
        /// </summary>
        void SaveGameState();

        /// <summary>
        /// Loads the saved game state
        /// </summary>
        void LoadGameState();
    }
}