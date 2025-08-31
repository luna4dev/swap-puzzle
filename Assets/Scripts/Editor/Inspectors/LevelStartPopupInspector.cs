#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(LevelStartPopup))]
public class LevelStartPopupInspector : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // space
        EditorGUILayout.Space(10);

        // test button
        if (GUILayout.Button("Play"))
        {
            (target as LevelStartPopup).PlayLevelStartPopup("Test");
        }
    }
}
#endif