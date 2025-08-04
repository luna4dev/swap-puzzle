#!/bin/bash

# Deployment Configuration
# Copy this file to config.local.sh and modify the values

# AWS Configuration
export AWS_DEFAULT_REGION="us-east-1"
export AWS_PROFILE="default"  # Change to your AWS profile if needed

# S3 Bucket Configuration
export S3_BUCKET_NAME="your-game-bucket-name"
export S3_REGION="us-east-1"

# CloudFront Configuration (optional)
export CLOUDFRONT_DISTRIBUTION_ID=""
export CLOUDFRONT_DOMAIN=""

# Deployment URLs
export PRODUCTION_URL="https://your-game-bucket-name.s3.us-east-1.amazonaws.com"
export STAGING_URL="https://your-game-bucket-name.s3.us-east-1.amazonaws.com/staging"
export DEVELOPMENT_URL="https://your-game-bucket-name.s3.us-east-1.amazonaws.com/dev"

# Build Configuration
export UNITY_PROJECT_PATH="$(pwd)"
export BUILDS_PATH="$UNITY_PROJECT_PATH/Builds"

# Version Management
export VERSION_PREFIX="v"
export VERSION_FORMAT="%Y%m%d_%H%M%S"  # Default timestamp format

# Deployment Paths
export PRODUCTION_DEPLOY_PATH=""
export STAGING_DEPLOY_PATH="staging"
export DEVELOPMENT_DEPLOY_PATH="dev"

# Cache Settings
export HTML_CACHE_CONTROL="no-cache"
export JS_CACHE_CONTROL="max-age=3600"
export CSS_CACHE_CONTROL="max-age=3600"
export ASSET_CACHE_CONTROL="max-age=31536000"

# Notification Settings (optional)
export SLACK_WEBHOOK_URL=""
export DISCORD_WEBHOOK_URL=""

# Build Quality Settings
export DEVELOPMENT_BUILD_OPTIONS="--development"
export STAGING_BUILD_OPTIONS="--staging"
export PRODUCTION_BUILD_OPTIONS="--production"

# Unity Build Settings
export UNITY_BUILD_TARGET="WebGL"
export UNITY_BUILD_METHOD_PREFIX="BuildScripts.BuildWebGL"

# Logging
export LOG_LEVEL="INFO"  # DEBUG, INFO, WARN, ERROR
export LOG_FILE="$BUILDS_PATH/deploy.log"

# Security
export ENABLE_HTTPS_REDIRECT="true"
export ENABLE_SECURITY_HEADERS="true"

# Performance
export ENABLE_GZIP_COMPRESSION="true"
export ENABLE_BROTLI_COMPRESSION="false"

# Monitoring
export ENABLE_ANALYTICS="false"
export ANALYTICS_ID=""

# Backup Settings
export ENABLE_BACKUP="true"
export BACKUP_RETENTION_DAYS="30"

# Load local configuration if it exists
if [ -f "$(dirname "$0")/config.local.sh" ]; then
    source "$(dirname "$0")/config.local.sh"
fi 