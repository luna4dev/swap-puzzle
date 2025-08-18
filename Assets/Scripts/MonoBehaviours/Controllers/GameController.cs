using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class GameController : MonoBehaviour, IGameController
    {
        public ESceneType Type => ESceneType.Game;
        public string ContextName => "Game";
        public int Priority => 1;

        private void OnEnable()
        {
            InputContextManager.Instance.OnContextChanged += HandleContextChange;
        }

        private void OnDisable()
        {
            InputContextManager.Instance.OnContextChanged -= HandleContextChange;
        }

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
            ConfirmPopup.OpenPopup("Return to Main Menu", "Return to Main Menu", "Yes", "No", true, () =>
            {
                SceneManager.Instance.LoadScene(ESceneType.MainMenu, ETransitionType.Fade);
            });
        }

        private bool CanHandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Back:
                case InputType.Cancel:
                    return true;
                default:
                    return false;
            }
        }

        public bool HandleInput(InputType inputType, InputData inputData)
        {
            if (CanHandleInput(inputType) == false) return false;

            if (inputType == InputType.Back || inputType == InputType.Cancel)
            {
                ReturnToMainMenu();
            }

            return true;
        }

        public void HandleContextChange()
        {

        }
    }
}