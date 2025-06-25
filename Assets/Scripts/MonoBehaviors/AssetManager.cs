using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviors
{
    public enum EAssetType{
        Level,
        Illustration,
        Prefab,
    }

    public class AssetManager : MonoBehaviour, IAssetManager
    {
        public static AssetManager Instance { get; private set; } = null;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public ILevelData GetLevelData(int levelId)
        {
            return null;
        }

        public Texture2D GetIllustration(int illustrationId)
        {
            string path = ResolveIllustrationAssetPath(illustrationId);
            var locations = Addressables.LoadResourceLocationsAsync(path).WaitForCompletion();
            if (locations.Count == 0)
            {
                Debug.LogWarning(path + " - doesn't exists");
                return null;
            }
            // 주소가 존재하면 로드
            var request = Addressables.LoadAssetAsync<Texture2D>(path);
            request.WaitForCompletion();
            return request.Result;
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