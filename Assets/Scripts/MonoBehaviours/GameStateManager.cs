using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class GameStateManager : MonoBehaviour, IGameStateManager
    {
        public EGameState CurrentState { get; }

        public void ChangeState(EGameState newState)
        {

        }

        public void SaveGameState()
        {
            
        }

        public void LoadGameState()
        {

        }
    }
}