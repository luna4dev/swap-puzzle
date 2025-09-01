#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzleController))]
public class PuzzleControllerInspector : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying && GUILayout.Button("Toggle Debug Mode"))
        {
            PuzzleController controller = (PuzzleController)target;
            controller.ToggleDebugMode();
        }
    }
}
#endif