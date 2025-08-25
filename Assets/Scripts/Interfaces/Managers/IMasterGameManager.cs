using System;

namespace SwapPuzzle.Interfaces
{
    public enum EPlayState
    {
        Initialize, // when game is first initializing
        Block,  // when system blocks thing
        Play,  // normal state, gamer can interact with game
        Pause, // gamer triggered something that paused the game(popup, contextmenu)
    }

    public enum EDifficulty
    {
        None = -1,
        Easy,
        Medium,
        Hard,
    }

    public interface IMasterGameManager
    {
        event Action OnStateChange;
        EPlayState PlayState { get; }
        EDifficulty Difficulty { get; }
    }
}