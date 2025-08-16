using UnityEngine;

namespace SwapPuzzle.Interfaces
{

    public enum ETransitionType
    {
        Fade,
        Slide,
        Instant
    }

    /// <summary>
    /// Handles scene transitions and animations between different game scenes
    /// </summary>
    public interface ISceneTransition
    {
        /// <summary>
        /// Transitions to a target scene with optional transition data
        /// </summary>
        /// <param name="targetScene">The scene type to transition to</param>
        /// <param name="data">Optional transition data</param>
        void TransitionToScene(ESceneType targetScene, ISceneTransitionData data = null);

        /// <summary>
        /// Sets the type of transition animation to use
        /// </summary>
        /// <param name="type">The type of transition animation</param>
        void SetTransitionAnimation(ETransitionType type);

        /// <summary>
        /// Whether a transition is currently in progress
        /// </summary>
        bool IsTransitioning { get; }
    }
}