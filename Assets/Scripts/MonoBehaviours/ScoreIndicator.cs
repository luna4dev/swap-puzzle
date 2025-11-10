using TMPro;
using UnityEngine;


namespace SwapPuzzle.MonoBehaviours
{
    public class ScoreIndicator : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] PuzzleScoreSystem puzzleScoreSystem;

        public void Awake()
        {
            text.text = "";
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
            text.text = snapshot.DisplayedScore.ToString();
        }
    }
}