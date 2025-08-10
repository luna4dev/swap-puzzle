using UnityEngine;
using SwapPuzzle.Interfaces;
using TMPro;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzlePiece : MonoBehaviour, IPuzzlePiece
    {
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }
        public bool IsSolved { get; private set; }

        [SerializeField] private TMP_Text _debugText;
        [SerializeField] private Image _image;

        public void Initialize(int originalX, int originalY, int order = -1)
        {
            OriginalX = originalX;
            OriginalY = originalY;

            if (order > 0) SetDebug(true, order);
            else SetDebug(false);
        }

        public void SetDebug(bool debug, int order = 0)
        {
            if (debug)
            {
                _debugText.text = "" + order;
                _debugText.gameObject.SetActive(true);
            }
            else
            {
                _debugText.gameObject.SetActive(false);
            }
        }

        public void SetImage(Sprite sprite) 
        {
            _image.sprite = sprite;
            SetDebugTextColor();
        }

        private void SetDebugTextColor()
        {
            if (_image.sprite == null || _debugText == null) return;

            float luminance = CalculateSpriteLuminance(_image.sprite);
            
            if (luminance > 0.5f)
            {
                _debugText.color = Color.black;
            }
            else
            {
                _debugText.color = Color.white;
            }
        }

        private float CalculateSpriteLuminance(Sprite sprite)
        {
            if (sprite?.texture == null) return 0.5f;

            Texture2D texture = sprite.texture;
            Rect spriteRect = sprite.rect;
            
            if (!texture.isReadable)
            {
                return 0.5f;
            }

            Color[] pixels = texture.GetPixels(
                (int)spriteRect.x,
                (int)spriteRect.y,
                (int)spriteRect.width,
                (int)spriteRect.height
            );

            float totalLuminance = 0f;
            int sampleCount = 0;
            
            int step = Mathf.Max(1, pixels.Length / 1000);
            
            for (int i = 0; i < pixels.Length; i += step)
            {
                Color pixel = pixels[i];
                if (pixel.a > 0.1f)
                {
                    float luminance = 0.299f * pixel.r + 0.587f * pixel.g + 0.114f * pixel.b;
                    totalLuminance += luminance;
                    sampleCount++;
                }
            }

            return sampleCount > 0 ? totalLuminance / sampleCount : 0.5f;
        }

        public void MarkAsSolved()
        {

        }

        public void OnPieceSelected()
        {

        }
    }
}