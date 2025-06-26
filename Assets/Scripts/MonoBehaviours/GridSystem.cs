using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Services;

namespace SwapPuzzle.MonoBehaviours
{
    public class GridSystem : MonoBehaviour, IGridSystem
    {
        public delegate void GridSizeChanged(float size);
        public event GridSizeChanged OnGridSizeChanged;

        private float _gridSize;
        public float GridSize
        {
            get { return _gridSize; }
            private set
            {
                _gridSize = value;
                OnGridSizeChanged?.Invoke(_gridSize);
            }
        }

        private int _pieceCountPerRow;

        public void InitializeGrid(int pieceCountPerRow)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Only in play mode");
                return;
            }
            SetPieceCountPerRow(pieceCountPerRow);
            PrepareGridItems(pieceCountPerRow);
        }

        /// <summary>
        /// Prepare grid items to match the piece count per row
        /// </summary>
        /// <param name="pieceCountPerRow"></param>
        private void PrepareGridItems(int pieceCountPerRow)
        {
            int newGridItemCount = pieceCountPerRow * pieceCountPerRow;
            int existingGridItemCount = 0;

            // check child and find all GridItem including inactive ones
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out GridItem gridItem))
                {
                    existingGridItemCount++;
                }
            }

            int diff = newGridItemCount - existingGridItemCount;
            if (diff > 0)
            {
                GameObject puzzlePiecePrefab = AssetService.Instance.GetPrefab("PuzzlePiece");
                puzzlePiecePrefab.SetActive(false); // set inactive to avoid instantiation error

                for (int i = 0; i < diff; i++)
                {
                    Instantiate(puzzlePiecePrefab, transform);
                }
            }

            // first turn off all child
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            // set grid item position and size
            for (int i = 0; i < pieceCountPerRow; i++)
            {
                for (int j = 0; j < pieceCountPerRow; j++)
                {
                    GameObject child = transform.GetChild(i * pieceCountPerRow + j).gameObject;

                    RectTransform rectTransform = child.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(GridSize, GridSize);

                    GridItem gridItem = child.GetComponent<GridItem>();
                    gridItem.Initialize(this, GetGridNumberFromLogicalCell(i, j), i, j);
                    gridItem.SetPosition(i, j);
                    gridItem.SetDebug(true, GetGridNumberFromLogicalCell(i, j));
                    gridItem.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Set piece count per row and calculate grid size
        /// </summary>
        /// <param name="pieceCountPerRow"></param>
        public void SetPieceCountPerRow(int pieceCountPerRow)
        {
            _pieceCountPerRow = pieceCountPerRow;
            RectTransform rectTransform = GetComponent<RectTransform>();
            float areaLength = rectTransform.sizeDelta.x;
            GridSize = areaLength / pieceCountPerRow;
        }

        /// <summary>
        /// Get grid number by logical cell position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetGridNumberFromLogicalCell(int x, int y)
        {
            return x + y * _pieceCountPerRow + 1;
        }


        /// <summary>
        /// Check if the logical cell position is inside the grid bound
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInsideBound(int x, int y)
        {
            if (0 > x || x >= _pieceCountPerRow) return false;
            if (0 > y || y >= _pieceCountPerRow) return false;
            return true;
        }

        /// <summary>
        /// Get the rect transform position from top left anchor of the grid item by logical cell position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 GetTopLeftAnchorPositionFromLogicalCell(int x, int y)
        {
            return new(
                GridSize * (x + 0.5f),
                -GridSize * (y + 0.5f)
            );
        }

    }
}