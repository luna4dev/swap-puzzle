using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviors;

[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor {

    private int _gridSize = 100;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

        // get grid size from the inspector input fields
        EditorGUILayout.LabelField("Set Grid Size", EditorStyles.boldLabel);
        _gridSize = EditorGUILayout.IntField("Grid Size", _gridSize);
        if(GUILayout.Button("Execute")) {
            (target as GridSystem).SetGridSize(_gridSize);
        }
    }
}