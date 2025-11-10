#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;

namespace SwapPuzzle.Editor
{
    public static class S3Uploader
    {
        [MenuItem("Build/Upload to S3")]
        public static void UploadToS3()
        {
            UploadToS3(null);
        }

        public static void UploadToS3(DeploymentConfig config)
        {
            if (config == null)
            {
                UnityEngine.Debug.LogError("DeploymentConfig is required. Please provide a deployment configuration.");
                return;
            }

            // Find the latest build
            string buildArtifactsPath = Path.Combine(Application.dataPath, "..", "Builds", "Artifacts");
            string absolutePath = Path.GetFullPath(buildArtifactsPath);

            if (!Directory.Exists(absolutePath))
            {
                UnityEngine.Debug.LogError($"Build artifacts directory not found: {absolutePath}");
                return;
            }

            string latestBuildPath = GetLatestBuildDirectory(absolutePath);

            if (string.IsNullOrEmpty(latestBuildPath))
            {
                UnityEngine.Debug.LogError("No build found in Builds/Artifacts directory");
                return;
            }

            UnityEngine.Debug.Log($"Found latest build: {latestBuildPath}");
            UnityEngine.Debug.Log($"Using deployment config: {config.environmentName}");

            // Upload to S3
            UploadDirectoryToS3(latestBuildPath, config);
        }

        private static string GetLatestBuildDirectory(string artifactsPath)
        {
            var directories = Directory.GetDirectories(artifactsPath);

            if (directories.Length == 0)
                return null;

            // Sort by creation time (most recent first)
            Array.Sort(directories, (a, b) =>
                Directory.GetCreationTime(b).CompareTo(Directory.GetCreationTime(a)));

            return directories[0];
        }

        private static void UploadDirectoryToS3(string buildPath, DeploymentConfig config)
        {
            // Check if AWS CLI is installed
            if (!IsAwsCliInstalled())
            {
                UnityEngine.Debug.LogError("AWS CLI is not installed or not in PATH. Please install AWS CLI first.");
                return;
            }

            string buildFolderName = Path.GetFileName(buildPath);
            string s3Destination = $"s3://{config.s3Bucket}/{config.s3Path}/{buildFolderName}";

            UnityEngine.Debug.Log($"Uploading {buildPath} to {s3Destination}...");

            // Build AWS S3 sync command with appropriate headers
            string arguments = $"s3 sync \"{buildPath}\" \"{s3Destination}\" " +
                             $"--profile {config.awsProfile} " +
                             SetCacheHeaders(config) +
                             SetContentEncodingHeaders(config) +
                             (config.publicRead ? "--acl public-read " : "") +
                             (config.deleteRemovedFiles ? "--delete" : "");

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = GetAwsCliPath(),
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        UnityEngine.Debug.Log("Upload completed successfully!");
                        UnityEngine.Debug.Log($"S3 Location: {s3Destination}");
                        if (!string.IsNullOrEmpty(output))
                        {
                            UnityEngine.Debug.Log($"Output: {output}");
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Upload failed with exit code {process.ExitCode}");
                        UnityEngine.Debug.LogError($"Error: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to execute AWS CLI command: {ex.Message}");
            }
        }

        private static string SetCacheHeaders(DeploymentConfig config)
        {
            // Different cache policies for different file types
            return "--metadata-directive REPLACE " +
                   $"--cache-control \"public, max-age={config.staticAssetsCacheMaxAge}\" " +
                   "--exclude \"*.html\" " +
                   $"--cache-control \"{config.htmlCacheControl}\" " +
                   "--include \"*.html\" ";
        }

        private static string SetContentEncodingHeaders(DeploymentConfig config)
        {
            // Set content-encoding for gzipped files based on compression format
            if (config.compressionFormat == WebGLCompressionFormat.Gzip)
            {
                return "--content-encoding gzip " +
                       "--exclude \"*\" " +
                       "--include \"*.js.gz\" " +
                       "--include \"*.wasm.gz\" " +
                       "--include \"*.data.gz\" " +
                       "--content-type \"application/octet-stream\" ";
            }
            else if (config.compressionFormat == WebGLCompressionFormat.Brotli)
            {
                return "--content-encoding br " +
                       "--exclude \"*\" " +
                       "--include \"*.js.br\" " +
                       "--include \"*.wasm.br\" " +
                       "--include \"*.data.br\" " +
                       "--content-type \"application/octet-stream\" ";
            }

            return "";
        }

        private static string GetAwsCliPath()
        {
            // Common AWS CLI installation paths on macOS
            string[] possiblePaths = new[]
            {
                "/usr/local/bin/aws",
                "/opt/homebrew/bin/aws",
                "/usr/bin/aws"
            };

            foreach (string path in possiblePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    return path;
                }
            }

            // Fallback to PATH
            return "aws";
        }

        private static bool IsAwsCliInstalled()
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = GetAwsCliPath(),
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
#endif
