using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Unified interface for puzzle grid management and piece swapping mechanics
    /// </summary>
    public interface IPuzzleGrid
    {
        int GridSize { get; }
        void InitializeGrid(IPuzzleController controller, int gridSize);
        void ClearGrid();
        IPuzzlePiece GetPieceAt(Vector2Int pos);
        IPuzzlePiece GetPieceAt(int x, int y);
        void SetPieceAt(Vector2Int pos, IPuzzlePiece piece);
        void SetPieceAt(int x, int y, IPuzzlePiece piece);
        void Swap(IPuzzlePiece piece1, IPuzzlePiece piece2);
    }
}