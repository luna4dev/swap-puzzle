#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzlePieceRenderer))]
public class PuzzlePieceRendererEditor : Editor {

    private bool _solvedStatus = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Set Solved Status", EditorStyles.boldLabel);
        _solvedStatus = EditorGUILayout.Toggle("Solved", _solvedStatus);
        if (GUILayout.Button("Apply"))
        {
            (target as PuzzlePieceRenderer).SetSolvedState(_solvedStatus);
        }
    }
}
#endif