using UnityEngine;
using System;
using SwapPuzzle.Interfaces;
using UnityEngine.SceneManagement;

namespace SwapPuzzle.MonoBehaviours
{
    /// <summary>
    /// Handles Unity scene loading, unloading, and scene controller management.
    /// Mid-level manager that responds to higher-level game state commands.
    /// 
    /// Responsibilities:
    /// - Load/unload Unity scenes using SceneManager API
    /// - Find and manage scene controllers (ISceneController) in loaded scenes
    /// - Handle scene transitions with optional transition effects
    /// - Fire scene change events for other systems to respond
    /// - Validate scene configuration (SceneController tags and components)
    /// 
    /// Authority Hierarchy:
    /// - Reports to: GameStateManager (decides WHEN to change scenes based on game logic)
    /// - Commands: Scene controllers within individual scenes
    /// - Coordinates with: ProgressManager (for level-based scene changes)
    /// 
    /// Design Notes:
    /// - This manager focuses on the "HOW" of scene management (Unity technical aspects)
    /// - StateManager handles the "WHY" (game logic, state transitions)
    /// - Scene changes are typically initiated by GameStateManager, not directly by UI
    /// - Each scene must have a GameObject tagged "SceneController" with ISceneController component
    /// - EntryPoint scene controller is assigned in inspector as the default/fallback scene
    /// </summary>
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