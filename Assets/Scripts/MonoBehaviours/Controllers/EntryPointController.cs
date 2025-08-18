using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class EntryPointController : MonoBehaviour, ISceneController
    {
        public ESceneType Type => ESceneType.EntryPoint;
        public string ContextName => "EntryPoint";
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

        public void TransitionToMainMenu()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.LoadScene(ESceneType.MainMenu, ETransitionType.Fade);
            }
        }


        private bool CanHandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Confirm:
                    return true;
                default:
                    return false;
            }
        }
        public bool HandleInput(InputType inputType, InputData inputData)
        {
            if (CanHandleInput(inputType))
            {
                TransitionToMainMenu();
                return true;
            }
            return false;
        }

        public void HandleContextChange()
        {

        }
    }
}