using System;
using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleGrid : MonoBehaviour, IPuzzleGrid
    {
        public event Action OnSwap;

        /// <summary>
        /// a length and height of the puzzle piece
        /// </summary>
        private float puzzlePieceSize;
        private IPuzzlePiece[][] grid;
        [SerializeField] private PuzzlePiece piecePrefab;

        public void InitializeGrid(int _gridSize)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Only in play mode");
                return;
            }

            // initialize grid 2d list 
            grid = new IPuzzlePiece[_gridSize][];
            for (int i = 0; i < _gridSize; i++)
            {
                grid[i] = new IPuzzlePiece[_gridSize];
            }

            InitializePuzzlePieces();
        }

        public IPuzzlePiece GetPieceAt(int x, int y)
        {
            return grid[y][x];
        }

        public void SetPieceAt(int x, int y, IPuzzlePiece _piece)
        {
            if (_piece is PuzzlePiece piece)
            {
                float offset = grid.Length % 2 == 0 ? puzzlePieceSize / 2 : 0;
                float rectX = puzzlePieceSize * (x - grid.Length / 2) + offset;
                float rectY = -(puzzlePieceSize * (y - grid.Length / 2) + offset);
                piece.GetComponent<RectTransform>().localPosition = new(rectX, rectY);

                grid[y][x] = _piece;
            }
        }

        public void InitiateSwap(IPuzzlePiece piece1, IPuzzlePiece piece2, bool emitEvent = false)
        {
            (int piece1X, int piece1Y) = GetCoord(piece1);
            if (piece1X == -1) return;

            (int piece2X, int piece2Y) = GetCoord(piece2);
            if (piece2X == -1) return;

            SetPieceAt(piece1X, piece1Y, piece2);
            SetPieceAt(piece2X, piece2Y, piece1);

            if (emitEvent) OnSwap.Invoke();
        }

        public void HandlePieceSelection(IPuzzlePiece selectedPiece)
        {
            // TODO
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

        public void ClearSelection()
        {
            // TODO
        }

        public IPuzzlePiece GetSelectedPiece()
        {
            return default;
        }

        public int GetGridSize()
        {
            return grid.Length;
        }

        private (int, int) GetCoord(IPuzzlePiece _piece)
        {
            if (_piece is PuzzlePiece piece)
            {
                for (int y = 0; y < grid.Length; y++)
                {
                    for (int x = 0; x < grid[y].Length; x++)
                    {
                        if (grid[y][x].Equals(piece)) return (x, y);
                    }
                }
            }
            return (-1, -1);
        }


        public void InitializePuzzlePieces()
        {
            if (grid == null || grid.Length <= 0)
            {
                Debug.LogWarning("GridSize not properly initialized");
                return;
            }

            // set size of the puzzle piece
            RectTransform rectTransform = GetComponent<RectTransform>();
            float areaLength = rectTransform.sizeDelta.x;
            puzzlePieceSize = areaLength / grid.Length;

            // set desired number of puzzle pieces
            int newGridItemCount = grid.Length * grid.Length;
            int existingGridItemCount = 0;

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out PuzzlePiece piece))
                {
                    existingGridItemCount++;
                }
                child.gameObject.SetActive(false);
            }

            // instantiate puzzle pieces
            int diff = newGridItemCount - existingGridItemCount;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    var newObj = Instantiate(piecePrefab, transform);
                    newObj.gameObject.SetActive(false);
                }
            }

            int x = 0, y = 0;
            foreach (Transform child in transform)
            {
                // initialize piece object
                PuzzlePiece piece = child.GetComponent<PuzzlePiece>();
                piece.Initialize(x, y, x + y * grid.Length + 1);

                // initialize piece rect transform
                RectTransform rect = piece.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(puzzlePieceSize, puzzlePieceSize);

                // set piece position
                SetPieceAt(x, y, piece);

                // set ui dragdrop event
                UIDragDrop uiDragDrop = piece.GetComponent<UIDragDrop>();
                uiDragDrop.OnDrop.RemoveAllListeners();
                uiDragDrop.OnDrop.AddListener(HandlePuzzlePieceDrop);

                child.gameObject.SetActive(true);
                x++;
                if (x >= grid.Length)
                {
                    x = 0;
                    y++;
                }
                if (y >= grid.Length) break;
            }
        }

        private void HandlePuzzlePieceDrop()
        {
            var dropped = UIDragDrop.Dropped.GetComponent<PuzzlePiece>();
            var dropTarget = UIDragDrop.DropTarget.GetComponent<PuzzlePiece>();

            // check valid
            if (dropped == null) return;
            if (dropTarget == null) return;
            if (!CanSwapPieces(dropped, dropTarget)) return;

            // swap
            InitiateSwap(dropped, dropTarget, true);
        }
    }
}