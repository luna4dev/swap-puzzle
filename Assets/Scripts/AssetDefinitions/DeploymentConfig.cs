#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SwapPuzzle.AssetDefinitions
{
    [CreateAssetMenu(fileName = "DeploymentConfig", menuName = "SwapPuzzle/Deployment Config", order = 1)]
    public class DeploymentConfig : ScriptableObject
    {
        [Header("Environment")]
        [Tooltip("Environment name (e.g., Production, Staging, Development)")]
        public string environmentName = "Production";

        [Header("AWS S3 Settings")]
        [Tooltip("S3 bucket name (e.g., my-webgl-builds)")]
        public string s3Bucket = "your-bucket-name";

        [Tooltip("S3 path prefix (e.g., production/webgl)")]
        public string s3Path = "your/path/prefix";

        [Tooltip("AWS CLI profile name")]
        public string awsProfile = "default";

        [Header("Build Settings")]
        [Tooltip("WebGL compression format")]
        public WebGLCompressionFormat compressionFormat = WebGLCompressionFormat.Gzip;

        [Header("Cache Settings")]
        [Tooltip("Cache duration for static assets in seconds (default: 1 year)")]
        public int staticAssetsCacheMaxAge = 31536000;

        [Tooltip("Cache control for HTML files")]
        public string htmlCacheControl = "no-cache";

        [Header("Upload Options")]
        [Tooltip("Make uploaded files publicly readable")]
        public bool publicRead = true;

        [Tooltip("Delete files from S3 that don't exist locally")]
        public bool deleteRemovedFiles = false;

        [Header("Build Version")]
        [Tooltip("Current build number (auto-increments on each build)")]
        public int buildNumber = 0;

        public void IncrementBuildNumber()
        {
            buildNumber++;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
