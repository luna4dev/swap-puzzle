using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SwapPuzzle.Utilities
{
    /// <summary>
    /// Implements controlled placement shuffling strategy that guarantees exact number of solved pieces
    /// </summary>
    public class ControlledPlacement : IShuffler
    {
        /// <summary>
        /// Shuffles puzzle pieces with precise control over solved piece count
        /// </summary>
        /// <param name="puzzleGrid">The puzzle grid to shuffle</param>
        /// <param name="targetSolvedCount">Number of pieces that should remain in correct positions after shuffle</param>
        public void Shuffle(IPuzzleGrid puzzleGrid, int targetSolvedCount = 0)
        {
            int gridSize = puzzleGrid.GetGridSize();
            int totalPieces = gridSize * gridSize;
            
            // Validate target count
            if (targetSolvedCount < 0 || targetSolvedCount > totalPieces)
            {
                Debug.LogWarning($"Invalid targetSolvedCount: {targetSolvedCount}. Must be between 0 and {totalPieces}");
                targetSolvedCount = Mathf.Clamp(targetSolvedCount, 0, totalPieces);
            }

            // Step 1: Collect all pieces and their original positions
            List<IPuzzlePiece> allPieces = new List<IPuzzlePiece>();
            List<Vector2Int> allPositions = new List<Vector2Int>();
            
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    allPieces.Add(puzzleGrid.GetPieceAt(x, y));
                    allPositions.Add(new Vector2Int(x, y));
                }
            }

            // Step 2: Randomly select which pieces should be solved
            List<int> solvedIndices = GetRandomIndices(totalPieces, targetSolvedCount);
            
            // Step 3: Create list of pieces that need to be displaced
            List<IPuzzlePiece> piecesToDisplace = new List<IPuzzlePiece>();
            List<Vector2Int> availablePositions = new List<Vector2Int>();
            
            for (int i = 0; i < totalPieces; i++)
            {
                if (!solvedIndices.Contains(i))
                {
                    piecesToDisplace.Add(allPieces[i]);
                    availablePositions.Add(allPositions[i]);
                }
            }

            // Step 4: Place solved pieces in their correct positions
            foreach (int solvedIndex in solvedIndices)
            {
                IPuzzlePiece piece = allPieces[solvedIndex];
                Vector2Int position = allPositions[solvedIndex];
                puzzleGrid.SetPieceAt(position.x, position.y, piece);
            }

            // Step 5: Shuffle displaced pieces ensuring none end up in correct positions
            ShuffleDisplacedPieces(puzzleGrid, piecesToDisplace, availablePositions);
        }

        /// <summary>
        /// Gets random indices for pieces that should remain solved
        /// </summary>
        private List<int> GetRandomIndices(int totalCount, int selectCount)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < totalCount; i++)
            {
                indices.Add(i);
            }

            // Fisher-Yates shuffle and take first selectCount elements
            for (int i = indices.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (indices[i], indices[j]) = (indices[j], indices[i]);
            }

            return indices.Take(selectCount).ToList();
        }

        /// <summary>
        /// Shuffles displaced pieces ensuring none accidentally end up in correct positions
        /// </summary>
        private void ShuffleDisplacedPieces(IPuzzleGrid puzzleGrid, List<IPuzzlePiece> pieces, List<Vector2Int> positions)
        {
            if (pieces.Count == 0) return;

            // Create a derangement (permutation where no element appears in its original position)
            List<int> arrangement = CreateDerangement(pieces.Count);
            
            // Place pieces according to derangement
            for (int i = 0; i < pieces.Count; i++)
            {
                IPuzzlePiece piece = pieces[i];
                Vector2Int targetPosition = positions[arrangement[i]];
                
                // Double-check this piece won't be in its correct position
                if (targetPosition.x == piece.OriginalX && targetPosition.y == piece.OriginalY)
                {
                    // Find a safe position by swapping with next piece
                    int nextIndex = (arrangement[i] + 1) % pieces.Count;
                    (arrangement[i], arrangement[nextIndex]) = (arrangement[nextIndex], arrangement[i]);
                    targetPosition = positions[arrangement[i]];
                }
                
                puzzleGrid.SetPieceAt(targetPosition.x, targetPosition.y, piece);
            }
        }

        /// <summary>
        /// Creates a derangement (permutation where no element is in its original position)
        /// </summary>
        private List<int> CreateDerangement(int count)
        {
            if (count == 0) return new List<int>();
            if (count == 1) return new List<int> { 0 }; // Can't derange single element
            
            List<int> result = new List<int>();
            for (int i = 0; i < count; i++)
            {
                result.Add(i);
            }

            // Generate derangement using rejection sampling with fallback
            int attempts = 0;
            const int maxAttempts = 100;
            
            while (attempts < maxAttempts)
            {
                // Fisher-Yates shuffle
                for (int i = result.Count - 1; i > 0; i--)
                {
                    int j = Random.Range(0, i + 1);
                    (result[i], result[j]) = (result[j], result[i]);
                }

                // Check if it's a valid derangement
                bool isDerangement = true;
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i] == i)
                    {
                        isDerangement = false;
                        break;
                    }
                }

                if (isDerangement)
                {
                    return result;
                }
                
                attempts++;
            }

            // Fallback: force derangement by systematic swapping
            return ForceDerangement(result);
        }

        /// <summary>
        /// Forces a derangement by systematic swapping when random generation fails
        /// </summary>
        private List<int> ForceDerangement(List<int> arrangement)
        {
            for (int i = 0; i < arrangement.Count; i++)
            {
                if (arrangement[i] == i)
                {
                    // Find someone to swap with
                    for (int j = 0; j < arrangement.Count; j++)
                    {
                        if (j != i && arrangement[j] != j)
                        {
                            (arrangement[i], arrangement[j]) = (arrangement[j], arrangement[i]);
                            break;
                        }
                    }
                    
                    // If still can't swap, use circular shift for remaining elements
                    if (arrangement[i] == i && i < arrangement.Count - 1)
                    {
                        int next = (i + 1) % arrangement.Count;
                        (arrangement[i], arrangement[next]) = (arrangement[next], arrangement[i]);
                    }
                }
            }
            
            return arrangement;
        }
    }
}