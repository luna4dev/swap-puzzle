using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Services;
using SwapPuzzle.AssetDefinitions;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        private bool _initialized;
        [SerializeField] private PuzzleGrid _puzzleGrid;
        [SerializeField] private PuzzleSpriteProvider _spriteProvider;
        // TODO: remove before release
        [SerializeField] private LevelData MockupLevelData;

        public void OnEnable()
        {
            if (_initialized == false) return;

            _puzzleGrid.OnSwap += HandleSwap;
        }

        public void OnDisable()
        {
            _puzzleGrid.OnSwap -= HandleSwap;
        }

        public void InitializePuzzle(int levelId)
        {
            // TODO: remove mockup
            LevelData levelData = MockupLevelData;
            _puzzleGrid.InitializeGrid(levelData.GridSize);
            _puzzleGrid.OnSwap += HandleSwap;
            RenderSpriteToPuzzlePieces(levelData);
            ShufflePieces();

            _initialized = true;
        }

        public void RenderSpriteToPuzzlePieces(LevelData levelData)
        {
            if (_spriteProvider == null)
            {
                throw new System.Exception("Sprite Renderer not found");
            }
            _spriteProvider.Initialize(levelData.Illustration, levelData.GridSize);


            for (int y = 0; y < levelData.GridSize; y++)
            {
                for (int x = 0; x < levelData.GridSize; x++)
                {
                    _puzzleGrid.GetPieceAt(x, y).Renderer.SetImage(_spriteProvider.GetSprite(x, y));
                }
            }
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
                (pieces[i], pieces[j]) = (pieces[j], pieces[i]);
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

        public void HandleSwap()
        {
            CheckSolution();
            IsLevelComplete();
        }

        public void CheckSolution()
        {
            int gridSize = _puzzleGrid.GetGridSize();
            for (int y = 0; y < gridSize; y++) {
                for (int x = 0; x < gridSize; x++)
                {
                    IPuzzlePiece piece = _puzzleGrid.GetPieceAt(x, y);
                    if (x == piece.OriginalX && y == piece.OriginalY)
                    {
                        piece.MarkAsSolved();
                    }
                }
            }
        }

        public bool IsLevelComplete()
        {
            bool completed = true;
            int gridSize = _puzzleGrid.GetGridSize();
            for (int y = 0; y < gridSize; y++) {
                for (int x = 0; x < gridSize; x++)
                {
                    completed = completed && _puzzleGrid.GetPieceAt(x, y).IsSolved;
                }
            }
            return completed;
        }

        public int GetSolvedPiecesCount()
        {
            int count = 0;
            int gridSize = _puzzleGrid.GetGridSize();
            for (int y = 0; y < gridSize; y++) {
                for (int x = 0; x < gridSize; x++)
                {
                    count += _puzzleGrid.GetPieceAt(x, y).IsSolved ? 1 : 0;
                }
            }
            return count;
        }
    }
}
