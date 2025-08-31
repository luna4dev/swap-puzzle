using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;
using SwapPuzzle.Services;
using System.Threading.Tasks;

namespace SwapPuzzle.MonoBehaviours
{
    public class PopupController : MonoBehaviour, IPopupController
    {
        public static PopupController Instance { get; private set; }

        private Dictionary<System.Type, IPopup> _popupDictionary = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (!ReferenceEquals(this, Instance))
            {
                PopupController prev = Instance;
                Instance = this;
                Destroy(prev);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public async Task<T> OpenPopup<T>() where T : IPopup
        {
            System.Type popupType = typeof(T);

            // Check if popup already exists
            if (_popupDictionary.TryGetValue(popupType, out IPopup existingPopup))
            {
                // Reactivate existing popup
                if (existingPopup is MonoBehaviour mb)
                {
                    mb.gameObject.SetActive(true);
                }

                // initialize
                existingPopup.InitializePopup();
                return (T)existingPopup;
            }

            // Create new popup instance
            GameObject prefab = await AssetService.GetPopupAsync(popupType.Name);
            GameObject popupInstance = Instantiate(prefab, transform);
            Component[] components = popupInstance.GetComponents(typeof(IPopup));

            T popup = default;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T foundPopup)
                {
                    popup = foundPopup;
                }
            }

            if (popup != null)
            {
                _popupDictionary[popupType] = popup;
                popup.InitializePopup();
                return popup;
            }

            throw new System.Exception("Popup " + popupType.Name + " is not properly configured: IPopup not exists");
        }

        public void ClosePopup(IPopup popup)
        {
            if (popup == null) return;

            // Deactivate popup instead of destroying
            if (popup is MonoBehaviour mb)
            {
                mb.gameObject.SetActive(false);
            }
        }

        public void CloseAllPopup()
        {
            foreach (var popup in _popupDictionary.Values)
            {
                if (popup is MonoBehaviour mb)
                {
                    mb.gameObject.SetActive(false);
                }
            }
        }
    }
}