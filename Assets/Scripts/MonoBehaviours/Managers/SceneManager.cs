using UnityEngine;
using System;
using SwapPuzzle.Interfaces;
using UnityEngine.SceneManagement;

namespace SwapPuzzle.MonoBehaviours
{
    public class SceneManager : MonoBehaviour, ISceneManager
    {
        public event Action<ESceneType, ESceneType> OnSceneChangeStarted;
        public event Action<ISceneController> OnSceneChanged;

        [SerializeField] private EntryPointController _entryPoint;
        public ESceneType CurrentScene
        {
            get
            {
                if (CurrentSceneController == null) return ESceneType.EntryPoint;
                return CurrentSceneController.Type;
            }
        }
        public ISceneController CurrentSceneController { get; private set; }


        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static SceneManager Instance { get; private set; }

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

            if (_entryPoint == null)
            {
                throw new Exception("Must set entry point scene controller");
            }
            CurrentSceneController = _entryPoint;
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        public void LoadScene(ESceneType targetScene, ETransitionType transitionType = ETransitionType.Fade, ISceneTransitionData data = null)
        {
            if (CurrentScene == targetScene) return;

            var previousScene = CurrentScene;
            OnSceneChangeStarted?.Invoke(previousScene, targetScene);
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene.ToString());
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // find scene controller component
            GameObject[] controllers = GameObject.FindGameObjectsWithTag("SceneController");
            if (controllers.Length == 0)
            {
                throw new Exception("Scene " + scene.name + " is not configured correctly: there should be at least one SceneController tagged object");
            }
            ISceneController sceneController = null;
            for (int i = 0; i < controllers.Length; i++)
            {
                Component[] components = controllers[i].GetComponents(typeof(ISceneController));
                for (int j = 0; j < components.Length; j++)
                {
                    if (components[j] is ISceneController sceneControllerComponent)
                    {
                        sceneController = sceneControllerComponent;
                        break;
                    }
                }
                if (sceneController != null) break;
            }
            if (sceneController == null)
            {
                throw new Exception("Scene " + scene.name + " is not configured correctly: there should be at least one ISceneController component");
            }

            // invoke event with found scene controller;
            OnSceneChanged?.Invoke(sceneController);
        }
    }
}