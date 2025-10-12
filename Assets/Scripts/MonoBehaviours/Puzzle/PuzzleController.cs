using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Utilities;
using System;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        const int INITIAL_PUZZLE_PIECE_POOL_COUNT = 64;
        private IShuffler _shuffler = new ControlledPlacement();
        private bool _debugMode = false;
        [SerializeField] private PuzzleGrid _puzzleGrid;
        [SerializeField] private PuzzleSpriteProvider _spriteProvider;
        [SerializeField] private PuzzleScoreSystem _scoreSystem;
        [SerializeField] private PuzzlePieceProvider _puzzlePieceProvider;

        public void ToggleDebugMode()
        {
            // toggle
            SetDebug(!_debugMode);
        }

        public void InitializePuzzle(ILevelData level)
        {
            _puzzlePieceProvider.Prewarm(INITIAL_PUZZLE_PIECE_POOL_COUNT);
            _puzzleGrid.InitializeGrid(this, level.GridSize);
            RenderSpriteToPuzzlePieces(level);
            ShufflePieces(level.PresolvedPieces);
            UpdatePuzzlePieceState();
            SetDebug(_debugMode);
        }

        public void ClearPuzzle()
        {
            _puzzleGrid.ClearGrid();
            _scoreSystem.Clear();
        }

        private void RenderSpriteToPuzzlePieces(ILevelData levelData)
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

        private void ShufflePieces(int presolvedPiecesCount)
        {
            _shuffler.Shuffle(_puzzleGrid, presolvedPiecesCount);
        }

        private void UpdatePuzzlePieceState()
        {
            for (int y = 0; y < _puzzleGrid.GridSize; y++)
            {
                for (int x = 0; x < _puzzleGrid.GridSize; x++)
                {
                    IPuzzlePiece piece = _puzzleGrid.GetPieceAt(x, y);
                    if (piece.IsSolved()) piece.SetDisabledState();
                    else piece.SetNormalState();
                }
            }
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

        public void HandlePuzzlePieceDrop()
        {
            var dropped = UIDragDrop.Dropped.GetComponent<PuzzlePiece>();
            var dropTarget = UIDragDrop.DropTarget.GetComponent<PuzzlePiece>();

            // check valid
            if (dropped == null) return;
            if (dropTarget == null) return;
            if (!CanSwapPieces(dropped, dropTarget)) return;

            // swap
            _puzzleGrid.Swap(dropped, dropTarget);

            // if solved, set piece to disabled
            if (dropped.IsSolved()) dropped.SetDisabledState();
            if (dropTarget.IsSolved()) dropTarget.SetDisabledState();

            // notify score system
            NotifyScoreSystem(dropped, dropTarget);

            if (IsLevelComplete()) HandleLevelComplete();
        }

        private bool CanSwapPieces(IPuzzlePiece piece1, IPuzzlePiece piece2)
        {
            if (piece1.IsSolved()) return false;
            if (piece2.IsSolved()) return false;
            if (piece1.Equals(piece2)) return false;
            return true;
        }

        private void NotifyScoreSystem(IPuzzlePiece piece1, IPuzzlePiece piece2)
        {
            bool piece1Solved = piece1.IsSolved();
            bool piece2Solved = piece2.IsSolved();
            if (piece1Solved && piece2Solved) _scoreSystem.Notify(EPuzzleSolveType.SolveBoth);
            if (piece1Solved ^ piece2Solved) _scoreSystem.Notify(EPuzzleSolveType.SolveOne);
            if (!piece1Solved && !piece2Solved) _scoreSystem.Notify(EPuzzleSolveType.Fail);
        }

        private bool IsLevelComplete()
        {
            bool completed = true;
            int gridSize = _puzzleGrid.GridSize;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    completed = completed && _puzzleGrid.GetPieceAt(x, y).IsSolved();
                }
            }
            return completed;
        }

        private void HandleLevelComplete()
        {
            _scoreSystem.Notify(EPuzzleSolveType.PuzzleWin);

            // finalize level data
            ILevelData currentLevel = ProgressManager.Instance.GetCurrentLevel();
            bool hasNextLevel = ProgressManager.Instance.HasNextLevel();

            // Notify Current level is completed
            ProgressManager.Instance.CompleteCurrentLevel();
            if (hasNextLevel) ProgressManager.Instance.GoToNextLevel();
            StartCoroutine(LevelCompletePopup.OpenPopup(currentLevel, hasNextLevel));
        }
    }
}
