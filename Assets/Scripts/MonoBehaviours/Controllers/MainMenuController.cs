using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEngine.UI;
using SwapPuzzle.Services;

namespace SwapPuzzle.MonoBehaviours
{
    public class MainMenuController : MonoBehaviour, IMainMenuController
    {
        public ESceneType Type => ESceneType.MainMenu;
        public string ContextName => "MainMenu";
        public int Priority => 1;

        [SerializeField] private Button NewGameButton;
        [SerializeField] private Button ContinueGameButton;
        [SerializeField] private Button EasyButton;
        [SerializeField] private Button MediumButton;
        [SerializeField] private Button HardButton;
        [SerializeField] private Button BackButton;
        [SerializeField] private Button IndexButton;

        private bool _difficultySelectNavigation = false;

        private void OnEnable()
        {
            InputContextManager.Instance.OnContextChanged += HandleContextChange;
            SetDifficultyNavigationMode(false);
        }

        private void OnDisable()
        {
            InputContextManager.Instance.OnContextChanged -= HandleContextChange;
        }

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

        private void SetDifficultyNavigationMode(bool status)
        {
            _difficultySelectNavigation = status;

            // if true
            if (status == true)
            {
                NewGameButton.gameObject.SetActive(false);
                ContinueGameButton.gameObject.SetActive(false);

                EasyButton.gameObject.SetActive(true);
                MediumButton.gameObject.SetActive(true);
                HardButton.gameObject.SetActive(true);
                BackButton.gameObject.SetActive(true);

                IndexButton.gameObject.SetActive(false);
                return;
            }

            // if not
            NewGameButton.gameObject.SetActive(true);
            SetContinueGameButton();

            EasyButton.gameObject.SetActive(false);
            MediumButton.gameObject.SetActive(false);
            HardButton.gameObject.SetActive(false);
            BackButton.gameObject.SetActive(false);

            IndexButton.gameObject.SetActive(true);
        }

        private void SetContinueGameButton()
        {
            bool hasSavedProgress = SaveLoadService.HasSavedProgress();
            if (hasSavedProgress)
            {
                ContinueGameButton.gameObject.SetActive(true);
                return;
            }
            ContinueGameButton.gameObject.SetActive(false);
        }

        public void OnNewGameButtonPressed()
        {
            bool hasSavedProgress = SaveLoadService.HasSavedProgress();

            if (hasSavedProgress)
            {
                ConfirmPopup.OpenPopup("새로운 게임", "이전에 플레이하던 게임을 버리고 새로운 게임을 시작합니다", "네", "아니오", true, () =>
                {
                    SetDifficultyNavigationMode(true);
                });
                return;
            }

            SetDifficultyNavigationMode(true);
        }

        public void OnContinueGameButtonPressed()
        {

        }

        public void OnEasyButtonPressed()
        {
            StartCoroutine(MasterGameManager.Instance.StartNewGame(EDifficulty.Easy));
        }

        public void OnMediumButtonPressed()
        {
            StartCoroutine(MasterGameManager.Instance.StartNewGame(EDifficulty.Medium));
        }

        public void OnHardButtonPressed()
        {
            StartCoroutine(MasterGameManager.Instance.StartNewGame(EDifficulty.Hard));
        }

        public void OnBackButtonPressed()
        {
            SetDifficultyNavigationMode(false);
        }

        public void OnIndexButtonPressed()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.LoadScene(ESceneType.Index, ETransitionType.Fade);
            }
        }

        public bool CanHandleInput(InputType inputType)
        {
            return false;
        }

        public bool HandleInput(InputType inputType, InputData inputData)
        {
            return false;
        }

        public void HandleContextChange()
        {

        }
    }
}