using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    public class UIDragRoot : MonoBehaviour
    {
        public static UIDragRoot Instance;
        private RectTransform _rectTransform;
        public RectTransform RectTransform {
            get {
                if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        public void OnEnable() {
            Instance = this;
        }
    }
}