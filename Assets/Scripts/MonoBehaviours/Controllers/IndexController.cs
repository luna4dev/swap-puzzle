using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class IndexController : MonoBehaviour, IIndexController, IInputContext
    {
        public ESceneType Type => ESceneType.Index;
        public string ContextName => "Index";
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

        public void DisplayGallery()
        {

        }

        public void OnIllustrationSelected(int illustrationId)
        {

        }

        public void ReturnToMainMenu()
        {
            SceneManager.Instance.LoadScene(ESceneType.MainMenu, ETransitionType.Fade);
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