#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzlePiece))]
public class PuzzlePieceEditor : Editor {

    private int _x = 0;
    private int _y = 0;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

    }
}
#endif