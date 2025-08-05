using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;

public class BuildScripts
{
    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        BuildWebGL("Production", BuildOptions.None);
    }

    private static void BuildWebGL(string profile, BuildOptions options)
    {
        // Get the project path
        string projectPath = Directory.GetCurrentDirectory();
        string buildPath = Path.Combine(projectPath, "Builds", "Version", "Latest", profile);
        
        // Ensure the build directory exists
        Directory.CreateDirectory(buildPath);
        
        // Get enabled scenes
        string[] scenes = GetEnabledScenes();
        
        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes enabled for build. Please enable scenes in Build Settings.");
            return;
        }
        
        Debug.Log($"Building WebGL {profile} to: {buildPath}");
        Debug.Log($"Scenes to build: {string.Join(", ", scenes)}");
        
        // Configure build player options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = options;
        
        // Set scripting define symbols based on profile
        string defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL);
        string newDefines = defines;
        
        // Remove existing profile defines
        newDefines = RemoveDefine(newDefines, "DEVELOPMENT_BUILD");
        newDefines = RemoveDefine(newDefines, "STAGING_BUILD");
        newDefines = RemoveDefine(newDefines, "PRODUCTION_BUILD");
        
        // only production build
        newDefines = AddDefine(newDefines, "PRODUCTION_BUILD");
        
        // Add WebGL define
        newDefines = AddDefine(newDefines, "UNITY_WEBGL");
        
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, newDefines);
        
        // Configure WebGL settings based on profile
        ConfigureWebGLSettings(profile);
        
        // Build the project
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildResult result = report.summary.result;
        
        if (result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {buildPath}");
            Debug.Log($"Build size: {report.summary.totalSize / 1024 / 1024} MB");
            
            // Create build info file
            CreateBuildInfo(buildPath, profile, report);
        }
        else
        {
            Debug.LogError($"Build failed: {report.summary.result}");
            Debug.LogError($"Error: {report.summary.totalErrors} errors, {report.summary.totalWarnings} warnings");
        }
    }
    
    private static string[] GetEnabledScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }
    
    private static string AddDefine(string defines, string newDefine)
    {
        if (string.IsNullOrEmpty(defines))
            return newDefine;
        
        if (defines.Contains(newDefine))
            return defines;
        
        return defines + ";" + newDefine;
    }
    
    private static string RemoveDefine(string defines, string defineToRemove)
    {
        if (string.IsNullOrEmpty(defines))
            return defines;
        
        string[] defineArray = defines.Split(';');
        defineArray = defineArray.Where(d => d != defineToRemove).ToArray();
        return string.Join(";", defineArray);
    }
    
    private static void ConfigureWebGLSettings(string profile)
    {
        // Set WebGL memory size based on profile
        switch (profile.ToLower())
        {
            case "production":
                PlayerSettings.WebGL.memorySize = 256;
                PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
                break;
        }
        
        // Configure other WebGL settings
        // PlayerSettings.WebGL.threadsSupport = ThreadsSupport.None;
        // PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        // PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
        
        // Set quality settings based on profile
        switch (profile.ToLower())
        {
            case "production":
                QualitySettings.SetQualityLevel(2); // Good
                break;
        }
    }
    
    private static void CreateBuildInfo(string buildPath, string profile, BuildReport report)
    {
        string buildInfoPath = Path.Combine(buildPath, "build-info.json");
        
        var buildInfo = new
        {
            profile = profile,
            buildDate = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            unityVersion = Application.unityVersion,
            buildTarget = "WebGL",
            totalSize = report.summary.totalSize,
            totalErrors = report.summary.totalErrors,
            totalWarnings = report.summary.totalWarnings,
            scenes = GetEnabledScenes()
        };
        
        string json = JsonUtility.ToJson(buildInfo, true);
        File.WriteAllText(buildInfoPath, json);
        
        Debug.Log($"Build info saved to: {buildInfoPath}");
    }
    
    [MenuItem("Build/List Enabled Scenes")]
    public static void ListEnabledScenes()
    {
        string[] scenes = GetEnabledScenes();
        Debug.Log($"Enabled scenes ({scenes.Length}):");
        foreach (string scene in scenes)
        {
            Debug.Log($"  - {scene}");
        }
    }
    
    [MenuItem("Build/Open Builds Folder")]
    public static void OpenBuildsFolder()
    {
        string buildsPath = Path.Combine(Directory.GetCurrentDirectory(), "Builds");
        if (Directory.Exists(buildsPath))
        {
            EditorUtility.RevealInFinder(buildsPath);
        }
        else
        {
            Debug.LogWarning("Builds folder does not exist yet. Run a build first.");
        }
    }
} 