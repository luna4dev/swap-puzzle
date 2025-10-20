using UnityEngine;
using System.IO;

namespace SwapPuzzle.Utilities
{
    public class FilesystemSpriteLoader
    {
        private string illustrationsPath;

        public FilesystemSpriteLoader()
        {
#if UNITY_EDITOR
            // In Editor: Application.dataPath points to /path/to/project/Assets
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            illustrationsPath = Path.Combine(projectRoot, "Illustrations");
#else
            // In Build: Application.dataPath points to MyGame_Data (or .app/Contents on Mac)
            string baseFolder = Directory.GetParent(Application.dataPath).FullName;
            illustrationsPath = Path.Combine(baseFolder, "Illustrations");
#endif

            Debug.Log($"Looking for illustrations in: {illustrationsPath}");
        }

        public Sprite LoadSprite(string filename)
        {
            string filePath = Path.Combine(illustrationsPath, filename);

            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                return null;
            }

            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Auto-resizes to image dimensions

            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f // Pixels per unit
            );
        }

        public string[] GetAllPngFiles()
        {
            if (!Directory.Exists(illustrationsPath))
            {
                Directory.CreateDirectory(illustrationsPath);
                return new string[0];
            }

            return Directory.GetFiles(illustrationsPath, "*.png");
        }

        public string GetIllustrationsPath()
        {
            return illustrationsPath;
        }
    }
}
