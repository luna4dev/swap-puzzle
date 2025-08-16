using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class MainMenuController : MonoBehaviour, IMainMenuController
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
    }
}