using SwapPuzzle.Interfaces;
using TMPro;
using UnityEngine;


namespace SwapPuzzle.MonoBehaviours
{
    public class ScoreSummary : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _body;

        public const string TotalScoreExpression = "점수 - {0}";
        public const string ComboStreakExpression = "연속 - {0}콤보";
        public const string MoveCountExpression = "움직인 횟수 - {0}회";
        public const string SolveTwoExpression = "한번에 두개 - {0}회";

        public void Initialize(ILevelData levelData, IScoreReport scoreReport)
        {
            // Set title to level name
            _title.text = levelData.Name;

            // Build body with 4 expressions joined by <br>
            string totalScore = string.Format(TotalScoreExpression, scoreReport.TotalScore);
            string comboStreak = string.Format(ComboStreakExpression, scoreReport.MaxCombo);
            string comboScore = string.Format(MoveCountExpression, scoreReport.TotalMoves);
            string solveTwo = string.Format(SolveTwoExpression, scoreReport.SolveBothCount);

            _body.text = $"{totalScore}<br>{comboStreak}<br>{comboScore}<br>{solveTwo}";
        }
    }
}
