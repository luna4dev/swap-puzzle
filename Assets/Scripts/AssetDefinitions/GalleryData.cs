using UnityEngine;
using SwapPuzzle.Interfaces;
using System.Collections.Generic;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "New Gallery", menuName = "SwapPuzzle/Gallery/Gallery")]
    public class GalleryData : ScriptableObject, IGalleryData
    {
        public string Name { get => name; }

        [SerializeField] private List<IllustrationData> _illustrations;
        public List<IIllustrationData> Illustrations
        {
            get
            {
                List<IIllustrationData> list = new();
                foreach (IllustrationData item in _illustrations)
                {
                    list.Add(item);
                }
                return list;
            }
        }
    }
}