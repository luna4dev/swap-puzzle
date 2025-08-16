using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class EntryPointController : MonoBehaviour, ISceneController
    {
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
    }
}