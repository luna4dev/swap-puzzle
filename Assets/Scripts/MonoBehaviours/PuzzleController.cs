using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Services;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        public void InitializePuzzle(int levelId)
        {
            Debug.Log("Dev Stage: using mock data");

            // Request the art asset manager to generate the puzzle pieces
            var assetService = AssetService.Instance;
            ILevelData levelData = assetService.GetLevelData(levelId);
            var puzzlePieces = assetService.GeneratePuzzlePieces(levelData.Illustration, levelData.GridSize);

            // Initialize the puzzle pieces
            foreach (var piece in puzzlePieces)
            {
                var puzzlePiece = Instantiate(piece, transform);
            }

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
