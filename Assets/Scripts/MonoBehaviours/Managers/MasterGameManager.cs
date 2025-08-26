using UnityEngine;
using SwapPuzzle.Interfaces;
using System;
using SwapPuzzle.Services;
using SwapPuzzle.AssetDefinitions;
using System.Threading.Tasks;
using System.Collections;

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

        public IEnumerator StartNewGame(EDifficulty difficulty)
        {
            // initialize progress manager
            string key = difficulty switch
            {
                EDifficulty.Easy => _easyLevelDataKey,
                EDifficulty.Medium => _mediumLevelDataKey,
                EDifficulty.Hard => _hardLevelDataKey,
                _ => _easyLevelDataKey
            };
            Task<LevelProgressionData> task = AssetService.GetLevelProgressionDataAsync(key);

            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsCompletedSuccessfully)
            {
                LevelProgressionData data = task.Result;
                ProgressManager.Instance.InitializeProgress(data);

                // notify scene manager to change scene
                SceneManager.Instance.LoadScene(ESceneType.Game, ETransitionType.Fade);
                yield break;
            }

            throw new Exception("Exception: failed to load new game");
        }

        public void ContinuePreviousGame()
        {

        }

    }
}