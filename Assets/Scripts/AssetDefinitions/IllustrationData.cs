using UnityEngine;
using SwapPuzzle.Interfaces;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "New Level Relation", menuName = "SwapPuzzle/Illustration")]
    public class IllustrationData : ScriptableObject, IIllustrationData
    {
        public string Name { get => name; }

        [SerializeField] private Sprite _illustration;
        public Sprite Illustration { get => _illustration; }
    }
}