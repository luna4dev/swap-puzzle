using System;
using SwapPuzzle.Interfaces;
using UnityEngine;
using SwapPuzzle.Classes;

namespace SwapPuzzle.MonoBehaviours
{
    /// <summary>
    /// Manages player progression data, level tracking, and save/load operations.
    /// 
    /// Responsibilities:
    /// - Load/save progress data using SaveLoadService
    /// - Track current level progression (Easy/Medium/Hard) and specific level
    /// - Determine current level from progress logs (for saved games)
    /// - Handle difficulty selection for new games (StartEasy/Medium/Hard)
    /// - Log level start/completion events to progress runtime
    /// - Fire OnProgressChange events when progression state changes
    /// - Provide level navigation (next/previous level determination)
    /// 
    /// Does NOT handle:
    /// - Game play state (playing/paused/menu) - use GameStateManager
    /// - UI state management - use appropriate UI controllers  
    /// - Actual gameplay mechanics - use game-specific managers
    /// 
    /// Design Notes:
    /// - _currentLevelProgression and _currentLevel act as a "cursor" for current position
    /// - _runtime contains historical progress logs for persistence and analytics
    /// - For saved games: parse _runtime logs to determine incomplete levels
    /// - For new games: difficulty selection sets initial progression state
    /// </summary>
    public class ProgressManager : MonoBehaviour, IProgressManager
    {
        public static ProgressManager Instance { get; private set; }

        /// <summary>
        /// runtime is initialized in entry point
        /// </summary>
        private ProgressRuntime _runtime;
        private ILevelProgressionData _levelProgressionData;

        public event Action OnProgressChange;

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

        public void InitializeProgress(ILevelProgressionData levelProgression)
        {
            // override existing runtime;
            _runtime = new ProgressRuntime();

            // load progression data
            _levelProgressionData = levelProgression;

            // check validity of asset
            if (_levelProgressionData.Levels == null || _levelProgressionData.Levels.Count == 0)
            {
                throw new Exception("Invalid: Malformed level progression data");
            }

            // load first level
            ILevelData firstLevel = _levelProgressionData.Levels[0];
            _runtime.LogProgressStart(_levelProgressionData.Name, firstLevel.Name);

            // emit progress change
            OnProgressChange?.Invoke();
        }

        public ILevelData GetCurrentLevel()
        {
            if (_runtime == null)
            {
                return null;
            }

            if (_levelProgressionData == null || _levelProgressionData.Levels == null) return null;

            string currentLevelName = _runtime.CurrentLevel();
            for (int i = 0; i < _levelProgressionData.Levels.Count; i++)
            {
                if (_levelProgressionData.Levels[i].Name == currentLevelName) return _levelProgressionData.Levels[i];
            }

            throw new Exception("Invalid: level name doesn't exists");
        }

        public void CompleteCurrentLevel()
        {
            if (_runtime == null)
            {
                throw new Exception("Invalid: runtime not exists");
            }
            _runtime.LogProgressComplete(_levelProgressionData.Name, _runtime.CurrentLevel());
        }

        private ILevelData FindNextLevel(string currentLevelName)
        {
            if (_levelProgressionData == null || _levelProgressionData.Levels == null) return null;

            int i = 0;
            for (i = 0; i < _levelProgressionData.Levels.Count; i++)
            {
                if (_levelProgressionData.Levels[i].Name == currentLevelName) break;
            }

            if (i + 1 < _levelProgressionData.Levels.Count) return _levelProgressionData.Levels[i + 1];
            return null;
        }

        public bool HasNextLevel()
        {
            if (_runtime == null)
            {
                throw new Exception("Invalid: runtime not exists");
            }
            string levelName = _runtime.CurrentLevel();

            if (levelName == null) return false;
            if (FindNextLevel(levelName) == null) return false;
            return true;
        }

        public void GoToNextLevel()
        {
            if (_runtime == null)
            {
                throw new Exception("Invalid: runtime not exists");
            }
            string currentLevelName = _runtime.CurrentLevel();

            if (currentLevelName == null)
            {
                throw new Exception("Invalid: current level should exists");
            }

            ILevelData nextLevel = FindNextLevel(currentLevelName);
            _runtime.LogProgressStart(_levelProgressionData.Name, nextLevel.Name);
            OnProgressChange?.Invoke();
        }
    }
}