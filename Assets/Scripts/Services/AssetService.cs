using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SwapPuzzle.AssetDefinitions;
using UnityEditor.VersionControl;

namespace SwapPuzzle.Services
{
    public enum EAssetType
    {
        LevelProgression,
        Level,
        GalleryProgression,
        Gallery,
        Illustration,
        Prefab,
    }

    public static class AssetService
    {
        private static Dictionary<string, AsyncOperationHandle> loadedAssets = new();
        
        public static Dictionary<EAssetType, string> Paths = new()
        {
            {EAssetType.Prefab, "Assets/Prefabs/"},
            {EAssetType.LevelProgression, "Assets/Data/"},
            {EAssetType.Level, "Assets/Data/Levels/"},
            {EAssetType.GalleryProgression, "Assets/Data/"},
            {EAssetType.Gallery, "Assets/Data/Galleries/"},
            {EAssetType.Illustration, "Assets/Data/Illustrations"},
        };

        private static async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            try
            {
                // handle generic type, ex) texture.png:Sprite vs texture.png:Texture2D
                string key = $"{path}:{typeof(T).Name}";

                if (loadedAssets.ContainsKey(key) && loadedAssets[key].IsValid())
                {
                    var cachedHandle = loadedAssets[key];
                    if (cachedHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        return (T)cachedHandle.Result;
                    }
                }

                var loadHandle = Addressables.LoadAssetAsync<T>(path);
                loadedAssets[key] = loadHandle;

                var result = await loadHandle.Task;

                if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    return result;
                }
                else
                {
                    throw new Exception($"Failed to load asset: {path}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading asset {path}: {ex.Message}");
            }
        }

        public static async Task<GameObject> GetPrefabAsync(string prefabName)
        {
            string path = ResolveAssetPath(EAssetType.Prefab, prefabName);
            return await LoadAssetAsync<GameObject>(path);
        }

        public static async Task<LevelData> GetLevelDataAsync(string levelName)
        {
            string path = ResolveAssetPath(EAssetType.Level, levelName);
            return await LoadAssetAsync<LevelData>(path);
        }

        public static async Task<LevelProgressionData> GetLevelProgressionDataAsync(string levelProgressionName) {
            string path = ResolveAssetPath(EAssetType.LevelProgression, levelProgressionName);
            return await LoadAssetAsync<LevelProgressionData>(path);
        }

        // TODO: move to better place
        public static List<Sprite> GeneratePuzzlePieces(Texture2D illustration, int gridSize)
        {
            // make texture2d to rectangular texture2d and get the pixels
            int textureLength = Mathf.Min(illustration.width, illustration.height);
            int offsetX = (illustration.width - textureLength) / 2;
            int offsetY = (illustration.height - textureLength) / 2;
            Color[] pixels = illustration.GetPixels(offsetX, offsetY, textureLength, textureLength);
            Texture2D croppedTexture = new Texture2D(textureLength, textureLength);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            // generate puzzle pieces
            List<Sprite> puzzlePieces = new();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Sprite piece = Sprite.Create(
                        croppedTexture,
                        new Rect(i * textureLength / gridSize, j * textureLength / gridSize, textureLength / gridSize, textureLength / gridSize),
                        new Vector2(0.5f, 0.5f),
                        100f
                    );
                    puzzlePieces.Add(piece);
                }
            }

            return puzzlePieces;
        }

        public static void PreloadAssets()
        {

        }

        public static void UnloadUnusedAssets()
        {

        }

        private static string ResolveAssetPath(EAssetType assetType, string assetName)
        {
            string basePath = Paths[assetType] + assetName;
            switch (assetType)
            {
                case EAssetType.Prefab:
                    return basePath + ".prefab";
                case EAssetType.LevelProgression:
                case EAssetType.Level:
                case EAssetType.GalleryProgression:
                case EAssetType.Gallery:
                case EAssetType.Illustration:
                    return basePath + ".asset";
                default:
                    return null;
            }
        }
    }
}