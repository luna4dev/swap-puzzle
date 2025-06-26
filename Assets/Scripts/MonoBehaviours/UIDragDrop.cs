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
        private int _originalSiblingIndex;
        private RectTransform _rectTransform;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();

            _originalPosition = transform.position;
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            transform.SetParent(UIDragRoot.Instance.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 newPos = new(_rectTransform.localPosition.x + eventData.delta.x, _rectTransform.localPosition.y + eventData.delta.y);
            _rectTransform.localPosition = newPos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // rollback position
            transform.position = _originalPosition;
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(_originalSiblingIndex);

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