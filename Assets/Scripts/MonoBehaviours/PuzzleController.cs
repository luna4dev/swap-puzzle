using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Services;
using SwapPuzzle.AssetDefinitions;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        [SerializeField] private PuzzleGrid _puzzleGrid;

        public void InitializePuzzle(int levelId)
        {
            // TODO: remove mockup
            // mockup
            LevelData levelData = new()
            {
                LevelId = 999,
                GridSize = 4,
                PreSolvedPieces = 3
            };

            _puzzleGrid.InitializeGrid(levelData.GridSize);
            ShufflePieces();
        }

        public void ShufflePieces()
        {
            int gridSize = _puzzleGrid.GetGridSize();

            // Create list of all pieces
            List<IPuzzlePiece> pieces = new List<IPuzzlePiece>();
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    pieces.Add(_puzzleGrid.GetPieceAt(x, y));
                }
            }

            // Fisher-Yates shuffle - guarantees every piece moves
            for (int i = pieces.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (pieces[i], pieces[j]) = (pieces[j],pieces[i]);
            }

            // Put shuffled pieces back
            int index = 0;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    _puzzleGrid.SetPieceAt(x, y, pieces[index++]);
                }
            }
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
