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

        public static IEnumerator PlayPopup(string levelDisplayName)
        {
            Task<LevelStartPopup> taskHandle = PopupController.Instance.OpenPopup<LevelStartPopup>();

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
            StartCoroutine(PlayFadeSequence());
        }

        private IEnumerator PlayFadeSequence()
        {
            Debug.Log("wait for 3");
            yield return new WaitForSeconds(3f);
            Debug.Log("Play fade");

            _animator.SetBool("Play", true);

            // Wait for FadeOut to complete
            yield return new WaitForSeconds(3f);

            Debug.Log("Completed");

            // Close popup after animation completes
            // ClosePopup();
        }

        public void InitializePopup()
        {
            _animator.SetBool("Play", false);
            _animator.Play("Hidden", -1, 0f);
            _animator.speed = 1f;

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
            PopupController.Instance.ClosePopup(this);
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