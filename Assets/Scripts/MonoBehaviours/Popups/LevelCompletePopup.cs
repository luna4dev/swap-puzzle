using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class LevelCompletePopup : MonoBehaviour, IPopup
    {
        public string ContextName => "LevelCompletePopup";
        public int Priority => 2;

        private bool _uiHidden = false;
        private bool _hasNextLevel = true;

        [SerializeField] private Image _image;
        [SerializeField] private Button NextLevelButton;

        [SerializeField] private Animator _animator;
        private const string BOOL_HIDDEN = "Hidden";

        public static IEnumerator OpenPopup(ILevelData completedLevelData, bool hasNextLevel)
        {
            Task<LevelCompletePopup> taskHandle = PopupController.Current.OpenPopup<LevelCompletePopup>();

            while (!taskHandle.IsCompleted)
            {
                yield return null;
            }

            if (taskHandle.IsCompletedSuccessfully)
            {
                LevelCompletePopup popup = taskHandle.Result;
                popup.Initialize(completedLevelData, hasNextLevel);
            }
        }

        public void Initialize(ILevelData completedLevelData, bool hasNextLevel)
        {
            _image.sprite = completedLevelData.Illustration.Illustration;    
            _hasNextLevel = hasNextLevel;
            NextLevelButton.gameObject.SetActive(_hasNextLevel);
        }

        public void InitializePopup() { }

        public void ClosePopup()
        {
            PopupController.Current.ClosePopup(this);
        }

        public void OnClickPannel()
        {
            if (_uiHidden)
            {
                SetUiHidden(false);
                return;
            }
            SetUiHidden(true);
        }

        private void SetUiHidden(bool hidden)
        {
            _uiHidden = hidden;

            if (_uiHidden)
            {
                ContextMenu.Current.Collapse();
                _animator.SetBool(BOOL_HIDDEN, true);
                return;
            }

            ContextMenu.Current.Inflate();
            _animator.SetBool(BOOL_HIDDEN, false);
        }

        public void OnClickGoHome()
        {
            if (!_hasNextLevel)
            {
                SceneManager.Instance.LoadScene(ESceneType.MainMenu, ETransitionType.Fade);
                return;
            }

            ConfirmPopup.OpenPopup("메인 메뉴로 돌아가기", "메인 메뉴로 돌아가시겠습니까?", "네", "아니오", true, () =>
            {
                SceneManager.Instance.LoadScene(ESceneType.MainMenu, ETransitionType.Fade);
            });
        }

        public void OnClickNextLevel()
        {
            GameController.Current.StartCoroutine(GameController.Current.StartLevel());
            ClosePopup();
        }
    
        private bool CanHandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Up:
                case InputType.Down:
                    return true;
                case InputType.Back:
                case InputType.Cancel:
                    return !_uiHidden;
                case InputType.Confirm:
                    return !_uiHidden && _hasNextLevel;
                default:
                    return false;
            }
        }

        public bool HandleInput(InputType inputType, InputData inputData)
        {
            if (CanHandleInput(inputType) == false) return false;

            // if ui is hidden and input is confirm
            if (inputType == InputType.Up) 
            {
                SetUiHidden(false);
                return true;
            }

            if (inputType == InputType.Down)
            {
                SetUiHidden(true);
                return false;
            }

            if (inputType == InputType.Back || inputType == InputType.Cancel)
            {
                OnClickGoHome();
                return true;
            }

            if (inputType == InputType.Confirm)
            {
                OnClickNextLevel();
                return true;
            }

            return true;
        }

        public void HandleContextChange()
        {

        }
        
    }
}