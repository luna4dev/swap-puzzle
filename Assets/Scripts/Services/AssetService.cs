using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwapPuzzle.Services
{
    public enum EAssetType
    {
        Level,
        Illustration,
        Prefab,
    }

    public static class AssetService
    {
        private static void LoadAsset<T>(string path, Action<T, Exception> callback) where T : UnityEngine.Object
        {
            Addressables.LoadAssetAsync<T>(path).Completed += (request) =>
            {
                if (request.Status == AsyncOperationStatus.Succeeded)
                {
                    callback(request.Result, null);
                }
                else
                {
                    callback(null, new Exception("Failed to load asset: " + path));
                }
            };
        }

        public static void GetPrefab(string prefabName, Action<GameObject, Exception> callback)
        {
            string path = ResolveAssetPath(EAssetType.Prefab, prefabName);
            LoadAsset<GameObject>(path, callback);
        }

        public static async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            try
            {
                var loadHandle = Addressables.LoadAssetAsync<T>(path);
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

        public static async Task<Texture2D> GetIllustrationAsync(int illustrationId)
        {
            string path = ResolveIllustrationAssetPath(illustrationId);
            return await LoadAssetAsync<Texture2D>(path);
        }


        public static void GetIllustration(int illustrationId, Action<Texture2D, Exception> callback)
        {
            string path = ResolveIllustrationAssetPath(illustrationId);
            LoadAsset<Texture2D>(path, callback);
        }

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

        private static string ResolveIllustrationAssetPath(int illustrationId)
        {
            return ResolveAssetPath(EAssetType.Illustration, illustrationId.ToString("D3"));
        }

        private static string ResolveAssetPath(EAssetType assetType, string assetName)
        {
            switch (assetType)
            {
                case EAssetType.Illustration:
                    return "Assets/Sprites/Illustrations/" + assetName + ".png";
                case EAssetType.Level:
                    return "Assets/Levels/" + assetName + ".asset";
                case EAssetType.Prefab:
                    return "Assets/Prefabs/" + assetName + ".prefab";
                default:
                    return null;
            }
        }
    }
}