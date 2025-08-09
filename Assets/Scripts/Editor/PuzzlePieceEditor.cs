#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzlePiece))]
public class PuzzlePieceEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);
    }
}
#endif