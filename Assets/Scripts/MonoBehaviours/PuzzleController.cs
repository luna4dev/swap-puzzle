using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Services;

namespace SwapPuzzle.MonoBehaviours
{
    [RequireComponent(typeof(GridSystem))]
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        private GridSystem _gridSystem;
        public GridSystem GridSystem
        {
            get
            {
                if (_gridSystem == null) _gridSystem = GetComponent<GridSystem>();
                return _gridSystem;
            }
        }

        public void InitializePuzzle(int levelId)
        {
            // TODO: remove mockup
            

        }

        public void SwapPieces(PuzzlePiece a, PuzzlePiece b) {
            
        }

        public void ShufflePieces()
        {
            throw new System.NotImplementedException();
        }

        public void CheckSolution()
        {
            throw new System.NotImplementedException();
        }

        public bool IsLevelComplete()
        {
            throw new System.NotImplementedException();
        }

        public int GetSolvedPiecesCount()
        {
            throw new System.NotImplementedException();
        }
    }
}
