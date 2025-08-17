using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class MainMenuController : MonoBehaviour, IMainMenuController, IInputContext
    {
        public ESceneType Type => ESceneType.MainMenu;
        public string ContextName => "MainMenu";
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

        }

        public void OnSceneEnter(ISceneTransitionData data)
        {

        }

        public void OnSceneExit()
        {

        }

        public void CleanupScene()
        {

        }

        public void ShowLevelSelection()
        {

        }

        public void ShowSettings()
        {

        }

        public void OnPlayButtonPressed()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.LoadScene(ESceneType.Game, ETransitionType.Fade);
            }
        }

        public void OnIndexButtonPressed()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.LoadScene(ESceneType.Index, ETransitionType.Fade);
            }
        }

        public bool CanHandleInput(InputType inputType)
        {
            return false;
        }

        public bool HandleInput(InputType inputType, InputData inputData) {
            return false;
        }

        public void HandleContextChange() {

        }
    }
}