using UnityEngine;
using TMPro;

namespace SwapPuzzle.MonoBehaviours
{
    public class ScoreIndicator : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text comboText;
        [SerializeField] PuzzleScoreSystem puzzleScoreSystem;

        public void Awake()
        {
            scoreText.text = "";
            comboText.text = "";
        }

        private void OnEnable()
        {
            if (puzzleScoreSystem != null)
            {
                puzzleScoreSystem.OnScoreChange += HandleScoreChange;
            }
        }

        private void OnDisable()
        {
            if (puzzleScoreSystem != null)
            {
                puzzleScoreSystem.OnScoreChange -= HandleScoreChange;
            }
        }

        private void HandleScoreChange(PuzzleScoreSnapshot snapshot)
        {
            UpdateComboText(snapshot);
            UpdateScoreText(snapshot);
        }

        private void UpdateComboText(PuzzleScoreSnapshot snapshot)
        {
            if (snapshot.Combo > 0)
            {
                comboText.text = $"Combo X{snapshot.Combo}";
            }
            else
            {
                comboText.text = "";
            }
        }

        private void UpdateScoreText(PuzzleScoreSnapshot snapshot)
        {
            // display score without combo multiplier
            if (snapshot.Combo > 0)
            {
                scoreText.text = $"(+{snapshot.ScoreStack}) {snapshot.BaseScore}";
            }
            else
            {
                scoreText.text = snapshot.DisplayedScore.ToString();
            }
        }
    }
}