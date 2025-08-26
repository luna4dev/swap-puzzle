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
            ConfirmPopup.OpenPopup("메인 메뉴로 돌아가기", "풀지 못한 퍼즐은 저장되지 않습니다. 메인 메뉴로 돌아가시겠습니까?", "네", "아니오", true, () =>
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