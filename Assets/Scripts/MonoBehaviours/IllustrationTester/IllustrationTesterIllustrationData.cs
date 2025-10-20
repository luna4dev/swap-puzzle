using SwapPuzzle.Interfaces;
using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    public class IllustrationTesterIllustrationData : IIllustrationData
    {
        public string Name { get; set; }
        public Sprite Illustration { get; set; }
        
        public IllustrationTesterIllustrationData (Sprite sprite)
        {
            Name = "";
            Illustration = sprite;
        }
    }
}