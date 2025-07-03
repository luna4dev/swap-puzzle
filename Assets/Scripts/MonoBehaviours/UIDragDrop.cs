using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SwapPuzzle.MonoBehaviours
{
    public class UIDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// The most recent UIDragDrop object that is Dropped
        /// </summary>
        public static UIDragDrop Dropped;
        /// <summary>
        /// The most recent UIDragDrop object that was being targeted
        /// </summary>
        public static UIDragDrop DroppedTarget;

        [SerializeField] public UnityEvent OnDrop;

        private Vector2 _originalPosition;
        private Transform _originalParent;
        private Vector2 _originalAnchorMin;
        private Vector2 _originalAnchorMax;
        private int _originalSiblingIndex;
        private RectTransform _rectTransform;
        private Vector2 _dragOffset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();

            _originalPosition = transform.position;
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            // Calculate offset from pointer to object center
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition);
            _dragOffset = localPointerPosition;

            _originalAnchorMin = _rectTransform.anchorMin;
            _originalAnchorMax = _rectTransform.anchorMax;
            _rectTransform.anchorMin = new(0.5f, 0.5f);
            _rectTransform.anchorMax = new(0.5f, 0.5f);

            transform.SetParent(UIDragRoot.Instance.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                UIDragRoot.Instance.RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition))
            {
                // Account for the drag offset so object doesn't snap to pointer
                _rectTransform.anchoredPosition = localPointerPosition - _dragOffset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();

            // rollback position
            transform.position = _originalPosition;
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(_originalSiblingIndex);

            _rectTransform.anchorMin = _originalAnchorMin;
            _rectTransform.anchorMax = _originalAnchorMax;

            UIDragDrop target = RaycastDrop(eventData);
            if (target == null) return;

            Dropped = this;
            DroppedTarget = target;
            OnDrop?.Invoke();
        }

        public UIDragDrop RaycastDrop(PointerEventData eventData)
        {
            List<RaycastResult> result = new();
            EventSystem.current.RaycastAll(eventData, result);

            if (result.Count == 0) return null;

            // find the droppable beneath
            UIDragDrop target = null;
            foreach (var item in result)
            {
                UIDragDrop current = item.gameObject.GetComponent<UIDragDrop>();
                var test = item.gameObject.GetComponent<GridItem>();
                if (current == null) continue;
                target = current;
            }
            return target;
        }
    }
}