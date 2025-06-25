using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviors;

[CustomEditor(typeof(GridItem))]
public class GridItemEditor : Editor {

    private int _x = 0;
    private int _y = 0;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

        // get x, y from the inspector input fields
        EditorGUILayout.LabelField("Set Position", EditorStyles.boldLabel);
        _x = EditorGUILayout.IntField("X", _x);
        _y = EditorGUILayout.IntField("Y", _y);
        if(GUILayout.Button("Execute")) {
            (target as GridItem).SetPosition(_x, _y);
        }
    }
}