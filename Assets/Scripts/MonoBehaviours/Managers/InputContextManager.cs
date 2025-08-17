using UnityEngine;
using System;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    /// <summary>
    /// Note that this manager should have less priority than SceneManager
    /// </summary>
    public class InputContextManager : MonoBehaviour, IInputContextManager
    {
        public event Action OnContextChanged;
        public IInputContext CurrentContext { get; private set; }
        public bool IsEnabled { get; private set; }
        private List<IInputContext> _contextStack = new();

        /// <summary>
        /// A singleton object
        /// </summary>
        public static InputContextManager Instance { get; private set; }

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

        private void OnEnable()
        {
            SceneManager.Instance.OnSceneChangeStarted += HandleSceneChangeStarted;
            SceneManager.Instance.OnSceneChanged += HandleSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.Instance.OnSceneChangeStarted -= HandleSceneChangeStarted;
            SceneManager.Instance.OnSceneChanged -= HandleSceneChanged;
        }

        public void PushContext(IInputContext context)
        {
            _contextStack.Add(context);
            CurrentContext = context;
            OnContextChanged?.Invoke();
        }

        public bool PopContext()
        {
            if (_contextStack.Count > 0)
            {
                _contextStack.RemoveAt(_contextStack.Count - 1);
                CurrentContext = _contextStack.Count > 0 ? _contextStack[_contextStack.Count - 1] : null;
                OnContextChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void ClearAllContexts()
        {
            _contextStack.Clear();
            CurrentContext = null;
            OnContextChanged?.Invoke();
        }

        public bool ContainsContext(IInputContext context)
        {
            foreach (IInputContext element in _contextStack)
            {
                if (ReferenceEquals(element, context)) return true;
            }
            return false;
        }

        public bool ContainsContext<T>() where T : class, IInputContext
        {
            foreach (IInputContext element in _contextStack)
            {
                if (element is T) return true;
            }
            return false;
        }

        public T GetContext<T>() where T : class, IInputContext
        {
            foreach (IInputContext element in _contextStack)
            {
                if (element is T typeAssumedContext) return typeAssumedContext;
            }
            return default;
        }

        public IEnumerable<IInputContext> GetAllContexts()
        {
            return _contextStack;
        }

        public bool ProcessInput(InputType inputType, InputData inputData)
        {
            if (IsEnabled != true) return false;
            if (CurrentContext == null) return false;

            CurrentContext.HandleInput(inputType, inputData);
            return true;
        }

        private void HandleSceneChangeStarted(ESceneType prevScene, ESceneType nextScene) {
            // block input 
            IsEnabled = false;
        }

        private void HandleSceneChanged(ISceneController sceneController)
        {
            // re activate input 
            IsEnabled = true;

            // change context state
            if (sceneController is IInputContext inputContext)
            {
                CurrentContext = inputContext;
                OnContextChanged?.Invoke();
            }
        }
    }
}