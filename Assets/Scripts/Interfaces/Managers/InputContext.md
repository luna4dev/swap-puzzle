# Input Context System Implementation

## Overview
Complete input context system for Unity puzzle game that handles mouse, keyboard, and touch input through a unified pipeline. Uses `InputManager` → `InputContextManager` → `IInputContext` architecture.

## Architecture Overview

### Current Implementation
The system consists of three main components:

1. **InputManager** (MonoBehaviour): Raw input collection and conversion
2. **InputContextManager** (MonoBehaviour): Context management and routing  
3. **IInputContext**: Context-specific input handling

### Input Pipeline
```
Unity Input → InputManager → InputData → InputContextManager → CurrentContext.HandleInput()
```

### Scene Integration
- InputContextManager subscribes to SceneManager events
- Input disabled during scene transitions
- Scene controllers that implement IInputContext become active contexts automatically

## InputManager Implementation

### Supported Input Types
```csharp
public enum InputType
{
    Back, Forward, Left, Right, Up, Down,
    Confirm, Cancel, Menu, Pan, Zoom, Select
}
```

### Mouse Input Handling
- **Click**: Maps to `Select` InputType with world position
- **Drag**: Starts after drag threshold, maps to `Pan` with delta movement
- **Scroll**: Maps to `Zoom` with scroll value
- **Position**: Automatically converted from screen to world coordinates

### Keyboard Input Handling  
- **WASD/Arrow Keys**: Map to directional InputTypes (Up/Down/Left/Right)
- **Enter/Space**: Map to `Confirm`
- **Escape**: Maps to `Cancel`  
- **Tab**: Maps to `Menu`

### InputData Structure
```csharp
public struct InputData
{
    public Vector2 Position;    // World position
    public Vector2 Delta;       // Movement delta
    public float Value;         // Scroll/intensity value
    public bool IsPressed;      // Input started
    public bool IsReleased;     // Input ended
    public bool IsHeld;         // Input continuing
}
```

## InputContextManager Implementation

### Context Management
- **Singleton pattern**: Persistent across scene changes
- **Context stack**: Push/pop system for layered contexts (modals, menus)
- **Scene integration**: Automatically sets scene controllers as contexts
- **Input gating**: Disables input during scene transitions

### Key Methods
```csharp
void PushContext(IInputContext context)      // Add context to stack
bool PopContext()                            // Remove top context
void ClearAllContexts()                      // Clear all contexts
bool ProcessInput(InputType, InputData)      // Route input to current context
```

## IInputContext Interface

### Implementation Requirements
```csharp
public interface IInputContext
{
    string ContextName { get; }           // Debug identifier
    int Priority { get; }                 // Context priority level
    
    bool HandleInput(InputType, InputData);  // Process input
    void HandleContextChange();              // Context activation callback
}
```

### Context Behavior
- **HandleInput**: Return true if input was consumed, false to pass through
- **HandleContextChange**: Called when context becomes active
- **Priority**: Higher priority contexts handled first (future enhancement)

### Scene Controller Integration
Scene controllers implementing IInputContext automatically become active contexts when scenes load, providing seamless input handling per scene.

## Usage Examples

### Creating a Custom Context
```csharp
public class PuzzleContext : MonoBehaviour, IInputContext
{
    public string ContextName => "PuzzleGame";
    public int Priority => 0;

    public bool HandleInput(InputType inputType, InputData inputData)
    {
        switch (inputType)
        {
            case InputType.Select:
                if (inputData.IsPressed) SelectPiece(inputData.Position);
                return true;
                
            case InputType.Pan:
                if (inputData.IsHeld) DragPiece(inputData.Delta);
                if (inputData.IsReleased) DropPiece();
                return true;
                
            case InputType.Cancel:
                CancelCurrentAction();
                return true;
        }
        return false; // Not handled
    }

    public void HandleContextChange()
    {
        // Context became active
        Debug.Log("Puzzle context activated");
    }
}
```

### System Integration
The system is fully integrated with existing scene management:
- **SceneManager**: Triggers context changes on scene transitions
- **Controllers**: Scene controllers automatically become input contexts
- **Singleton Management**: All managers persist across scenes
- **Input Gating**: Prevents input during scene loading

### Debugging
- InputData includes comprehensive ToString() for logging
- Context names provide clear debugging information
- Input can be easily enabled/disabled for testing

## Current Status
✅ **Complete**: Input collection, context management, scene integration  
✅ **Tested**: Basic mouse/keyboard input working  
⚠️ **Pending**: Touch input support for mobile browsers  
⚠️ **Pending**: Advanced gesture recognition (pinch, complex swipes)