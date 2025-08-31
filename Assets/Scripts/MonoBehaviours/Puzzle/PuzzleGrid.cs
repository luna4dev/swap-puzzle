using System;
using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzleGrid : MonoBehaviour, IPuzzleGrid
    {

        /// <summary>
        /// a length and height of the puzzle piece
        /// </summary>
        private float _puzzlePieceSize;
        private IPuzzlePiece[][] _grid;
        private IPuzzleController _controller;

        [SerializeField] private PuzzlePiece piecePrefab;

        public void InitializeGrid(IPuzzleController controller, int _gridSize)
        {
            _controller = controller;

            // initialize grid 2d list 
            _grid = new IPuzzlePiece[_gridSize][];
            for (int i = 0; i < _gridSize; i++)
            {
                _grid[i] = new IPuzzlePiece[_gridSize];
            }

            InitializePuzzlePieces();
        }

        public void ClearGrid()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public IPuzzlePiece GetPieceAt(int x, int y)
        {
            return _grid[y][x];
        }

        public void SetPieceAt(int x, int y, IPuzzlePiece _piece)
        {
            if (_piece is PuzzlePiece piece)
            {
                float offset = _grid.Length % 2 == 0 ? _puzzlePieceSize / 2 : 0;
                float rectX = _puzzlePieceSize * (x - _grid.Length / 2) + offset;
                float rectY = -(_puzzlePieceSize * (y - _grid.Length / 2) + offset);
                piece.GetComponent<RectTransform>().localPosition = new(rectX, rectY);

                _grid[y][x] = _piece;
            }
        }

        public void InitiateSwap(IPuzzlePiece piece1, IPuzzlePiece piece2)
        {
            (int piece1X, int piece1Y) = GetCoord(piece1);
            if (piece1X == -1) return;

            (int piece2X, int piece2Y) = GetCoord(piece2);
            if (piece2X == -1) return;

            SetPieceAt(piece1X, piece1Y, piece2);
            SetPieceAt(piece2X, piece2Y, piece1);

            if (_controller != null) _controller.HandleSwap();
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
            return _grid.Length;
        }

        private (int, int) GetCoord(IPuzzlePiece _piece)
        {
            if (_piece is PuzzlePiece piece)
            {
                for (int y = 0; y < _grid.Length; y++)
                {
                    for (int x = 0; x < _grid[y].Length; x++)
                    {
                        if (_grid[y][x].Equals(piece)) return (x, y);
                    }
                }
            }
            return (-1, -1);
        }


        public void InitializePuzzlePieces()
        {
            if (_grid == null || _grid.Length <= 0)
            {
                Debug.LogWarning("GridSize not properly initialized");
                return;
            }

            // set size of the puzzle piece
            RectTransform rectTransform = GetComponent<RectTransform>();
            float areaLength = rectTransform.sizeDelta.x;
            _puzzlePieceSize = areaLength / _grid.Length;

            // set desired number of puzzle pieces
            int newGridItemCount = _grid.Length * _grid.Length;
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
                piece.Initialize(x, y, x + y * _grid.Length + 1);

                // initialize piece rect transform
                RectTransform rect = piece.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(_puzzlePieceSize, _puzzlePieceSize);

                // set piece position
                SetPieceAt(x, y, piece);

                // set ui dragdrop event
                UIDragDrop uiDragDrop = piece.GetComponent<UIDragDrop>();
                uiDragDrop.OnDrop.RemoveAllListeners();
                uiDragDrop.OnDrop.AddListener(HandlePuzzlePieceDrop);

                child.gameObject.SetActive(true);
                x++;
                if (x >= _grid.Length)
                {
                    x = 0;
                    y++;
                }
                if (y >= _grid.Length) break;
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
            InitiateSwap(dropped, dropTarget);
        }
    }
}