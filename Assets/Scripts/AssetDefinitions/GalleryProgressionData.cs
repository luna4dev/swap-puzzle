using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "New Gallery Progression", menuName = "SwapPuzzle/Gallery/Progression")]
    public class GalleryProgressionData : ScriptableObject, IGalleryProgressionData
    {
        public string Name { get => name; }
        [SerializeField] private bool _isTest;
        public bool IsTest { get => _isTest; }

        [SerializeField] private List<GalleryData> _galleries;
        public List<IGalleryData> Galleries
        {
            get
            {
                List<IGalleryData> list = new();
                if (_galleries == null) return list;
                foreach (GalleryData item in _galleries)
                {
                    list.Add(item);
                }
                return list;
            }
        }
    }
}