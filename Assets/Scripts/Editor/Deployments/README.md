# Deployments

WebGL build and AWS S3 deployment pipeline using Unity Build Profiles and AWS CLI.

## Components
- **DeploymentConfig**: ScriptableObject storing S3 bucket, region, cache policies, and compression settings
- **BuildAndDeploy**: Editor scripts for building WebGL and uploading to S3 via AWS CLI
- **S3Uploader**: Handles file uploads with proper MIME types, cache headers, and content encoding

## Setup
Requires AWS CLI installed and configured with appropriate credentials for S3 access.
