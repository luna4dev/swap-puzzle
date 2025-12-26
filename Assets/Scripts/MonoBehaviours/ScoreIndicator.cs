using UnityEngine;
using TMPro;
using SwapPuzzle.Interfaces;

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

        private void HandleScoreChange(IPuzzleScoreSnapshot snapshot)
        {
            UpdateComboText(snapshot);
            UpdateScoreText(snapshot);
        }

        private void UpdateComboText(IPuzzleScoreSnapshot snapshot)
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

        private void UpdateScoreText(IPuzzleScoreSnapshot snapshot)
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