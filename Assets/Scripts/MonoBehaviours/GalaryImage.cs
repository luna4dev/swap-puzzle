using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SwapPuzzle.MonoBehaviours
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class GalaryImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void Focus();
        public event Focus OnFocus;
        public delegate void Blur();
        public event Blur OnBlur;

        [Header("Zoom Settings")]
        [SerializeField] private float minZoom = 1f;
        [SerializeField] private float maxZoom = 5f;
        [SerializeField] private float zoomSpeed = 0.1f;

        [Header("Focus Settings")]
        [SerializeField] private float dragThreshold = 5f;

        private RectTransform rectTransform;
        private RectTransform parentRectTransform;
        private Image image;
        private Canvas canvas;

        private float currentZoom = 1f;
        private Vector2 initialImageSize;

        // Focus tracking
        private bool isFocused = false;
        private bool isDragging = false;
        private float totalDragDistance = 0f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();

            InitializeImageSize();
        }

        private void InitializeImageSize()
        {
            if (image.sprite == null)
            {
                Debug.LogWarning("GalaryImage: No sprite assigned to Image component");
                return;
            }

            // Get the sprite's native size
            Sprite sprite = image.sprite;
            float spriteWidth = sprite.rect.width;
            float spriteHeight = sprite.rect.height;
            float aspectRatio = spriteWidth / spriteHeight;

            // Get parent's size
            Vector2 parentSize = parentRectTransform.rect.size;
            float parentAspectRatio = parentSize.x / parentSize.y;

            // Calculate scale to fit entire image in parent (letterbox/pillarbox)
            float scale;
            if (aspectRatio > parentAspectRatio)
            {
                // Image is wider - fit to width
                scale = parentSize.x / spriteWidth;
            }
            else
            {
                // Image is taller - fit to height
                scale = parentSize.y / spriteHeight;
            }

            // Set the image size to fit parent
            initialImageSize = new Vector2(spriteWidth * scale, spriteHeight * scale);
            rectTransform.sizeDelta = initialImageSize;

            // Center the image
            rectTransform.anchoredPosition = Vector2.zero;

            // Reset zoom
            currentZoom = 1f;
            rectTransform.localScale = Vector3.one;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Ignore right click
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                SetBlur();
                return;
            }

            totalDragDistance = 0f;
            isDragging = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Ignore right click
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            // If we didn't drag enough, it's a click - trigger blur
            if (!isDragging || totalDragDistance < dragThreshold)
            {
                SetBlur();
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            // Get scroll delta
            float scrollDelta = eventData.scrollDelta.y;

            // Calculate new zoom level
            float newZoom = currentZoom + scrollDelta * zoomSpeed;
            newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

            if (Mathf.Approximately(newZoom, currentZoom))
                return;

            // Scrolling triggers focus
            SetFocus();

            // Get mouse position in local space before zoom
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localMousePos
            );

            // Calculate the position offset
            Vector2 pivotOffset = localMousePos;

            // Apply zoom
            float zoomRatio = newZoom / currentZoom;
            currentZoom = newZoom;
            rectTransform.localScale = Vector3.one * currentZoom;

            // Adjust position to zoom towards mouse cursor
            Vector2 newPivotOffset = pivotOffset * zoomRatio;
            Vector2 offset = pivotOffset - newPivotOffset;
            rectTransform.anchoredPosition += offset;

            // Apply constraints
            ConstrainToParent();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Track total drag distance
            float dragDelta = eventData.delta.magnitude;
            totalDragDistance += dragDelta;

            // If we've exceeded the threshold, set focus
            if (totalDragDistance >= dragThreshold)
            {
                SetFocus();
            }

            // Convert drag delta to local space
            Vector2 delta = eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += delta;

            // Apply constraints
            ConstrainToParent();
        }

        private void SetFocus()
        {
            if (!isFocused)
            {
                isFocused = true;
                OnFocus?.Invoke();
            }
        }

        private void SetBlur()
        {
            if (isFocused)
            {
                isFocused = false;
                OnBlur?.Invoke();
            }
        }

        private void ConstrainToParent()
        {
            if (parentRectTransform == null)
                return;

            // Get the current size of the image with zoom applied
            Vector2 currentSize = initialImageSize * currentZoom;
            Vector2 parentSize = parentRectTransform.rect.size;

            // Calculate bounds
            Vector2 minPosition = new Vector2(
                (parentSize.x - currentSize.x) / 2f,
                (parentSize.y - currentSize.y) / 2f
            );
            Vector2 maxPosition = new Vector2(
                (currentSize.x - parentSize.x) / 2f,
                (currentSize.y - parentSize.y) / 2f
            );

            // Clamp position
            Vector2 pos = rectTransform.anchoredPosition;

            // Only constrain if image is larger than parent in that dimension
            if (currentSize.x > parentSize.x)
            {
                pos.x = Mathf.Clamp(pos.x, -maxPosition.x, maxPosition.x);
            }
            else
            {
                pos.x = 0; // Center if smaller than parent
            }

            if (currentSize.y > parentSize.y)
            {
                pos.y = Mathf.Clamp(pos.y, -maxPosition.y, maxPosition.y);
            }
            else
            {
                pos.y = 0; // Center if smaller than parent
            }

            rectTransform.anchoredPosition = pos;
        }

        // Public method to initialize with a sprite
        public void InitializeImage(Sprite sprite)
        {
            if (image != null)
            {
                image.sprite = sprite;
                InitializeImageSize();
            }
        }

        // Public method to reset view
        public void ResetView()
        {
            InitializeImageSize();
        }

        // Public method to set a new sprite
        public void SetSprite(Sprite sprite)
        {
            if (image != null)
            {
                image.sprite = sprite;
                InitializeImageSize();
            }
        }
    }

}