# Claude Context

## Project Overview
Unity puzzle game with drag-and-drop mechanics, level progression system, and unlockable illustration gallery.

## Key Architecture
- Interface-driven design with `ISaveManager`, `IProgressData`, `IGameStateManager`
- Level-based progression system
- Illustration gallery that unlocks with progress
- Build system with custom Unity scripts

## Build Commands
- Build script: `Builds/BuildScripts/build.sh`
- Requires Unity CLI or Unity Hub installation

## Development Notes
- Uses Addressable Assets system
- WebGL target platform
- Git ignores `Builds/Archive/` and `Builds/Version/` folders only