using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class LevelStartPopup : MonoBehaviour, IPopup
    {
        public string ContextName => "ConfirmPopup";
        public int Priority => 2;

        [SerializeField] private Animator _animator;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _levelDisplayText;

        private const string PLAYFADE_TRIGGER = "PlayFade";

        public static IEnumerator PlayPopup(string levelDisplayName)
        {
            Task<LevelStartPopup> taskHandle = PopupController.Current.OpenPopup<LevelStartPopup>();

            while (!taskHandle.IsCompleted)
            {
                yield return null;
            }

            if (taskHandle.IsCompletedSuccessfully)
            {
                LevelStartPopup popup = taskHandle.Result;
                popup.PlayLevelStartPopup(levelDisplayName);
            }
        }

        public void PlayLevelStartPopup(string levelDisplayName)
        {
            _levelDisplayText.text = levelDisplayName;

            // Reset any existing triggers with the same name (optional)
            _animator.ResetTrigger(PLAYFADE_TRIGGER);
            // Set the new trigger
            _animator.SetTrigger(PLAYFADE_TRIGGER);
        }

        public void InitializePopup()
        {
            _image.color = new Color(
                _image.color.r,
                _image.color.g,
                _image.color.b,
                0f
            );
            _levelDisplayText.color = new Color(
                _levelDisplayText.color.r,
                _levelDisplayText.color.g,
                _levelDisplayText.color.b,
                0f
            );
        }

        public void ClosePopup()
        {
            PopupController.Current.ClosePopup(this);
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