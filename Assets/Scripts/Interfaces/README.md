# Interface Definitions

Interface definitions for the SwapPuzzle game architecture.

## Folder Structure

**Components/** - Interfaces for game components (grid, pieces)
**Controllers/** - Interfaces for scene and game controllers
**Data/** - Interfaces for scriptable object definitions (configured in development)
**Managers/** - Interfaces for singleton services (instantiated at startup, persist through runtime)
**Runtime/** - Interfaces for runtime data (progress, saved game state)
**Services/** - Interfaces for stateless utility services
**Utilities/** - Interfaces for helper utilities

## Architecture Types

| Type | Lifecycle | State | Purpose | Examples |
|------|-----------|-------|---------|----------|
| **Controllers** | Scene-scoped | Stateful | Manage specific scenes or major behaviors | Scene controllers, puzzle controllers |
| **Managers** | Application-wide | Stateful | Singleton services that persist through runtime | Game state, save management |
| **Services** | On-demand | Stateless | Utility functions and operations | Asset loading, data persistence |