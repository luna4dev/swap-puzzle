using UnityEngine;
using SwapPuzzle.Interfaces;
using System;

namespace SwapPuzzle.MonoBehaviours
{
    public class MasterGameManager : MonoBehaviour, IMasterGameManager
    {
        public static MasterGameManager Instance { get; private set; }
        public EPlayState PlayState { get; private set; } = EPlayState.Initialize;
        public EDifficulty Difficulty { get; private set; } = EDifficulty.None;
        public event Action OnStateChange;

        [SerializeField] private string _easyLevelDataKey;
        [SerializeField] private string _mediumLevelDataKey;
        [SerializeField] private string _hardLevelDataKey;

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

        public void StartNewGame(EDifficulty difficulty)
        {

        }

        public void ContinuePreviousGame()
        {

        }

    }
}