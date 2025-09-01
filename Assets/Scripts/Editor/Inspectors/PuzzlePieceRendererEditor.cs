#if UNITY_EDITOR
using UnityEditor;

using SwapPuzzle.MonoBehaviours;

[CustomEditor(typeof(PuzzlePieceRenderer))]
public class PuzzlePieceRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif