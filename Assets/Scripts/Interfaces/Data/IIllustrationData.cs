using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface IIllustrationData
    {
        int IllustrationId { get; }
        string IllustrationName { get; }
        Texture2D IllustrationImage { get; }
    }
}