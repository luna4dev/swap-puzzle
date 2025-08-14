using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "SwapPuzzle/LevelData")]
    public class LevelData : ScriptableObject, ILevelData
    {
        [SerializeField] private int _levelId;
        public int LevelId { get => _levelId; set => _levelId = value; }

        [SerializeField] private Sprite _illustration;
        public Sprite Illustration { get => _illustration; set => _illustration = value; }

        [SerializeField] private int _gridSize;
        public int GridSize { get => _gridSize; set => _gridSize = value; }

        [SerializeField] private int _presolvedPieces;
        public int PresolvedPieces { get => _presolvedPieces; set => _presolvedPieces = value; }

        [SerializeField] private bool _isVariation;
        public bool IsVariation { get => _isVariation; set => _isVariation = value; }

        [SerializeField] private int _baseLevel;
        public int BaseLevel { get => _baseLevel; set => _baseLevel = value; }

        public string LevelName { get => name; }
    }
}