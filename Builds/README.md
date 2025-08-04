# Unity WebGL Build & Deployment System

This directory contains scripts and configuration for building and deploying Unity WebGL projects to AWS S3.

## Directory Structure

```
Builds/
├── Version/
│   ├── v1.0.0/
│   │   ├── Development/
│   │   ├── Staging/
│   │   └── Production/
│   └── Latest/ -> v1.0.0/
├── Archive/
├── BuildScripts/
│   ├── build.sh
│   ├── deploy.sh
│   ├── deploy-helper.sh
│   ├── config.sh
│   └── config.local.sh (create this)
└── README.md
```

## Quick Start

### 1. Setup Configuration

Copy the configuration template and modify it:

```bash
cp Builds/BuildScripts/config.sh Builds/BuildScripts/config.local.sh
```

Edit `config.local.sh` with your AWS settings:

```bash
export S3_BUCKET_NAME="your-game-bucket-name"
export S3_REGION="us-east-1"
export CLOUDFRONT_DISTRIBUTION_ID="your-cloudfront-id"  # Optional
```

### 2. Build and Deploy

```bash
# Build only
./Builds/BuildScripts/build.sh production v1.0.0

# Deploy only
./Builds/BuildScripts/deploy.sh production v1.0.0 your-bucket-name

# Build and deploy in one command
./Builds/BuildScripts/deploy-helper.sh build-deploy production v1.0.0
```

## Build Profiles

### Development
- Debug symbols enabled
- Development server settings
- Hot reload support
- Deployed to `/dev/` path

### Staging
- Optimized but with debug info
- Testing environment settings
- Deployed to `/staging/` path

### Production
- Fully optimized
- Minified assets
- CDN-ready
- Deployed to root path

## Scripts Overview

### build.sh
Main build script for Unity WebGL projects.

**Usage:**
```bash
./build.sh [profile] [version]
```

**Examples:**
```bash
./build.sh development
./build.sh production v1.0.0
./build.sh staging 20241225_143022
```

### deploy.sh
Deploys builds to AWS S3 with proper configuration.

**Usage:**
```bash
./deploy.sh [profile] [version] [bucket-name] [region]
```

**Examples:**
```bash
./deploy.sh production latest my-game-bucket us-east-1
./deploy.sh staging v1.0.0 my-game-bucket us-west-2
```

### deploy-helper.sh
Helper script for common deployment operations.

**Usage:**
```bash
./deploy-helper.sh [command] [options]
```

**Commands:**
- `build [profile] [version]` - Build the project
- `deploy [profile] [version]` - Deploy to S3
- `build-deploy [profile] [version]` - Build and deploy
- `list-builds` - List available builds
- `list-deployments` - List deployed versions
- `rollback [version]` - Rollback to specific version
- `cleanup [days]` - Clean up old builds
- `status` - Show deployment status
- `config` - Show current configuration

## Configuration

### AWS Setup

1. Install AWS CLI:
   ```bash
   # macOS
   brew install awscli
   
   # Ubuntu
   sudo apt-get install awscli
   ```

2. Configure AWS credentials:
   ```bash
   aws configure
   ```

3. Create S3 bucket:
   ```bash
   aws s3 mb s3://your-game-bucket-name
   ```

4. Configure bucket for static website hosting:
   ```bash
   aws s3 website s3://your-game-bucket-name \
     --index-document index.html \
     --error-document index.html
   ```

### Unity Setup

1. Ensure Unity is installed and accessible
2. Set up build methods in Unity (see Unity Build Scripts section)
3. Configure WebGL build settings in Unity

## Unity Build Scripts

You'll need to create build methods in Unity. Create a new script in your Unity project:

```csharp
using UnityEditor;
using UnityEngine;

public class BuildScripts
{
    [MenuItem("Build/Build WebGL Development")]
    public static void BuildWebGLDevelopment()
    {
        BuildWebGL("Development", BuildOptions.Development);
    }

    [MenuItem("Build/Build WebGL Staging")]
    public static void BuildWebGLStaging()
    {
        BuildWebGL("Staging", BuildOptions.None);
    }

    [MenuItem("Build/Build WebGL Production")]
    public static void BuildWebGLProduction()
    {
        BuildWebGL("Production", BuildOptions.None);
    }

    private static void BuildWebGL(string profile, BuildOptions options)
    {
        string buildPath = $"Builds/Version/Latest/{profile}";
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetEnabledScenes();
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = options;
        
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildResult result = report.summary.result;
        
        if (result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {buildPath}");
        }
        else
        {
            Debug.LogError($"Build failed: {report.summary.result}");
        }
    }

    private static string[] GetEnabledScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }
}
```

## Deployment URLs

After deployment, your game will be available at:

- **Production**: `https://your-bucket-name.s3.region.amazonaws.com/`
- **Staging**: `https://your-bucket-name.s3.region.amazonaws.com/staging/`
- **Development**: `https://your-bucket-name.s3.region.amazonaws.com/dev/`

## Version Management

### Version Format
- Default: `YYYYMMDD_HHMMSS` (e.g., `20241225_143022`)
- Custom: Any string (e.g., `v1.0.0`, `release-2024`)

### Version Paths
- Latest: Always points to the most recent build
- Versioned: Specific version builds for rollback

## Best Practices

### 1. Version Control
- Always use semantic versioning for releases
- Tag releases in Git
- Keep build artifacts separate from source code

### 2. Testing
- Test builds locally before deployment
- Use staging environment for testing
- Validate WebGL compatibility across browsers

### 3. Performance
- Optimize assets for web delivery
- Use appropriate cache headers
- Consider CDN for global distribution

### 4. Security
- Enable HTTPS redirects
- Set proper security headers
- Validate file uploads (if applicable)

### 5. Monitoring
- Set up CloudWatch monitoring
- Configure error tracking
- Monitor build and deployment logs

## Troubleshooting

### Common Issues

1. **Unity not found**
   - Ensure Unity is installed and in PATH
   - Check Unity Hub installation

2. **AWS credentials not configured**
   - Run `aws configure`
   - Check IAM permissions

3. **S3 bucket not accessible**
   - Verify bucket exists
   - Check bucket permissions
   - Ensure region is correct

4. **Build fails**
   - Check Unity console for errors
   - Verify build scenes are enabled
   - Check WebGL build settings

### Logs
- Build logs: `Builds/build.log`
- Deployment logs: `Builds/deploy.log`

## Advanced Configuration

### CloudFront Setup
1. Create CloudFront distribution
2. Set S3 bucket as origin
3. Configure cache behaviors
4. Update `CLOUDFRONT_DISTRIBUTION_ID` in config

### Custom Domains
1. Configure Route 53
2. Set up SSL certificate
3. Update CloudFront distribution
4. Configure custom domain in config

### CI/CD Integration
The scripts can be integrated with:
- GitHub Actions
- Jenkins
- GitLab CI
- AWS CodePipeline

## Support

For issues or questions:
1. Check the logs in `Builds/` directory
2. Verify configuration in `config.local.sh`
3. Test with development profile first
4. Check AWS CLI and Unity installation 