#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor {

    private int _pieceCountPerRow = 4;
    private int _pieceCountPerRowForInitialize = 4;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

        // get grid size from the inspector input fields
        EditorGUILayout.LabelField("Set Piece Count Per Row", EditorStyles.boldLabel);
        _pieceCountPerRow = EditorGUILayout.IntField("Count", _pieceCountPerRow);
        if(GUILayout.Button("Execute")) {
            (target as GridSystem).SetPieceCountPerRow(_pieceCountPerRow);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("(Play Mode) Initialize Grid", EditorStyles.boldLabel);
        _pieceCountPerRowForInitialize = EditorGUILayout.IntField("Count", _pieceCountPerRowForInitialize);
        if(GUILayout.Button("Execute")) {
            (target as GridSystem).InitializeGrid(_pieceCountPerRowForInitialize);
        }
    }
}
#endif