using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "New Level Progression", menuName = "SwapPuzzle/Level/Progression")]
    public class LevelProgressionData : ScriptableObject, ILevelProgressionData
    {
        public string Name { get => name; }

        [SerializeField] private bool _isTest;
        public bool IsTest { get => _isTest; }
        [SerializeField] private List<LevelData> _levels;
        public List<ILevelData> Levels
        {
            get
            {
                List<ILevelData> list = new();
                foreach (LevelData item in _levels)
                {
                    list.Add(item);
                }
                return list;
            }
        }
    }
}