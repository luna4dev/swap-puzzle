using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Utilities;
using System;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        private IShuffler _shuffler;
        [SerializeField] private PuzzleGrid _puzzleGrid;
        [SerializeField] private PuzzleSpriteProvider _spriteProvider;

        private bool _debugMode = false;

        public void ToggleDebugMode()
        {
            // toggle
            SetDebug(!_debugMode);
        }

        private void SetDebug(bool debug)
        {
            _debugMode = debug;
            int gridSize = _puzzleGrid.GetGridSize();
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    _puzzleGrid.GetPieceAt(x, y).SetDebug(debug);
                }
            }
        }

        public void InitializePuzzle(ILevelData level)
        {
            _shuffler = new ControlledPlacement();

            _puzzleGrid.InitializeGrid(this, level.GridSize);
            RenderSpriteToPuzzlePieces(level);
            ShufflePieces(level.PresolvedPieces);
            CheckSolution();
            SetDebug(_debugMode);
        }

        public void RenderSpriteToPuzzlePieces(ILevelData levelData)
        {
            if (_spriteProvider == null)
            {
                throw new System.Exception("Sprite Renderer not found");
            }
            _spriteProvider.Initialize(levelData.Illustration.Illustration, levelData.GridSize);


            for (int y = 0; y < levelData.GridSize; y++)
            {
                for (int x = 0; x < levelData.GridSize; x++)
                {
                    _puzzleGrid.GetPieceAt(x, y).SetImage(_spriteProvider.GetSprite(x, y));
                }
            }
        }

        public void ShufflePieces(int presolvedPiecesCount)
        {
            _shuffler.Shuffle(_puzzleGrid, presolvedPiecesCount);
        }

        public bool CanSwapPieces(IPuzzlePiece _piece1, IPuzzlePiece _piece2)
        {
            if (_piece1 is PuzzlePiece piece1 && _piece2 is PuzzlePiece piece2)
            {
                if (piece1.IsSolved) return false;
                if (piece2.IsSolved) return false;
                if (piece1.Equals(piece2)) return false;
                return true;
            }
            return false;
        }

        public void HandleSwap()
        {
            CheckSolution();
            bool completed = IsLevelComplete();

            if (!completed) return;

            ILevelData currentLevel = ProgressManager.Instance.GetCurrentLevel();
            bool hasNextLevel = ProgressManager.Instance.HasNextLevel();
            ProgressManager.Instance.CompleteCurrentLevel();
            if (hasNextLevel) ProgressManager.Instance.GoToNextLevel();
            StartCoroutine(LevelCompletePopup.OpenPopup(currentLevel, hasNextLevel));
        }

        public void CheckSolution()
        {
            int gridSize = _puzzleGrid.GetGridSize();
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    IPuzzlePiece piece = _puzzleGrid.GetPieceAt(x, y);
                    if (x == piece.OriginalX && y == piece.OriginalY)
                    {
                        piece.SetSolved();
                    }
                }
            }
        }

        public bool IsLevelComplete()
        {
            bool completed = true;
            int gridSize = _puzzleGrid.GetGridSize();
            for (int y = 0; y < gridSize; y++)
            {
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
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    count += _puzzleGrid.GetPieceAt(x, y).IsSolved ? 1 : 0;
                }
            }
            return count;
        }
    }
}
