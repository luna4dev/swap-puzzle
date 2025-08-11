using UnityEngine;
using SwapPuzzle.Interfaces;
using TMPro;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzlePiece : MonoBehaviour, IPuzzlePiece
    {
        [SerializeField] private PuzzlePieceRenderer _renderer;
        public IPuzzlePieceRenderer Renderer { get { return _renderer; } }
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }
        private bool _isSolved;
        public bool IsSolved
        {
            get { return _isSolved; }
            set
            {
                Renderer.SetSolvedState(value);
                _isSolved = value;
            }
        }

        public void Initialize(int originalX, int originalY, int order = -1)
        {
            OriginalX = originalX;
            OriginalY = originalY;
            IsSolved = false;
            if (order > 0) SetDebug(true, order);
            else SetDebug(false);
        }

        public void SetDebug(bool debug, int order = 0)
        {
            Renderer.SetDebugText(debug, order);
        }

        public void MarkAsSolved()
        {
            IsSolved = true;
        }

        public void OnPieceSelected()
        {

        }
    }
}