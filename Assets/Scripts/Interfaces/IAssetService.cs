using System;
using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages art assets and their loading/unloading
    /// </summary>
    public interface IAssetService
    {
        ILevelData GetLevelData(int levelId);

        /// <summary>
        /// Gets an illustration texture by ID
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration</param>
        /// <returns>The illustration texture</returns>
        void GetIllustration(int illustrationId, Action<Texture2D, Exception> callback);

        /// <summary>
        /// Generates puzzle pieces from an illustration
        /// </summary>
        /// <param name="illustration">The source illustration texture</param>
        /// <param name="gridSize">The size of the grid to split into. ex)3 -> 3 x 3 grid pieces</param>
        /// <returns>Array of puzzle piece sprites</returns>
        List<Sprite> GeneratePuzzlePieces(Texture2D illustration, int gridSize);

        /// <summary>
        /// Preloads required assets
        /// </summary>
        void PreloadAssets();

        /// <summary>
        /// Unloads unused assets to free memory
        /// </summary>
        void UnloadUnusedAssets();
    }
} 