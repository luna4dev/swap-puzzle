using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEngine.EventSystems;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzlePiece : MonoBehaviour, IPuzzlePiece
    {
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }
        public bool IsSolved { get; private set; }
        public bool IsLocked { get; private set; }

        public void Initialize(IPuzzlePiece pieceData)
        {

        }

        public void MarkAsSolved()
        {

        }

        public void OnPieceSelected()
        {

        }
    }
}