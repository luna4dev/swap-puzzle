using TMPro;
using UnityEngine;
using SwapPuzzle.AssetDefinitions;

namespace SwapPuzzle.MonoBehaviours
{
    public class VersionInfo : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] DeploymentConfig deploymentConfig;

        void OnEnable()
        {
            text.text = "";

            if (deploymentConfig)
            {
                SetVersionText(deploymentConfig.buildNumber);
            }
        }

        public void SetVersionText(int versionIndex)
        {
            text.text = $"v_{versionIndex}";
        }
    }
}