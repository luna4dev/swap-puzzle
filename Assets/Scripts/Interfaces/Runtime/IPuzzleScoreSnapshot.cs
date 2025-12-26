namespace SwapPuzzle.Interfaces
{
    public enum EPuzzleSolveType
    {
        SolveBoth,
        SolveOne,
        Fail,
        PuzzleWin,
    }
    public interface IPuzzleScoreSnapshot
    {
        /// <summary>
        /// Current solve type
        /// </summary>
        EPuzzleSolveType SolveType { get; }

        /// <summary>
        /// Current combo count
        /// ex) if fail -> set to 0, if success prev combo was 2 then set to 3
        /// </summary>
        int Combo { get; }

        /// <summary>
        /// Score stack starts from last fail movement. if prev was 4 and i solve both this time, it is set to 8(4 + 4)
        /// Later, it is multiplied with combo and added up to base score
        /// </summary>
        int ScoreStack { get; }

        /// <summary>
        /// Base score is the score of last fail movement. if gamer in combo, the base score stays the same
        /// when gamer fails to keep the combo, the score stack * combo will be added to basescore
        /// </summary>
        int BaseScore { get; }

        /// <summary>
        /// Displayed score is basically baseScore + scoreStack * combo at this snapshot.
        /// It is not confirmed yet but if next movement is failure or game ends it becomes the real one
        /// </summary>
        int DisplayedScore { get; }
    }
}
