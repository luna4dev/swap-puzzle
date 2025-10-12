using System;
using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    /// <summary>
    /// 
    /// </summary>
    public class PuzzleGrid : MonoBehaviour, IPuzzleGrid
    {

        /// <summary>
        /// a length and height of the puzzle piece
        /// </summary>
        private float _puzzlePieceSize;
        public int GridSize { get; private set; }
        private List<IPuzzlePiece> _puzzlePieces = new();
        [SerializeField] PuzzlePieceProvider _puzzlePieceProvider;

        private int CalcIdxFromCoord(int x, int y)
        {
            return (GridSize * y) + x;
        }

        public void InitializeGrid(IPuzzleController controller, int gridSize)
        {
            // set grid
            GridSize = gridSize;

            ClearGrid();


            // set size of the puzzle piece
            RectTransform rectTransform = GetComponent<RectTransform>();
            float areaLength = rectTransform.rect.width;
            _puzzlePieceSize = areaLength / gridSize;

            // request puzzle piece from pool
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    PuzzlePiece piece = _puzzlePieceProvider.Get();
                    _puzzlePieces.Add(piece);
                    piece.transform.SetParent(transform);
                    piece.gameObject.SetActive(true);
                }
            }

            int x = 0, y = 0;
            foreach (Transform child in transform)
            {
                // initialize piece object
                PuzzlePiece piece = child.GetComponent<PuzzlePiece>();
                piece.Initialize(controller, new Vector2Int(x, y), CalcIdxFromCoord(x, y) + 1);

                // initialize piece rect transform
                RectTransform rect = piece.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(_puzzlePieceSize, _puzzlePieceSize);

                // set piece position
                SetPieceAt(x, y, piece);

                child.gameObject.SetActive(true);
                x++;
                if (x >= GridSize)
                {
                    x = 0;
                    y++;
                }
                if (y >= GridSize) break;
            }
        }

        public void ClearGrid()
        {
            while (_puzzlePieces.Count > 0)
            {
                IPuzzlePiece iPuzzlePiece = _puzzlePieces[_puzzlePieces.Count - 1];
                _puzzlePieces.RemoveAt(_puzzlePieces.Count - 1);
                if (iPuzzlePiece is PuzzlePiece puzzlePiece)
                {
                    _puzzlePieceProvider.Return(puzzlePiece);
                }
            }

            _puzzlePieces.Clear();
        }

        public IPuzzlePiece GetPieceAt(int x, int y)
        {
            return _puzzlePieces[CalcIdxFromCoord(x, y)];
        }

        public void SetPieceAt(int x, int y, IPuzzlePiece _piece)
        {
            if (_piece is PuzzlePiece piece)
            {
                float offset = GridSize % 2 == 0 ? _puzzlePieceSize / 2 : 0;
                float rectX = _puzzlePieceSize * (x - GridSize / 2) + offset;
                float rectY = -(_puzzlePieceSize * (y - GridSize / 2) + offset);
                piece.GetComponent<RectTransform>().localPosition = new(rectX, rectY);

                _puzzlePieces[CalcIdxFromCoord(x, y)] = _piece;
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
        }

        private (int, int) GetCoord(IPuzzlePiece _piece)
        {
            if (_piece is PuzzlePiece piece)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    for (int x = 0; x < GridSize; x++)
                    {
                        if (_puzzlePieces[CalcIdxFromCoord(x, y)].Equals(piece)) return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
    }
}