using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Base interface for all scene controllers in the game.
    /// Handles scene lifecycle and transitions.
    /// </summary>
    public interface ISceneController
    {
        /// <summary>
        /// Initializes the scene and its components
        /// </summary>
        void InitializeScene();

        /// <summary>
        /// Called when entering the scene with transition data
        /// </summary>
        /// <param name="data">Transition data containing scene parameters</param>
        void OnSceneEnter(ISceneTransitionData data);

        /// <summary>
        /// Called when exiting the scene
        /// </summary>
        void OnSceneExit();

        /// <summary>
        /// Cleans up scene resources and state
        /// </summary>
        void CleanupScene();
    }
}
