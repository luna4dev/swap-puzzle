# Navigation Context System Design

## Overview
Design for a navigation context system that handles both mobile browser input (primary) and mouse/keyboard input (secondary) using the existing `IInputContext` and `IInputContextManager` infrastructure.

## Core Principles

### Mobile-First Design
- **Primary**: Touch/tap interactions, swipe gestures
- **Secondary**: Mouse clicks, keyboard navigation
- **Responsive**: Adapt behavior based on input method detection

### Context-Aware Navigation
Different scenes/states require different navigation behaviors:
- **Menu Navigation**: List/grid selection, back/forward flow
- **Puzzle Interaction**: Drag-and-drop, piece selection
- **Gallery Browsing**: Swipe navigation, zoom interactions
- **Dialog/Modal**: Focus trapping, escape handling

## Input Mapping Strategy

### Touch/Mouse Unified Handling
```
Touch/Click → Select InputType
Swipe Left/Right → Left/Right InputType  
Swipe Up/Down → Up/Down InputType
Pinch → Zoom InputType
Pan → Pan InputType
```

### Gesture Recognition
- **Tap**: Quick press/release for selection
- **Hold**: Long press for context menus or alternative actions
- **Swipe**: Directional movement for navigation
- **Pinch**: Two-finger zoom (gallery/puzzle view)

### Keyboard Fallbacks
- **Arrow Keys**: Map to Up/Down/Left/Right
- **Enter/Space**: Map to Confirm
- **Escape**: Map to Cancel/Back
- **Tab**: Focus navigation in menus

## Context Hierarchy Design

### Navigation Strategy

The navigation system uses action-based routing where common navigation actions have different behaviors depending on the current context state.

### Core Navigation Actions

#### Go Back Action
- **Root Navigation**: Exit application or return to entry point
- **Nested Navigation**: Return to parent navigation level
- **Modal/Popup**: Close overlay and return to underlying context
- **Sub-Menu**: Return to parent menu
- **Interactive State**: Cancel current interaction and return to idle

#### Go Forward Action
- **Root Navigation**: Proceed to main content area
- **Selection Context**: Confirm selection and advance
- **Interactive State**: Complete current action
- **Modal/Popup**: Execute primary action and close

#### Navigate Up/Down Actions
- **List/Grid Navigation**: Move focus vertically
- **Hierarchical Menus**: Navigate between menu levels
- **Interactive Elements**: Move between focusable UI elements
- **Content Browsing**: Scroll or navigate content vertically

#### Navigate Left/Right Actions
- **Tab Navigation**: Switch between horizontal sections
- **Content Browsing**: Navigate content horizontally
- **Multi-Option Selection**: Move between choices
- **Interactive Elements**: Navigate within grouped controls

#### Select/Confirm Action
- **Focused Element**: Activate currently focused item
- **Interactive Mode**: Pick up/place or toggle selection
- **Navigation Context**: Enter selected area or confirm choice

### Context Stack Management
- Use existing `IInputContextManager` push/pop system
- Actions are handled by the current top context
- Unhandled actions bubble down to lower priority contexts
- Graceful fallback when contexts are removed

## Implementation Considerations

### Mobile Browser Constraints
- **No native gestures**: Implement custom gesture recognition
- **Touch precision**: Larger hit areas for touch targets
- **Performance**: Efficient gesture detection without frame drops
- **Viewport handling**: Account for virtual keyboards and browser UI

### Responsive Behavior
- **Input method detection**: Switch between touch and mouse modes
- **Dynamic hit areas**: Adjust based on input method
- **Feedback systems**: Different visual/audio feedback for touch vs mouse

### Accessibility
- **Keyboard navigation**: Full keyboard support as fallback
- **Focus management**: Clear focus indicators and logical tab order
- **Screen reader support**: Proper ARIA-like announcements

## Integration Points

### Existing Systems
- **Scene Management**: Context switching on scene transitions
- **UI Systems**: Integration with Unity UI components
- **Save System**: Remember navigation preferences
- **Asset System**: Load gesture/input configuration from addressables

### Performance Optimization
- **Gesture pooling**: Reuse gesture detection objects
- **Input buffering**: Smooth input handling during frame drops
- **Context caching**: Avoid recreation of common contexts

## Next Steps
1. Implement BaseNavigationContext with core gesture recognition
2. Create specific context implementations for each scene type
3. Integrate with existing scene management system
4. Add mobile browser optimization and testing
5. Implement keyboard navigation fallbacks