using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    /// <summary>
    /// A provider class that provides splited sprite
    /// It loads a source sprite and holds an 2D array of splitted sprites
    /// </summary>
    public class PuzzleSpriteProvider : MonoBehaviour
    {
        private Sprite sourceSprite;
        private int gridSize;
        // Note that the index start from top left
        private Sprite[][] gridSprites;

        public void Initialize(Sprite sprite, int size)
        {
            sourceSprite = CropToSquare(sprite);
            gridSize = size;
            
            gridSprites = new Sprite[gridSize][];
            for (int i = 0; i < gridSize; i++)
            {
                gridSprites[i] = new Sprite[gridSize];
            }
            
            CreateGridSprites();
        }

        private Sprite CropToSquare(Sprite originalSprite)
        {
            Rect originalRect = originalSprite.rect;
            float width = originalRect.width;
            float height = originalRect.height;
            
            if (Mathf.Approximately(width, height))
            {
                return originalSprite;
            }
            
            float size = Mathf.Min(width, height);
            
            float offsetX = (width - size) * 0.5f;
            float offsetY = (height - size) * 0.5f;
            
            Rect squareRect = new Rect(
                originalRect.x + offsetX,
                originalRect.y + offsetY,
                size - 0.1f, // give some margin
                size - 0.1f  // give some margin
            );
            
            Sprite newCroppedSprite = Sprite.Create(
                originalSprite.texture,
                squareRect,
                new Vector2(0.5f, 0.5f),
                originalSprite.pixelsPerUnit
            );
            
            newCroppedSprite.name = $"{originalSprite.name}_cropped_square";
            return newCroppedSprite;
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
                            DestroyImmediate(gridSprites[row][col]);
                            gridSprites[row][col] = null;
                        }
                    }
                }
                gridSprites = null;
            }
            
            if (sourceSprite != null)
            {
                DestroyImmediate(sourceSprite);
            }
            
            sourceSprite = null;
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
            Texture2D sourceTexture = sourceSprite.texture;

            // Calculate piece dimensions
            float pieceWidth = sourceSprite.rect.width / gridSize;
            float pieceHeight = sourceSprite.rect.height / gridSize;

            // Get the starting position of the sprite in the texture
            float startX = sourceSprite.rect.x;
            float startY = sourceSprite.rect.y;

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    // Calculate the rect for this piece
                    float x = startX + (col * pieceWidth);
                    float y = startY + (row * pieceHeight);

                    Rect pieceRect = new Rect(x, y, pieceWidth, pieceHeight);

                    // Create sprite from the piece
                    Sprite pieceSprite = Sprite.Create(
                        sourceTexture,
                        pieceRect,
                        new Vector2(0.5f, 0.5f), // Pivot at center
                        sourceSprite.pixelsPerUnit
                    );

                    // Name the sprite for easier debugging
                    pieceSprite.name = $"{sourceSprite.name}_piece_{row}_{col}";

                    gridSprites[col][gridSize - row - 1] = pieceSprite;
                }
            }
        }
    }
}