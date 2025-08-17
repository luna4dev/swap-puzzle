using System;

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
        /// Event fired when scene change started
        /// </summary>
        event Action<ESceneType, ESceneType> OnSceneChangeStarted;

        /// <summary>
        /// Event fired when scene changes
        /// </summary>
        event Action<ISceneController> OnSceneChanged;

        /// <summary>
        /// Current scene type
        /// </summary>
        ESceneType CurrentScene { get; }

        ISceneController CurrentSceneController { get; }

        /// <summary>
        /// Loads a scene with optional transition
        /// </summary>
        /// <param name="targetScene">Target scene to load</param>
        /// <param name="transitionType">Type of transition animation</param>
        /// <param name="data">Optional transition data</param>
        void LoadScene(ESceneType targetScene, ETransitionType transitionType = ETransitionType.Fade, ISceneTransitionData data = null);
    }
}