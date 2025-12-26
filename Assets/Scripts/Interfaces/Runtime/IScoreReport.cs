using System;
using System.Collections.Generic;

namespace SwapPuzzle.Interfaces
{
    public interface IScoreReport
    {
        /// <summary>
        /// Final total score (DisplayedScore from last snapshot)
        /// </summary>
        int TotalScore { get; }

        /// <summary>
        /// Base score accumulated (BaseScore from last snapshot)
        /// </summary>
        int BaseScore { get; }

        /// <summary>
        /// Score earned through combo bonuses (TotalScore - BaseScore)
        /// </summary>
        int ComboScore { get; }

        /// <summary>
        /// Maximum combo streak achieved during gameplay
        /// </summary>
        int MaxCombo { get; }

        /// <summary>
        /// Total number of moves made
        /// </summary>
        int TotalMoves { get; }

        /// <summary>
        /// Number of times both puzzles were solved
        /// </summary>
        int SolveBothCount { get; }

        /// <summary>
        /// Number of times one puzzle was solved
        /// </summary>
        int SolveOneCount { get; }

        /// <summary>
        /// Number of failed moves
        /// </summary>
        int FailCount { get; }

        /// <summary>
        /// Success rate (successful solves / total moves)
        /// </summary>
        float SuccessRate { get; }
    }
}