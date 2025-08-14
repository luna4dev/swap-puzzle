#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace SwapPuzzle.Editor
{
    public static class WebGLBuildCommand
    {
        [MenuItem("Build/Build WebGL")]
        public static void BuildWebGL()
        {
            // Check WebGL platform support
            if (!IsWebGLSupported())
            {
                Debug.LogError("WebGL platform is not supported or not installed.");
                return;
            }

            // Generate build ID
            DateTime now = DateTime.Now;
            int secondsInDay = (int)now.TimeOfDay.TotalSeconds;
            string buildId = $"webgl_{now:yyMMdd}_{secondsInDay}";
            
            // Create build path
            string buildPath = Path.Combine("Builds", "Artifacts", buildId);
            string absoluteBuildPath = Path.GetFullPath(buildPath);
            
            Debug.Log($"Starting WebGL build: {buildId}");
            Debug.Log($"Build path: {absoluteBuildPath}");

            // Ensure directory exists
            try
            {
                Directory.CreateDirectory(absoluteBuildPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create build directory: {ex.Message}");
                return;
            }

            // Configure build settings
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = absoluteBuildPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            // Set WebGL specific settings
            ConfigureWebGLSettings();

            // Execute build
            try
            {
                Debug.Log("Building WebGL...");
                var report = BuildPipeline.BuildPlayer(buildOptions);

                // Check build result
                if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    Debug.Log($"WebGL build completed successfully!");
                    Debug.Log($"Build location: {absoluteBuildPath}");
                    Debug.Log($"Build size: {GetBuildSizeString(report.summary.totalSize)}");
                }
                else
                {
                    Debug.LogError($"WebGL build failed with result: {report.summary.result}");
                    if (report.summary.totalErrors > 0)
                    {
                        Debug.LogError($"Build had {report.summary.totalErrors} error(s)");
                    }
                    
                    // Cleanup failed build
                    CleanupFailedBuild(absoluteBuildPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Build process failed: {ex.Message}");
                
                // Cleanup failed build
                CleanupFailedBuild(absoluteBuildPath);
            }
        }

        private static bool IsWebGLSupported()
        {
            // Check if WebGL platform is available
            return BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        }

        private static string[] GetScenePaths()
        {
            // Get all enabled scenes from build settings
            var scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            return scenes;
        }

        private static void ConfigureWebGLSettings()
        {
            // Set WebGL compression to Gzip
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
            
            // Additional WebGL settings can be configured here
            // For example:
            // PlayerSettings.WebGL.template = "PROJECT:Minimal";
            // PlayerSettings.WebGL.memorySize = 512;
            
            Debug.Log("WebGL settings configured: Compression = Gzip");
        }

        private static void CleanupFailedBuild(string buildPath)
        {
            try
            {
                if (Directory.Exists(buildPath))
                {
                    Debug.Log($"Cleaning up failed build at: {buildPath}");
                    Directory.Delete(buildPath, true);
                    Debug.Log("Failed build cleanup completed successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to cleanup build directory: {ex.Message}");
            }
        }

        private static string GetBuildSizeString(ulong sizeInBytes)
        {
            if (sizeInBytes < 1024)
                return $"{sizeInBytes} B";
            else if (sizeInBytes < 1024 * 1024)
                return $"{sizeInBytes / 1024.0:F1} KB";
            else if (sizeInBytes < 1024 * 1024 * 1024)
                return $"{sizeInBytes / (1024.0 * 1024.0):F1} MB";
            else
                return $"{sizeInBytes / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }
    }
}
#endif