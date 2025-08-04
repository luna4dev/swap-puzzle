#!/bin/bash

# Unity WebGL Build Script
# Usage: ./build.sh [version]
# Version: optional, defaults to current timestamp

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
UNITY_PROJECT_PATH="$(pwd)"
BUILD_SCRIPTS_PATH="$UNITY_PROJECT_PATH/Builds/BuildScripts"
BUILDS_PATH="$UNITY_PROJECT_PATH/Builds"

# Default values
VERSION=${1:-$(date +"%Y%m%d_%H%M%S")}

echo -e "${BLUE}=== Unity WebGL Build Script ===${NC}"
echo -e "${YELLOW}Version: $VERSION${NC}"
echo -e "${YELLOW}Project Path: $UNITY_PROJECT_PATH${NC}"

# Create version directory
VERSION_PATH="$BUILDS_PATH/Version/v$VERSION"

mkdir -p "$VERSION_PATH"

# Build settings
BUILD_METHOD="BuildScripts.BuildWebGLProduction"
DEFINES="PRODUCTION_BUILD;UNITY_WEBGL"

# Check if Unity is available
if ! command -v unity-hub &> /dev/null && ! command -v Unity &> /dev/null; then
    echo -e "${RED}Error: Unity not found. Please install Unity Hub or Unity CLI.${NC}"
    exit 1
fi

# Find Unity executable
UNITY_EXECUTABLE=""
if command -v unity-hub &> /dev/null; then
    # Try to find Unity through Unity Hub
    UNITY_EXECUTABLE=$(find /Applications -name "Unity" -type f 2>/dev/null | head -n 1)
fi

if [ -z "$UNITY_EXECUTABLE" ]; then
    echo -e "${RED}Error: Unity executable not found. Please ensure Unity is installed.${NC}"
    exit 1
fi

echo -e "${BLUE}Using Unity: $UNITY_EXECUTABLE${NC}"

# Build command
echo -e "${BLUE}Starting build...${NC}"

"$UNITY_EXECUTABLE" \
    -batchmode \
    -quit \
    -projectPath "$UNITY_PROJECT_PATH" \
    -executeMethod "$BUILD_METHOD" \
    -buildTarget WebGL \
    -logFile "$BUILDS_PATH/build.log" \
    -buildPath "$BUILD_PATH"

# Check build result
if [ $? -eq 0 ]; then
    echo -e "${GREEN}Build completed successfully!${NC}"
    echo -e "${BLUE}Build location: $BUILD_PATH${NC}"
    
    # Create a README for the build
    cat > "$BUILD_PATH/README.md" << EOF
# WebGL Build

**Version:** $VERSION  
**Build Date:** $(date)  
**Build Path:** $BUILD_PATH

## Files
- \`index.html\` - Main entry point
- \`Build/\` - Unity WebGL build files
- \`StreamingAssets/\` - Streaming assets (if any)

## Deployment
This build can be deployed to any static web hosting service.

## Notes
- Built with Unity WebGL target
- Version: $VERSION
EOF

    # Update Latest symlink
    if [ -L "$BUILDS_PATH/Version/Latest" ]; then
        rm "$BUILDS_PATH/Version/Latest"
    fi
    ln -sf "v$VERSION" "$BUILDS_PATH/Version/Latest"
    
    echo -e "${GREEN}Build documentation created${NC}"
    echo -e "${GREEN}Latest symlink updated${NC}"
else
    echo -e "${RED}Build failed! Check the log file: $BUILDS_PATH/build.log${NC}"
    exit 1
fi 