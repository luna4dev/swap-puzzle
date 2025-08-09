#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzleGrid))]
public class PuzzleGridEditor : Editor {

    private int _pieceCountPerRow = 4;
    private int _pieceCountPerRowForInitialize = 4;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("(Play Mode) Initialize Grid", EditorStyles.boldLabel);
        _pieceCountPerRowForInitialize = EditorGUILayout.IntField("Count", _pieceCountPerRowForInitialize);
        if(GUILayout.Button("Execute")) {
            (target as PuzzleGrid).InitializeGrid(_pieceCountPerRowForInitialize);
        }
    }
}
#endif