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

        private int CalcIdxFromCoord(Vector2Int pos)
        {
            return CalcIdxFromCoord(pos.x, pos.y);
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
                Vector2Int pos = new(x, y);

                // initialize piece object
                PuzzlePiece piece = child.GetComponent<PuzzlePiece>();
                piece.Initialize(controller, pos, CalcIdxFromCoord(pos) + 1);

                // initialize piece rect transform
                RectTransform rect = piece.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(_puzzlePieceSize, _puzzlePieceSize);

                // set piece position
                SetPieceAt(pos, piece);

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

        public IPuzzlePiece GetPieceAt(Vector2Int pos)
        {
            return _puzzlePieces[CalcIdxFromCoord(pos)];
        }

        public void SetPieceAt(Vector2Int pos, IPuzzlePiece piece)
        {
            // set pos data of the puzzle piece
            piece.SetPos(pos);

            // set pos in the list that this object maintains
            _puzzlePieces[CalcIdxFromCoord(pos)] = piece;

            // set piece's Canvas position
            if (piece is PuzzlePiece pieceObj)
            {
                float offset = GridSize % 2 == 0 ? _puzzlePieceSize / 2 : 0;
                float rectX = _puzzlePieceSize * (pos.x - GridSize / 2) + offset;
                float rectY = -(_puzzlePieceSize * (pos.y - GridSize / 2) + offset);
                pieceObj.GetComponent<RectTransform>().localPosition = new(rectX, rectY);
            }
        }

        public void SetPieceAt(int x, int y, IPuzzlePiece piece)
        {
            SetPieceAt(new Vector2Int(x, y), piece);
        }

        public void Swap(IPuzzlePiece piece1, IPuzzlePiece piece2)
        {
            Vector2Int piece1Pos = piece1.Pos;
            Vector2Int piece2Pos = piece2.Pos;
            SetPieceAt(piece1Pos, piece2);
            SetPieceAt(piece2Pos, piece1);
        }
    }
}