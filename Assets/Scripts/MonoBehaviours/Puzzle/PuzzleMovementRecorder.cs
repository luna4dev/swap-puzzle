using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleMovement
    {
        Vector2Int PieceAOriginalPos;
        Vector2Int PieceBOriginalPos;
        bool SolvePieceA;
        bool SolvePieceB;

        public PuzzleMovement()
        {

        }
    }

    public class PuzzleMovementRecorder : MonoBehaviour
    {
        private List<PuzzleMovement> _movements;
        private bool _dirty = false;
        public bool Dirty { get { return _dirty; } }

        public void NotifySwap(PuzzlePiece pieceA, PuzzlePiece pieceB)
        {
            _dirty = true;
        }

        public void Cleanup()
        {
            _dirty = false;
        }
    }
}