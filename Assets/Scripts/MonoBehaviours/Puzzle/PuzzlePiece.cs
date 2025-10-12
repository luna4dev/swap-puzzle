using UnityEngine;
using SwapPuzzle.Interfaces;
using TMPro;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzlePiece : MonoBehaviour, IPuzzlePiece
    {
        [SerializeField] private UIDragDrop _uiDragDrop;
        [SerializeField] private PuzzlePieceRenderer _renderer;
        public IPuzzlePieceRenderer Renderer { get { return _renderer; } }
        public Vector2Int OriginalPos { get; private set; }

        public bool IsSolved { get; private set; }
        private int _displayNumber = 0;

        public void Initialize(IPuzzleController controller, Vector2Int originalPos, int displayNumber)
        {
            OriginalPos = originalPos;
            _displayNumber = displayNumber;

            // set ui dragdrop event
            _uiDragDrop.OnDrop.RemoveAllListeners();
            _uiDragDrop.OnDrop.AddListener(controller.HandlePuzzlePieceDrop);

            SetPrestine();
        }

        public void SetPrestine()
        {
            _uiDragDrop.enabled = true;
            _renderer.SetEnabled();
            IsSolved = false;
        }

        public void SetSolved()
        {
            _uiDragDrop.enabled = false;
            _renderer.SetDisabled();
            IsSolved = true;
        }

        public void SetLevelCompleted()
        {
            _uiDragDrop.enabled = false;
            _renderer.SetEnabled();
            IsSolved = true;
        }

        public void SetImage(Sprite image)
        {
            _renderer.SetImage(image);
        }

        public void SetDebug(bool debug)
        {
            Renderer.SetDebug(debug, _displayNumber);
        }
    }
}