using System.Collections.Generic;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.Classes
{
    public class ScoreReport : IScoreReport
    {
        public int TotalScore { get; private set; }
        public int BaseScore { get; private set; }
        public int ComboScore { get; private set; }
        public int MaxCombo { get; private set; }
        public int TotalMoves { get; private set; }
        public int SolveBothCount { get; private set; }
        public int SolveOneCount { get; private set; }
        public int FailCount { get; private set; }
        public float SuccessRate { get; private set; }

        public ScoreReport(List<IPuzzleScoreSnapshot> log)
        {
            if (log == null || log.Count == 0)
            {
                // Initialize with zeros
                TotalScore = 0;
                BaseScore = 0;
                ComboScore = 0;
                MaxCombo = 0;
                TotalMoves = 0;
                SolveBothCount = 0;
                SolveOneCount = 0;
                FailCount = 0;
                SuccessRate = 0f;
                return;
            }

            // Calculate total moves
            TotalMoves = log.Count;

            // Get final scores from last snapshot
            IPuzzleScoreSnapshot lastSnapshot = log[log.Count - 1];
            TotalScore = lastSnapshot.DisplayedScore;
            BaseScore = lastSnapshot.BaseScore;
            ComboScore = TotalScore - BaseScore;

            // Iterate through log to calculate statistics
            int maxCombo = 0;
            int solveBothCount = 0;
            int solveOneCount = 0;
            int failCount = 0;

            foreach (var snapshot in log)
            {
                // Track max combo
                if (snapshot.Combo > maxCombo)
                {
                    maxCombo = snapshot.Combo;
                }

                // Count solve types
                switch (snapshot.SolveType)
                {
                    case EPuzzleSolveType.SolveBoth:
                        solveBothCount++;
                        break;
                    case EPuzzleSolveType.SolveOne:
                        solveOneCount++;
                        break;
                    case EPuzzleSolveType.Fail:
                        failCount++;
                        break;
                    case EPuzzleSolveType.PuzzleWin:
                        // PuzzleWin doesn't count as a move type for statistics
                        break;
                }
            }

            MaxCombo = maxCombo;
            SolveBothCount = solveBothCount;
            SolveOneCount = solveOneCount;
            FailCount = failCount;

            // Calculate success rate
            int successfulMoves = solveBothCount + solveOneCount;
            SuccessRate = TotalMoves > 0 ? (float)successfulMoves / TotalMoves : 0f;
        }

        public override string ToString()
        {
            return $"ScoreReport(Total: {TotalScore}, Base: {BaseScore}, Combo: {ComboScore}, " +
                   $"MaxCombo: {MaxCombo}, Moves: {TotalMoves}, " +
                   $"SolveBoth: {SolveBothCount}, SolveOne: {SolveOneCount}, Fail: {FailCount}, " +
                   $"SuccessRate: {SuccessRate:P1})";
        }
    }
}
