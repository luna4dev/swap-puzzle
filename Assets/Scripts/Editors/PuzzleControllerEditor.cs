using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzleController))]
public class PuzzleControllerEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Test")) {
            (target as PuzzleController).InitializePuzzle(999);
        }
    }
}