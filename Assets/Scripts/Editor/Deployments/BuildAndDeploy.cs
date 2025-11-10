#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SwapPuzzle.Editor
{
    public class BuildAndDeploy : EditorWindow
    {
        private DeploymentConfig selectedConfig;

        [MenuItem("Build/Build and Deploy")]
        public static void ShowWindow()
        {
            var window = GetWindow<BuildAndDeploy>("Build & Deploy");
            window.minSize = new Vector2(400, 200);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Build and Deploy", EditorStyles.boldLabel);
            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "Select a deployment configuration and choose an action.\n" +
                "You can create deployment configs via: Assets > Create > SwapPuzzle > Deployment Config",
                MessageType.Info);

            GUILayout.Space(10);

            // Config selection
            selectedConfig = (DeploymentConfig)EditorGUILayout.ObjectField(
                "Deployment Config",
                selectedConfig,
                typeof(DeploymentConfig),
                false);

            if (selectedConfig != null)
            {
                EditorGUILayout.LabelField("Environment", selectedConfig.environmentName);
                EditorGUILayout.LabelField("S3 Bucket", selectedConfig.s3Bucket);
                EditorGUILayout.LabelField("S3 Path", selectedConfig.s3Path);
                EditorGUILayout.LabelField("Compression", selectedConfig.compressionFormat.ToString());
            }

            GUILayout.Space(20);

            EditorGUI.BeginDisabledGroup(selectedConfig == null);

            if (GUILayout.Button("Build Only", GUILayout.Height(30)))
            {
                WebGLBuildCommand.BuildWebGL(selectedConfig);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Upload Only", GUILayout.Height(30)))
            {
                S3Uploader.UploadToS3(selectedConfig);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Build and Deploy", GUILayout.Height(40)))
            {
                BuildAndDeployToS3(selectedConfig);
            }

            EditorGUI.EndDisabledGroup();

            if (selectedConfig == null)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Please select a deployment configuration to continue.", MessageType.Warning);
            }
        }

        private void BuildAndDeployToS3(DeploymentConfig config)
        {
            Debug.Log($"Starting build and deploy process for: {config.environmentName}");

            // Build
            WebGLBuildCommand.BuildWebGL(config);

            // Upload
            S3Uploader.UploadToS3(config);

            Debug.Log("Build and deploy process completed!");
        }
    }
}
#endif
