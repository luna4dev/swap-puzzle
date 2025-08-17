using UnityEngine;
using UnityEngine.UI;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    // TODO: move this enum to interface when user preference is implemented
    public enum ESoundPreference
    {
        PlayAll = 0,
        EffectOnly,
        MuteAll,
        Max,
    }

    public struct ContextMenuMode
    {
        public bool ShowSoundButton;
        public bool ShowNavigateOutButton;

        public ContextMenuMode(bool showSoundButton, bool showNavigateOutButton)
        {
            ShowSoundButton = showSoundButton;
            ShowNavigateOutButton = showNavigateOutButton;
        }
    }

    public class ContextMenu : MonoBehaviour
    {
        [SerializeField] Button MuteAllButton;
        [SerializeField] Button EffectOnlyButton;
        [SerializeField] Button PlayAllSoundButton;
        [SerializeField] Button BackButton;
        [SerializeField] Button CloseButton;

        // TODO: move this config to user preference after it is implemented;
        private ESoundPreference _soundPreference = ESoundPreference.PlayAll;

        // TODO: move this config to popupManager after implemented
        private bool _isPopupOpened = false;

        private static readonly Dictionary<ESceneType, ContextMenuMode> _sceneModeMapping = new Dictionary<ESceneType, ContextMenuMode>
        {
            { ESceneType.EntryPoint, new ContextMenuMode(false, false) },
            { ESceneType.MainMenu, new ContextMenuMode(true, false) },
            { ESceneType.Game, new ContextMenuMode(true, true) },
            { ESceneType.Index, new ContextMenuMode(true, true) }
        };

        private void OnEnable()
        {
            SceneManager.Instance.OnSceneChanged += HandleSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.Instance.OnSceneChanged -= HandleSceneChanged;
        }

        private void HandleSceneChanged(ISceneController sceneController)
        {
            if (_sceneModeMapping.TryGetValue(sceneController.Type, out ContextMenuMode mode))
            {
                SetSoundButton(mode.ShowSoundButton);
                SetNavigateOutButton(mode.ShowNavigateOutButton);
            }
        }

        private void SetSoundButton(bool active)
        {
            if (MuteAllButton != null) MuteAllButton.gameObject.SetActive(false);
            if (EffectOnlyButton != null) EffectOnlyButton.gameObject.SetActive(false);
            if (PlayAllSoundButton != null) PlayAllSoundButton.gameObject.SetActive(false);

            if (!active) return;

            switch (_soundPreference)
            {
                case ESoundPreference.PlayAll:
                    if (PlayAllSoundButton != null) PlayAllSoundButton.gameObject.SetActive(true);
                    break;
                case ESoundPreference.EffectOnly:
                    if (EffectOnlyButton != null) EffectOnlyButton.gameObject.SetActive(true);
                    break;
                case ESoundPreference.MuteAll:
                    if (MuteAllButton != null) MuteAllButton.gameObject.SetActive(true);
                    break;
            }
        }

        private void SetNavigateOutButton(bool active)
        {
            if (BackButton != null) BackButton.gameObject.SetActive(false);
            if (CloseButton != null) CloseButton.gameObject.SetActive(false);

            if (!active) return;

            if (_isPopupOpened && CloseButton != null) CloseButton.gameObject.SetActive(true);
            else if (BackButton != null) BackButton.gameObject.SetActive(true);
        }

        public void OnClickNavigateOut()
        {
            InputContextManager.Instance.ProcessInput(InputType.Cancel, new InputData());
        }

        public void OnClickSetSound()
        {
            // TODO
            Debug.Log("Implement this");
        }
    }
}