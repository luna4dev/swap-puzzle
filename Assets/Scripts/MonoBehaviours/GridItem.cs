using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class GridItem : MonoBehaviour
    {
        public int GridId { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        [SerializeField] private GridSystem _gridSystem;
        [SerializeField] private TMP_Text _debugText;
        [SerializeField] private Image _image;

        private void OnEnable()
        {
            if (_gridSystem != null) _gridSystem.OnGridSizeChanged += OnGridSizeChanged;
        }
        private void OnDisable()
        {
            if (_gridSystem != null) _gridSystem.OnGridSizeChanged -= OnGridSizeChanged;
        }

        public void Initialize(GridSystem gridSystem, int gridId, int x, int y)
        {
            _gridSystem = gridSystem;
            GridId = gridId;
            X = x;
            Y = y;
        }

        private void OnGridSizeChanged(float size)
        {
            transform.localScale = new Vector3(size, size, size);
            SetPosition(X, Y);
        }

        public void SetDebug(bool isDebug, int number)
        {

            if (_debugText && _image)
            {
                Color color = Random.ColorHSV();
                float luminance = color.r * 0.2126f + color.g * 0.7152f + color.g * 0.0722f;

                _image.color = color;

                _debugText.text = "" + number;
                _debugText.color = luminance > 0.5f ? Color.black : Color.white;
                _debugText.gameObject.SetActive(isDebug);
            }
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = new(0, 1);
            rectTransform.anchorMax = new(0, 1);
            rectTransform.anchoredPosition = _gridSystem.GetTopLeftAnchorPositionFromLogicalCell(x, y);
            rectTransform.localScale = new(1, 1, 1);
        }

        public void SetPosition(Vector2Int cell) {
            SetPosition(cell.x, cell.y);
        }

        // TODO: remove
        public void TestSwap()
        {
            GridItem droppedGridItem = UIDragDrop.Dropped.gameObject.GetComponent<GridItem>();
            GridItem targetGridItem = UIDragDrop.DroppedTarget.gameObject.GetComponent<GridItem>();

            Vector2Int droppedCell = new(droppedGridItem.X, droppedGridItem.Y);
            Vector2Int targetCell = new(targetGridItem.X, targetGridItem.Y);

            droppedGridItem.SetPosition(targetCell); 
            targetGridItem.SetPosition(droppedCell);
        }
    }
}
