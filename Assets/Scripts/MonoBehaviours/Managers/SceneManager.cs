using UnityEngine;
using System;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class SceneManager : MonoBehaviour, ISceneManager
    {
        public static SceneManager Instance { get; private set; }

        [SerializeField] private ESceneType currentScene;

        public ESceneType CurrentScene => currentScene;

        public Action<ESceneType> OnSceneChanged { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(ESceneType targetScene, ETransitionType transitionType = ETransitionType.Fade, ISceneTransitionData data = null)
        {
            if (currentScene == targetScene) return;

            var previousScene = currentScene;
            currentScene = targetScene;

            OnSceneChanged?.Invoke(targetScene);

            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene.ToString());
        }
    }
}