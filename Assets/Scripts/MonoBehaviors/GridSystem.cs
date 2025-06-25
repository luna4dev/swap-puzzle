using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviors
{
    [RequireComponent(typeof(Grid))]
    public class GridSystem : MonoBehaviour, IGridSystem
    {
        public delegate void GridSizeChanged(int size);
        public event GridSizeChanged OnGridSizeChanged;
        private int _gridSize;
        public int GridSize { get { return _gridSize; } private set { 
            _gridSize = value; 
            _grid.cellSize = new Vector3(_gridSize, _gridSize, _gridSize);
            OnGridSizeChanged?.Invoke(_gridSize);
        } }
        [SerializeField] private Grid _grid;

    
        public void InitializeGrid(int size)
        {
            GridSize = size;
        }

        public void SetGridSize(int size) {
            GridSize = size;
        }

        public Vector2 GetGridPosition(int x, int y) {
            Vector2 worldPosition = _grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            return worldPosition;
        }
    }
}