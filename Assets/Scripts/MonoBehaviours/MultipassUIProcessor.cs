using UnityEngine;
using UnityEngine.UI;

namespace SwapPuzzle.MonoBehaviours
{
    public class MultipassUIRenderer : MonoBehaviour
    {
        [Header("Materials")]
        public Material analysisMaterial;    // First pass: calculates data
        public Material processingMaterial;  // Second pass: uses the data

        [Header("Textures")]
        public Texture sourceTexture;
        public RenderTexture analysisTexture; // Stores first pass results

        private Image displayImage;

        void Start()
        {
            displayImage = GetComponent<Image>();
            ProcessTwoPass();
        }

        void ProcessTwoPass()
        {
            // Pass 1: Analysis material calculates data across entire UV space
            Graphics.Blit(sourceTexture, analysisTexture, analysisMaterial);

            // Pass 2: Processing material uses analysis results + original texture
            processingMaterial.SetTexture("_AnalysisData", analysisTexture);
            processingMaterial.SetTexture("_MainTex", sourceTexture);

            // Apply final material to Image component
            displayImage.material = analysisMaterial;
        }
    }
}