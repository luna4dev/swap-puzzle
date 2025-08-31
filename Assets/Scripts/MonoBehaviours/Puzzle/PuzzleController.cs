using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.AssetDefinitions;
using SwapPuzzle.Utilities;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        private IShuffler _shuffler;
        [SerializeField] private PuzzleGrid _puzzleGrid;
        [SerializeField] private PuzzleSpriteProvider _spriteProvider;
        // TODO: remove before release
        [SerializeField] private LevelData MockupLevelData;

        public void InitializePuzzle(ILevelData level)
        {
            _shuffler = new ControlledPlacement();

            _puzzleGrid.InitializeGrid(this, level.GridSize);
            RenderSpriteToPuzzlePieces(level);
            ShufflePieces(level.PresolvedPieces);
            CheckSolution();
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
                    _puzzleGrid.GetPieceAt(x, y).Renderer.SetImage(_spriteProvider.GetSprite(x, y));
                }
            }
        }

        public void ShufflePieces(int presolvedPiecesCount) 
        {
            _shuffler.Shuffle(_puzzleGrid, presolvedPiecesCount);
        }

        public void HandleSwap()
        {
            CheckSolution();
            bool completed = IsLevelComplete();

            if (completed) Debug.Log("yay");
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
