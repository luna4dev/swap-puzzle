
using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    public enum EPuzzleSolveType
    {
        SolveBoth,
        SolveOne,
        Fail,
        PuzzleWin,
    }

    public class PuzzleScoreSnapshot
    {
        public const int SOLVE_BOTH_SCORE = 4;
        public const int SOLVE_ONE_SCORE = 1;
        public const int FAIL_SCORE = 0;

        /// <summary>
        /// Save Current solve type
        /// </summary>
        public EPuzzleSolveType SolveType;
        /// <summary>
        /// save current combo count
        /// ex) if fail -> set to 0, if success prev combo was 2 then set to 3
        /// </summary>
        public int Combo;
        /// <summary>
        /// Score stack starts from last fail movement. if prev was 4 and i solve both this time, it is set to 8(4 + 4)
        /// Later, it is multiplied with combo and added up to base score
        /// </summary>
        public int ScoreStack;
        /// <summary>
        /// base score is the score of last fail movment. if gamer in combo, the base score stays the same
        /// when gamer fails to keep the combo, the score stack * combo will be added to basescore
        /// </summary>
        public int BaseScore;
        /// <summary>
        /// displayed score is basically baseScore + scoreStack * combo at this snapshot.
        /// it is not confirmed yet but if next movement is failure or game ends it becomes the real one 
        /// </summary>
        public int DisplayedScore;

        public PuzzleScoreSnapshot(PuzzleScoreSnapshot prev, EPuzzleSolveType solveType)
        {
            SolveType = solveType;

            int prevBaseScore = 0;
            int prevCombo = 0;
            int prevScoreStack = 0;

            if (prev != null)
            {
                prevBaseScore = prev.BaseScore;
                prevCombo = prev.Combo;
                prevScoreStack = prev.ScoreStack;
            }

            int scoreAddition = solveType switch
            {
                EPuzzleSolveType.Fail => FAIL_SCORE,
                EPuzzleSolveType.SolveOne => SOLVE_ONE_SCORE,
                EPuzzleSolveType.SolveBoth => SOLVE_BOTH_SCORE,
                _ => 0,
            };

            // calculate fail or game end
            if (solveType == EPuzzleSolveType.PuzzleWin || solveType == EPuzzleSolveType.Fail)
            {
                Combo = 0;
                ScoreStack = scoreAddition;
                BaseScore = prevBaseScore + prevCombo * prevScoreStack;
                DisplayedScore = BaseScore;
                return;
            }

            // calculate solve
            Combo = prevCombo + 1;
            ScoreStack = prevScoreStack + scoreAddition;
            BaseScore = prevBaseScore;
            DisplayedScore = BaseScore + Combo * ScoreStack;
        }

        public override string ToString()
        {
            return $"PuzzleScoreSnapshot(SolveType: {SolveType}, Combo: {Combo}, ScoreStack: {ScoreStack}, BaseScore: {BaseScore}, DisplayedScore: {DisplayedScore})";
        }
    }

    public class PuzzleScoreSystem : MonoBehaviour
    {
        private List<PuzzleScoreSnapshot> _log = new();
        private bool _dirty = false;
        public bool Dirty { get { return _dirty; } }

        private PuzzleScoreSnapshot LastSnapshot()
        {
            return _log.Count > 0 ? _log[_log.Count - 1] : null;
        }

        private PuzzleScoreSnapshot SecondLastSnapshot() {
            if (_log.Count < 2) return null;
            return _log[_log.Count - 2];
        }

        public int GetDisplayedScore()
        {
            PuzzleScoreSnapshot lastSnapshot = LastSnapshot();
            if (lastSnapshot == null) return 0;
            return lastSnapshot.DisplayedScore;
        }

        public int GetCombo()
        {
            PuzzleScoreSnapshot lastSnapshot = LastSnapshot();
            if (lastSnapshot == null) return 0;
            return lastSnapshot.Combo;
        }

        public int GetScoreAddition()
        {
            PuzzleScoreSnapshot lastSnapshot = LastSnapshot();
            PuzzleScoreSnapshot secondLastSnapshot = SecondLastSnapshot();

            if (lastSnapshot == null) return 0;
            if (secondLastSnapshot == null) return LastSnapshot().DisplayedScore;

            return lastSnapshot.DisplayedScore - secondLastSnapshot.DisplayedScore;
        }

        public int GetCumulativeScoreAdditionFromLastFail()
        {
            PuzzleScoreSnapshot lastSnapshot = LastSnapshot();
            if (lastSnapshot == null) return 0;
            if (lastSnapshot.SolveType != EPuzzleSolveType.Fail && lastSnapshot.SolveType != EPuzzleSolveType.PuzzleWin)
            {
                return lastSnapshot.DisplayedScore - lastSnapshot.BaseScore;
            }

            PuzzleScoreSnapshot secondLastSnapshot = SecondLastSnapshot();
            if (secondLastSnapshot.SolveType == EPuzzleSolveType.Fail) return 0;
            return secondLastSnapshot.DisplayedScore - secondLastSnapshot.BaseScore;
        }


        public void Notify(EPuzzleSolveType solveType)
        {
            _dirty = true;
            PuzzleScoreSnapshot curSnapshot = new PuzzleScoreSnapshot(LastSnapshot(), solveType);
            _log.Add(curSnapshot);
            Debug.Log(curSnapshot);
        }

        public void Clear()
        {
            _dirty = false;
            _log.Clear();
        }
    }
}