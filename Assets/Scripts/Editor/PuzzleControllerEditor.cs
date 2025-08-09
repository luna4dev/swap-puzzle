#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzleController))]
public class PuzzleControllerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Test")) {
            (target as PuzzleController).InitializePuzzle(999);
        }
    }
}
#endif