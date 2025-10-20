using UnityEngine;
using SwapPuzzle.Interfaces;
using SwapPuzzle.Utilities;
using System;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    public class IllustrationViewer : PuzzleController
    {
        public void SolvePieces()
        {
            int gridSize = _currentLevelData.GridSize;
            List<IPuzzlePiece> pieces = new();

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    IPuzzlePiece puzzlePiece = _puzzleGrid.GetPieceAt(x, y);
                    pieces.Add(puzzlePiece);
                }
            }

            foreach (IPuzzlePiece piece in pieces)
            {
                _puzzleGrid.SetPieceAt(piece.OriginalPos, piece);
                piece.SetDisabledState();
            }
        }

        public void ShufflePieces()
        {
            ShufflePieces(_currentLevelData.PresolvedPieces);

            int gridSize = _currentLevelData.GridSize;

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    IPuzzlePiece puzzlePiece = _puzzleGrid.GetPieceAt(x, y);
                    if (puzzlePiece.Pos != puzzlePiece.OriginalPos)
                    {
                        puzzlePiece.SetNormalState();
                    }
                    else
                    {
                        puzzlePiece.SetDisabledState();
                    }
                }
            }
        }
    }
}
