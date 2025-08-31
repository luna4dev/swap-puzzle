#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzleGrid))]
public class PuzzleGridEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);
    }
}
#endif