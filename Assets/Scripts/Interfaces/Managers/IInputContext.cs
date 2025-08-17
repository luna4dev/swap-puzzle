using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public enum InputType
    {
        Back,
        Forward,
        Left,
        Right,
        Up,
        Down,
        Confirm,
        Cancel,
        Menu,
        Pan,
        Zoom,
        Select
    }

    public struct InputData
    {
        public Vector2 Position;
        public Vector2 Delta;
        public float Value;
        public bool IsPressed;
        public bool IsReleased;
        public bool IsHeld;

        public InputData(Vector2 position = default, Vector2 delta = default, float value = 0f,
                        bool isPressed = false, bool isReleased = false, bool isHeld = false)
        {
            Position = position;
            Delta = delta;
            Value = value;
            IsPressed = isPressed;
            IsReleased = isReleased;
            IsHeld = isHeld;
        }

        public override string ToString()
        {
            return $"InputData(Pos:{Position}, Delta:{Delta}, Val:{Value}, Pressed:{IsPressed}, Released:{IsReleased}, Held:{IsHeld})";
        }
    }

    public interface IInputContext
    {
        string ContextName { get; }
        int Priority { get; }
        
        bool HandleInput(InputType inputType, InputData inputData);
        void HandleContextChange();
    }
}