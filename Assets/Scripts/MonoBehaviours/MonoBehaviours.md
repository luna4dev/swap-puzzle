# MonoBehaviour Implementations

Concrete MonoBehaviour implementations for the SwapPuzzle game.

## Folder Structure

**Controllers/** - Scene-specific controllers and game behavior managers
**Managers/** - Application-wide singleton services that persist through runtime  
**Puzzle/** - Puzzle-specific game components and rendering logic

## Architecture Implementation

MonoBehaviours that implement the interface-driven architecture, providing concrete functionality for Unity GameObjects and scene management.


## Script Execution Order

Managers MonoBehaviors should always have higher execution order. Please check Edit > Project Settings > Script Execution Order.