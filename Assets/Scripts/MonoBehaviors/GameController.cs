using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviors
{
    public class GameController : MonoBehaviour, IGameController
    {
        public void InitializeScene()
        {
            throw new System.NotImplementedException();
        }

        public void OnSceneEnter(ISceneTransitionData data)
        {
            throw new System.NotImplementedException();
        }

        public void OnSceneExit()
        {
            throw new System.NotImplementedException();
        }

        public void CleanupScene()
        {
            throw new System.NotImplementedException();
        }

        public void StartLevel(int levelId)
        {
            throw new System.NotImplementedException();
        }

        public void PauseGame()
        {
            throw new System.NotImplementedException();
        }

        public void ResumeGame()
        {
            throw new System.NotImplementedException();
        }

        public void RestartLevel()
        {
            throw new System.NotImplementedException();
        }

        public void CompleteLevel()
        {
            throw new System.NotImplementedException();
        }

        public void ReturnToMainMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}