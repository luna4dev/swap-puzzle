using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEngine.EventSystems;

namespace SwapPuzzle.MonoBehaviors
{
    public class PuzzlePiece : MonoBehaviour, IPuzzlePiece, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }
        public bool IsSolved { get; private set; }
        public bool IsLocked { get; private set; }

        public void Initialize(IPuzzlePiece pieceData)
        {

        }

        public void SetPosition(int x, int y)
        {

        }

        public void MarkAsSolved()
        {

        }

        public void LockPiece()
        {

        }

        public void OnPieceSelected()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}