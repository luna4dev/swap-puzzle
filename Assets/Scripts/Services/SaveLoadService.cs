using System.Collections.Generic;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Classes;
using UnityEngine;

namespace SwapPuzzle.Services
{
    /// <summary>
    /// Provides static methods for saving and loading game data using Unity's PlayerPrefs system.
    /// Optimized for WebGL deployment with error handling and data validation.
    /// </summary>
    public static class SaveLoadService
    {
        /// <summary>
        /// Dictionary mapping runtime data types to their PlayerPrefs key names.
        /// </summary>
        public static Dictionary<ERuntimeType, string> Paths = new()
        {
            {ERuntimeType.Progress, "progress" }, {ERuntimeType.Unlock, "unlock"}
        };
        
        /// <summary>
        /// Saves player progress data to PlayerPrefs with error handling.
        /// </summary>
        /// <param name="progress">The progress runtime data to save. If null, the operation is skipped.</param>
        public static void SaveProgress(ProgressRuntime progress)
        {
            if (progress == null) return;
            
            try
            {
                string value = progress.Serialize();
                PlayerPrefs.SetString(Paths[ERuntimeType.Progress], value);
                PlayerPrefs.Save();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save progress: {e.Message}");
            }
        }

        /// <summary>
        /// Loads player progress data from PlayerPrefs with error handling and fallback.
        /// </summary>
        /// <returns>A ProgressRuntime instance containing saved progress data, or an empty instance if loading fails.</returns>
        public static ProgressRuntime LoadProgress()
        {
            try
            {
                string value = PlayerPrefs.GetString(Paths[ERuntimeType.Progress], string.Empty);
                ProgressRuntime progress = new(value);
                return progress;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load progress: {e.Message}");
                return new ProgressRuntime(string.Empty);
            }
        }
    }
}