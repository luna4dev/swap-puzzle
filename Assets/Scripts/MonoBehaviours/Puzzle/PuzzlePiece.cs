using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzlePiece : MonoBehaviour, IPuzzlePiece
    {
        [SerializeField] private UIDragDrop _uiDragDrop;
        [SerializeField] private PuzzlePieceRenderer _renderer;
        public IPuzzlePieceRenderer Renderer { get { return _renderer; } }
        public Vector2Int OriginalPos { get; private set; }
        public Vector2Int Pos { get; private set; }

        private int _displayNumber = 0;

        public void Initialize(IPuzzleController controller, Vector2Int originalPos, int displayNumber)
        {
            OriginalPos = originalPos;
            _displayNumber = displayNumber;

            // set ui dragdrop event
            _uiDragDrop.OnDrop.RemoveAllListeners();
            _uiDragDrop.OnDrop.AddListener(controller.HandlePuzzlePieceDrop);

            SetNormalState();
        }

        public bool IsSolved()
        {
            return OriginalPos == Pos;
        }

        public void SetPos(Vector2Int pos)
        {
            Pos = pos;
        }

        public void SetNormalState()
        {
            _uiDragDrop.enabled = true;
            _renderer.SetEnabled();
        }

        public void SetDisabledState()
        {
            _uiDragDrop.enabled = false;
            _renderer.SetDisabled();
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