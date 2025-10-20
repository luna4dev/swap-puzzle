using SwapPuzzle.Interfaces;
using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    public class IllustrationTesterLevelData : ILevelData
    {
        public IIllustrationData Illustration { get; set; }
        public string Name { get; set; }
        public int GridSize { get; set; }
        public int PresolvedPieces { get; set; }

        public IllustrationTesterLevelData()
        {
            Name = "";
            GridSize = 7;
            PresolvedPieces = 10;
        }
    }
}