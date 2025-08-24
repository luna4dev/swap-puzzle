using UnityEngine;
using SwapPuzzle.Interfaces;
using UnityEditor;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "New Level", menuName = "SwapPuzzle/Level/Level")]
    public class LevelData : ScriptableObject, ILevelData
    {
        public string Name { get => name; }

        [SerializeField] private IllustrationData _illustration;
        public IIllustrationData Illustration { get => _illustration; }

        [SerializeField] private int _gridSize;
        public int GridSize { get => _gridSize; }

        [SerializeField] private int _presolvedPieces;
        public int PresolvedPieces { get => _presolvedPieces; }
    }
}