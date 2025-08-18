using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEngine.Events;
using TMPro;

namespace SwapPuzzle.MonoBehaviours
{
    public class ConfirmPopup : MonoBehaviour, IPopup
    {
        public string ContextName => "ConfirmPopup";
        public int Priority => 2;

        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _confirmText;
        [SerializeField] private TMP_Text _cancelText;

        [SerializeField] private GameObject _backgroundBlocker;

        private UnityAction _onConfirm;
        private UnityAction _onCancel;

        public static async void OpenPopup(
            string title, string description,
            string confrimText = "확인", string cancelText = "취소", bool blockBackground = true,
            UnityAction onConfirm = null, UnityAction onCancel = null
        )
        {
            ConfirmPopup popup = await PopupController.Instance.OpenPopup<ConfirmPopup>();
            popup.Initialize(title, description, confrimText, cancelText, blockBackground, onConfirm, onCancel);
        }

        public void Initialize(
            string title, string description,
            string confrimText = "확인", string cancelText = "취소", bool blockBackground = true,
            UnityAction onConfirm = null, UnityAction onCancel = null
        )
        {
            _title.text = title;
            _description.text = description;
            _confirmText.text = confrimText;
            _cancelText.text = cancelText;

            _backgroundBlocker.gameObject.SetActive(blockBackground);

            _onConfirm = onConfirm;
            _onCancel = onCancel;
        }

        public void OnClickConfirm()
        {
            _onConfirm?.Invoke();
            ClosePopup();
        }

        public void OnClickCancel()
        {
            _onCancel?.Invoke();
            ClosePopup();
        }

        public void ClosePopup()
        {
            PopupController.Instance.ClosePopup(this);
        }

        private bool CanHandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Confirm:
                case InputType.Cancel:
                    return true;
                default:
                    return false;
            }
        }

        public bool HandleInput(InputType inputType, InputData inputData)
        {
            if (!CanHandleInput(inputType)) return false;

            if (inputType == InputType.Confirm) OnClickConfirm();
            if (inputType == InputType.Cancel) OnClickCancel();
            return true;
        }

        public void HandleContextChange()
        {

        }
    }
}