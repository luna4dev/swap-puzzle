using System;
using SwapPuzzle.Interfaces;
using UnityEngine;
using SwapPuzzle.AssetDefinitions;
using SwapPuzzle.Classes;
using SwapPuzzle.Services;

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

        [SerializeField] private string _easyLevelData;
        [SerializeField] private string _mediumLevelData;
        [SerializeField] private string _hardLevelData;

        /// <summary>
        /// runtime is initialized in entry point
        /// </summary>
        private ProgressRuntime _runtime;

        /// <summary>
        /// Level progression and level will be initialized from main menu right before transitioning to game scene
        /// </summary>
        private LevelProgressionData _currentLevelProgression;
        private LevelData _currentLevel;

        public event Action OnProgressChange;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            _runtime = SaveLoadService.LoadProgress();
        }

        public void StartEasy()
        {

        }

        public void StartMedium()
        {

        }

        public void StartHard()
        {

        }

        public void ContinueSaved()
        {
            
        }

        public ILevelData GetCurrentLevel()
        {
            if (_runtime == null)
            {
                return null;
            }



            return default;
        }

        public void CompleteCurrentLevel()
        {

        }

        public void GoToNextLevel()
        {

        }

        public void ResetProgress()
        {

        }
    }
}