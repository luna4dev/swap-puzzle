# SwapPuzzle Interface Architecture

## Overview
This folder contains the interface definitions that form the core architecture of the SwapPuzzle game. The system follows interface-driven design principles for modularity, testability, and maintainability.

## Core Architecture

### Game Management Interfaces

#### `IGameStateManager`
- **Purpose**: Manages global game state transitions
- **States**: MainMenu, Playing, Paused, LevelComplete, ViewingGallery
- **Key Methods**: `ChangeState()`, `SaveGameState()`, `LoadGameState()`

#### `ISaveManager` 
- **Purpose**: High-level save management that coordinates persistence and unlock services
- **Key Methods**: `SaveProgress()`, `LoadProgress()`, `CompleteLevel()`
- **Works with**: `IProgressPersistence`, `IIllustrationUnlockService`, `IProgressData`

#### `IProgressPersistence`
- **Purpose**: Core persistence of game progress data
- **Key Methods**: `SaveProgress()`, `LoadProgress()`, `SaveLevelCompletion()`, `IsDataAvailable()`
- **Works with**: `IProgressData`

#### `IIllustrationUnlockService`
- **Purpose**: Handles illustration unlock logic and validation
- **Key Methods**: `UnlockIllustration()`, `CanUnlockIllustration()`, `GetUnlockRequirements()`
- **Works with**: `IProgressData`

#### `IProgressData`
- **Purpose**: Data contract for player progress
- **Properties**: `CurrentLevel`, `UnlockedLevels[]`, `UnlockedIllustrations[]`, `LastPlayTime`

### Puzzle Game Logic

#### `IPuzzleController`
- **Purpose**: Core puzzle mechanics and game logic
- **Key Methods**: `InitializePuzzle()`, `ShufflePieces()`, `CheckSolution()`, `IsLevelComplete()`
- **Integrates with**: `IPuzzleGrid`

#### `IPuzzleGrid`
- **Purpose**: Unified interface for puzzle grid management and piece swapping mechanics
- **Key Methods**: `InitializeGrid()`, `GetPieceAt()`, `SetPieceAt()`, `InitiateSwap()`, `HandlePieceSelection()`, `CanSwapPieces()`, `ClearSelection()`, `GetSelectedPiece()`
- **Works with**: `IPuzzlePiece`

#### `IPuzzlePiece`
- **Purpose**: Represents individual puzzle pieces
- **Properties**: `OriginalX`, `OriginalY`, `IsSolved`
- **Key Methods**: `MarkAsSolved()`

### Content & Data Management

#### `ILevelData`
- **Purpose**: Level configuration and metadata
- **Properties**: `LevelId`, `Illustration`, `GridSize`, `PreSolvedPieces`, `IsVariation`, `BaseLevel`, `LevelName`

#### `ILevelDataProvider`
- **Purpose**: Provides access to level data
- **Location**: Referenced but not read in detail

#### `IIllustrationData`
- **Purpose**: Illustration metadata and assets
- **Properties**: `IllustrationId`, `IllustrationName`, `IllustrationImage`

#### `IIllustrationGallery`
- **Purpose**: Gallery system for unlocked illustrations
- **Key Methods**: `LoadUnlockedIllustrations()`, `DisplayIllustration()`, `ShowIllustrationDetails()`, `IsIllustrationUnlocked()`

### Scene Management

#### `ISceneController`
- **Purpose**: Base interface for all scene controllers
- **Key Methods**: `InitializeScene()`, `OnSceneEnter()`, `OnSceneExit()`, `CleanupScene()`
- **Works with**: `ISceneTransitionData`

#### `ISceneTransition` & `ISceneTransitionData`
- **Purpose**: Scene transition system
- **Location**: Referenced but not read in detail

### UI Controllers

#### `IMainMenuController`
- **Purpose**: Main menu functionality
- **Location**: Referenced but not read in detail

#### `ILevelSelector`
- **Purpose**: Level selection interface
- **Location**: Referenced but not read in detail

#### `IBookViewer`
- **Purpose**: Book/illustration viewing interface
- **Location**: Referenced but not read in detail

#### `IIndexController`
- **Purpose**: Index/navigation controller
- **Location**: Referenced but not read in detail

### Service Layer

#### `IAssetService`
- **Purpose**: Asset loading and management
- **Location**: Referenced but not read in detail

## Assembly Definition
The interfaces are organized in their own assembly (`SwapPuzzle.Interfaces.asmdef`) for clean separation and compilation boundaries.

## Design Patterns Used

1. **Interface Segregation**: Each interface has a focused, single responsibility
2. **Dependency Injection**: Interfaces allow for easy testing and swapping of implementations
3. **State Pattern**: Game state management through `IGameStateManager`
4. **Observer Pattern**: Implied through state change notifications
5. **Strategy Pattern**: Different controllers implement common interfaces

## Key Relationships

```
IGameStateManager → ISaveManager → IProgressPersistence, IIllustrationUnlockService → IProgressData
IPuzzleController → IPuzzleGrid → IPuzzlePiece
ILevelData → IIllustrationData
ISceneController ← All scene-specific controllers
IIllustrationGallery → IIllustrationData, IProgressData
```

## Usage Guidelines

- All major components should implement these interfaces
- Use dependency injection to provide implementations
- Prefer composition over inheritance
- Keep interfaces focused and cohesive
- Document any breaking changes to interface contracts

## Testing Strategy

The interface-driven design enables:
- Easy mocking for unit tests
- Integration testing with test implementations
- Behavior verification through interface contracts