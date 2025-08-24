# Asset Definitions

ScriptableObject definitions for configurable game data in the SwapPuzzle game.

## Core Concepts

### Illustration
Represents a single illustration asset. Holds link to texture and will contain metadata in the future for illustration-specific properties.

### Level
Game level configuration used in gameplay. Each level links to a single illustration and contains level-specific settings and configuration data.

### Gallery
Page configuration for the index book that becomes accessible after unlocking illustrations. Each gallery represents one page in the index with metadata about that page.

### Progression Data
Tracks player progression through the game. Contains progression state and can hold metadata about the progression itself.

## Asset Definitions

ScriptableObject classes that define data structures for assets configured during development and loaded at runtime.