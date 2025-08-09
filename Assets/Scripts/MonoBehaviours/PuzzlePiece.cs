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

        public void Initialize(int originalX, int originalY, int num, bool isDebug = true)
        {
            OriginalX = originalX;
            OriginalY = originalY;

            // setup debug
            if (isDebug)
            {
                Color color = Random.ColorHSV();
                float luminance = color.r * 0.2126f + color.g * 0.7152f + color.g * 0.0722f;

                _image.color = color;

                _debugText.text = "" + num;
                _debugText.color = luminance > 0.5f ? Color.black : Color.white;
                _debugText.gameObject.SetActive(true);
            }
        }

        public void MarkAsSolved()
        {

        }

        public void OnPieceSelected()
        {

        }

        public void SetDebug(bool isDebug, int number)
        {

            if (_debugText && _image)
            {
                Color color = Random.ColorHSV();
                float luminance = color.r * 0.2126f + color.g * 0.7152f + color.g * 0.0722f;

                _image.color = color;

                _debugText.text = "" + number;
                _debugText.color = luminance > 0.5f ? Color.black : Color.white;
                _debugText.gameObject.SetActive(isDebug);
            }
        }
    }
}