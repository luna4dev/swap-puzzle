using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    /// <summary>
    /// A provider class that provides splited sprite
    /// It loads a source sprite and holds an 2D array of splitted sprites
    /// </summary>
    public class PuzzleSpriteProvider : MonoBehaviour
    {
        private Texture2D sourceTexture;
        private float pixelsPerUnit;
        private int gridSize;
        // Note that the index start from top left
        private Sprite[][] gridSprites;

        public void Initialize(Sprite sprite, int size)
        {
            sourceTexture = CropToSquare(sprite);
            pixelsPerUnit = sprite.pixelsPerUnit;
            gridSize = size;

            gridSprites = new Sprite[gridSize][];
            for (int i = 0; i < gridSize; i++)
            {
                gridSprites[i] = new Sprite[gridSize];
            }

            CreateGridSprites();
        }

        private Texture2D MakeTextureReadable(Texture2D source)
        {
            // Create a temporary RenderTexture
            RenderTexture tmp = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Linear
            );

            // Blit the texture to RenderTexture
            Graphics.Blit(source, tmp);

            // Backup the current RenderTexture
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmp;

            // Create readable texture
            Texture2D readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            readable.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            readable.Apply();

            // Restore state
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tmp);

            return readable;
        }

        private Texture2D CropToSquare(Sprite originalSprite)
        {
            Texture2D sourceTexture = originalSprite.texture;

            // Make texture readable if it isn't
            if (!sourceTexture.isReadable)
            {
                sourceTexture = MakeTextureReadable(sourceTexture);
            }

            Rect originalRect = originalSprite.rect;
            float width = originalRect.width;
            float height = originalRect.height;

            Color[] pixels;

            if (Mathf.Approximately(width, height))
            {
                // Create new texture from sprite rect
                Texture2D newTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
                pixels = sourceTexture.GetPixels(
                    (int)originalRect.x,
                    (int)originalRect.y,
                    (int)width,
                    (int)height
                );
                newTexture.SetPixels(pixels);
                newTexture.Apply();
                return newTexture;
            }

            int size = (int)Mathf.Min(width, height) - 1; // give some margin

            int offsetX = (int)((width - size) * 0.5f);
            int offsetY = (int)((height - size) * 0.5f);

            // Create new texture with cropped pixels
            Texture2D croppedTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            pixels = sourceTexture.GetPixels(
                (int)originalRect.x + offsetX,
                (int)originalRect.y + offsetY,
                size,
                size
            );
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }

        public void Cleanup()
        {
            if (gridSprites != null)
            {
                for (int row = 0; row < gridSize; row++)
                {
                    for (int col = 0; col < gridSize; col++)
                    {
                        if (gridSprites[row][col] != null)
                        {
                            // Destroy the texture first, then the sprite
                            if (gridSprites[row][col].texture != null)
                            {
                                DestroyImmediate(gridSprites[row][col].texture);
                            }
                            DestroyImmediate(gridSprites[row][col]);
                            gridSprites[row][col] = null;
                        }
                    }
                }
                gridSprites = null;
            }

            if (sourceTexture != null)
            {
                DestroyImmediate(sourceTexture);
            }

            sourceTexture = null;
            gridSize = 0;
        }

        public Sprite GetSprite(int x, int y)
        {
            if (gridSprites != null && x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            {
                return gridSprites[x][y];
            }
            return null;
        }

        private void CreateGridSprites()
        {
            // Calculate piece dimensions
            int pieceWidth = sourceTexture.width / gridSize;
            int pieceHeight = sourceTexture.height / gridSize;

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    // Calculate the position for this piece
                    int x = col * pieceWidth;
                    int y = row * pieceHeight;

                    // Create a new texture for this piece
                    Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight, TextureFormat.RGBA32, false);

                    // Copy pixels from source texture to piece texture
                    Color[] pixels = sourceTexture.GetPixels(x, y, pieceWidth, pieceHeight);
                    pieceTexture.SetPixels(pixels);
                    pieceTexture.Apply();

                    // Create sprite from the new texture (now UVs are 0-1 for the whole texture)
                    Sprite pieceSprite = Sprite.Create(
                        pieceTexture,
                        new Rect(0, 0, pieceWidth, pieceHeight), // Full texture rect
                        new Vector2(0.5f, 0.5f), // Pivot at center
                        pixelsPerUnit
                    );

                    // Name the sprite for easier debugging
                    pieceSprite.name = $"piece_{row}_{col}";

                    gridSprites[col][gridSize - row - 1] = pieceSprite;
                }
            }
        }
    }
}