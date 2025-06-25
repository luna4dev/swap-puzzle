using UnityEngine;

namespace SwapPuzzle.MonoBehaviors
{
    public class GridItem : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        [SerializeField] private GridSystem _gridSystem;

        private void OnEnable() {
            _gridSystem.OnGridSizeChanged += OnGridSizeChanged;
        }
        private void OnDisable() {
            _gridSystem.OnGridSizeChanged -= OnGridSizeChanged;
        }


        public void Initialize(GridSystem gridSystem, int x, int y)
        {
            _gridSystem = gridSystem;
            X = x;
            Y = y;
        }

        private void OnGridSizeChanged(int size) {
            transform.localScale = new Vector3(size, size, size);
            SetPosition(X, Y);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            transform.position = _gridSystem.GetGridPosition(x, y);
        }
    }
}
