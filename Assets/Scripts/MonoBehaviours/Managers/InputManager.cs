using UnityEngine;
using System;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.MonoBehaviours
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [SerializeField] private float _dragThreshold = 10f;
        [SerializeField] private bool _enableInput = true;

        private Vector2 _lastMousePosition;
        private Vector2 _dragStartPosition;
        private bool _isDragging = false;
        private bool _mousePressed = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!_enableInput) return;

            HandleMouseInput();
            HandleKeyboardInput();
        }

        private void HandleMouseInput()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 mouseDelta = mousePosition - _lastMousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _mousePressed = true;
                _dragStartPosition = mousePosition;
                _isDragging = false;

                SendInput(InputType.Select, new InputData(
                    position: Camera.main.ScreenToWorldPoint(mousePosition),
                    isPressed: true
                ));
            }

            if (Input.GetMouseButton(0) && _mousePressed)
            {
                float dragDistance = Vector2.Distance(mousePosition, _dragStartPosition);
                
                if (!_isDragging && dragDistance > _dragThreshold)
                {
                    _isDragging = true;
                }

                if (_isDragging)
                {
                    SendInput(InputType.Pan, new InputData(
                        position: Camera.main.ScreenToWorldPoint(mousePosition),
                        delta: Camera.main.ScreenToWorldPoint(mouseDelta) - Camera.main.ScreenToWorldPoint(Vector2.zero),
                        isHeld: true
                    ));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isDragging)
                {
                    SendInput(InputType.Pan, new InputData(
                        position: Camera.main.ScreenToWorldPoint(mousePosition),
                        isReleased: true
                    ));
                }
                else
                {
                    SendInput(InputType.Select, new InputData(
                        position: Camera.main.ScreenToWorldPoint(mousePosition),
                        isReleased: true
                    ));
                }

                _mousePressed = false;
                _isDragging = false;
            }

            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                SendInput(InputType.Zoom, new InputData(
                    position: Camera.main.ScreenToWorldPoint(mousePosition),
                    value: scrollDelta
                ));
            }

            _lastMousePosition = mousePosition;
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SendInput(InputType.Cancel, new InputData(isPressed: true));
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                SendInput(InputType.Confirm, new InputData(isPressed: true));
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SendInput(InputType.Menu, new InputData(isPressed: true));
            }

            Vector2 movement = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                movement.y += 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                movement.y -= 1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                movement.x -= 1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                movement.x += 1;

            if (movement != Vector2.zero)
            {
                if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
                {
                    SendInput(movement.x > 0 ? InputType.Right : InputType.Left, 
                        new InputData(value: Mathf.Abs(movement.x), isHeld: true));
                }
                else
                {
                    SendInput(movement.y > 0 ? InputType.Up : InputType.Down,
                        new InputData(value: Mathf.Abs(movement.y), isHeld: true));
                }
            }
        }

        private void SendInput(InputType inputType, InputData inputData)
        {
            // Debug.Log(inputType + ": " + inputData);
            if (InputContextManager.Instance != null)
            {
                InputContextManager.Instance.ProcessInput(inputType, inputData);
            }
        }

        public void SetInputEnabled(bool enabled)
        {
            _enableInput = enabled;
        }

        public bool IsInputEnabled()
        {
            return _enableInput;
        }
    }
}