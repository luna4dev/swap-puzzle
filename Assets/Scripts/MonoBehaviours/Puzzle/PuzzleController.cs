using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Utilities;
using System;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        const int INITIAL_PUZZLE_PIECE_POOL_COUNT = 64;

        private IShuffler _shuffler;
        [SerializeField] private PuzzleGrid _puzzleGrid;
        [SerializeField] private PuzzleSpriteProvider _spriteProvider;
        [SerializeField] private PuzzleScoreSystem _scoreSystem;
        [SerializeField] private PuzzlePieceProvider _puzzlePieceProvider;

        private bool _debugMode = false;

        public void ToggleDebugMode()
        {
            // toggle
            SetDebug(!_debugMode);
        }

        private void SetDebug(bool debug)
        {
            _debugMode = debug;
            int gridSize = _puzzleGrid.GridSize;
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

            _puzzlePieceProvider.Prewarm(INITIAL_PUZZLE_PIECE_POOL_COUNT);
            _puzzleGrid.InitializeGrid(this, level.GridSize);
            RenderSpriteToPuzzlePieces(level);
            ShufflePieces(level.PresolvedPieces);
            CheckSolutionAndMarkScore();
            SetDebug(_debugMode);
        }

        public void RenderSpriteToPuzzlePieces(ILevelData levelData)
        {
            if (_spriteProvider == null)
            {
                throw new Exception("Sprite Renderer not found");
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
            //TODO: initiate swap
            CheckSolutionAndMarkScore(true);
            bool completed = IsLevelComplete();

            if (!completed) return;
            _scoreSystem.Notify(EPuzzleSolveType.PuzzleWin);
            ILevelData currentLevel = ProgressManager.Instance.GetCurrentLevel();
            bool hasNextLevel = ProgressManager.Instance.HasNextLevel();
            ProgressManager.Instance.CompleteCurrentLevel();
            if (hasNextLevel) ProgressManager.Instance.GoToNextLevel();
            StartCoroutine(LevelCompletePopup.OpenPopup(currentLevel, hasNextLevel));
        }

        public void CheckSolutionAndMarkScore(bool notifyScore = false)
        {
            int gridSize = _puzzleGrid.GridSize;

            int solveCount = 0;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    IPuzzlePiece piece = _puzzleGrid.GetPieceAt(x, y);
                    if (x == piece.OriginalPos.x && y == piece.OriginalPos.y)
                    {
                        piece.SetSolved();
                        solveCount++;
                    }
                }
            }

            if (!notifyScore) return;

            Debug.Log(solveCount);

            if (solveCount == 0) _scoreSystem.Notify(EPuzzleSolveType.Fail);
            if (solveCount == 1) _scoreSystem.Notify(EPuzzleSolveType.SolveOne);
            if (solveCount == 2) _scoreSystem.Notify(EPuzzleSolveType.SolveBoth);
        }

        public bool IsLevelComplete()
        {
            bool completed = true;
            int gridSize = _puzzleGrid.GridSize;
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
            int gridSize = _puzzleGrid.GridSize;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    count += _puzzleGrid.GetPieceAt(x, y).IsSolved ? 1 : 0;
                }
            }
            return count;
        }

        public void HandlePuzzlePieceDrop()
        {
            var dropped = UIDragDrop.Dropped.GetComponent<PuzzlePiece>();
            var dropTarget = UIDragDrop.DropTarget.GetComponent<PuzzlePiece>();

            // check valid
            if (dropped == null) return;
            if (dropTarget == null) return;
            if (!CanSwapPieces(dropped, dropTarget)) return;

            // swap
            _puzzleGrid.InitiateSwap(dropped, dropTarget);

            HandleSwap();
        }
    }
}
