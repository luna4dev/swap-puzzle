namespace SwapPuzzle.Interfaces
{
    public enum ESceneType
    {
        EntryPoint,
        MainMenu,
        Game,
        Index
    }

    /// <summary>
    /// Manages scene loading and transitions
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// Current scene type
        /// </summary>
        ESceneType CurrentScene { get; }

        /// <summary>
        /// Loads a scene with optional transition
        /// </summary>
        /// <param name="targetScene">Target scene to load</param>
        /// <param name="transitionType">Type of transition animation</param>
        /// <param name="data">Optional transition data</param>
        void LoadScene(ESceneType targetScene, ETransitionType transitionType = ETransitionType.Fade, ISceneTransitionData data = null);

        /// <summary>
        /// Event fired when scene changes
        /// </summary>
        System.Action<ESceneType> OnSceneChanged { get; set; }
    }
}