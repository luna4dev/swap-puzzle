using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SwapPuzzle.Services
{
    public enum EAssetType
    {
        Level,
        Illustration,
        Prefab,
    }

    public class AssetService : IAssetService
    {
        private static AssetService _instance = null;
        public static AssetService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssetService();
                }
                return _instance;
            }
        }

        private void LoadAsset<T>(string path, Action<T, Exception> callback) where T: UnityEngine.Object {
            Addressables.LoadAssetAsync<T>(path).Completed += (request) => {
                if (request.Status == AsyncOperationStatus.Succeeded) {
                    callback(request.Result, null);
                } else {
                    callback(null, new Exception("Failed to load asset: " + path));
                }
            };
        }

        public ILevelData GetLevelData(int levelId)
        {
            return null;
        }

        public void GetPrefab(string prefabName, Action<GameObject, Exception> callback) {
            string path = ResolveAssetPath(EAssetType.Prefab, prefabName);
            LoadAsset<GameObject>(path, callback); 
        }

        public void GetIllustration(int illustrationId, Action<Texture2D, Exception> callback)
        {
            string path = ResolveIllustrationAssetPath(illustrationId);
            LoadAsset<Texture2D>(path, callback);
        }

        public List<Sprite> GeneratePuzzlePieces(Texture2D illustration, int gridSize)
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

        public void PreloadAssets()
        {

        }

        public void UnloadUnusedAssets()
        {

        }

        private string ResolveIllustrationAssetPath(int illustrationId)
        {
            return ResolveAssetPath(EAssetType.Illustration, illustrationId.ToString("D3"));
        }

        private string ResolveAssetPath(EAssetType assetType, string assetName)
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