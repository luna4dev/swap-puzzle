#!/bin/bash

# S3 Deployment Script for Unity WebGL Builds
# Usage: ./deploy.sh [profile] [version] [bucket-name] [region]
# Example: ./deploy.sh production v1.0.0 my-game-bucket us-east-1

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
UNITY_PROJECT_PATH="$(pwd)"
BUILDS_PATH="$UNITY_PROJECT_PATH/Builds"

# Default values
PROFILE=${1:-"production"}
VERSION=${2:-"latest"}
BUCKET_NAME=${3:-""}
REGION=${4:-"us-east-1"}

# Validate inputs
if [[ ! "$PROFILE" =~ ^(development|staging|production)$ ]]; then
    echo -e "${RED}Error: Invalid profile. Use: development, staging, or production${NC}"
    exit 1
fi

if [ -z "$BUCKET_NAME" ]; then
    echo -e "${RED}Error: S3 bucket name is required${NC}"
    echo -e "${YELLOW}Usage: ./deploy.sh [profile] [version] [bucket-name] [region]${NC}"
    exit 1
fi

echo -e "${BLUE}=== S3 Deployment Script ===${NC}"
echo -e "${YELLOW}Profile: $PROFILE${NC}"
echo -e "${YELLOW}Version: $VERSION${NC}"
echo -e "${YELLOW}Bucket: $BUCKET_NAME${NC}"
echo -e "${YELLOW}Region: $REGION${NC}"

# Determine build path
if [ "$VERSION" = "latest" ]; then
    BUILD_PATH="$BUILDS_PATH/Version/Latest/$PROFILE"
else
    BUILD_PATH="$BUILDS_PATH/Version/v$VERSION/$PROFILE"
fi

# Check if build exists
if [ ! -d "$BUILD_PATH" ]; then
    echo -e "${RED}Error: Build not found at $BUILD_PATH${NC}"
    echo -e "${YELLOW}Please run the build script first: ./build.sh $PROFILE $VERSION${NC}"
    exit 1
fi

# Check if AWS CLI is installed
if ! command -v aws &> /dev/null; then
    echo -e "${RED}Error: AWS CLI not found. Please install AWS CLI.${NC}"
    echo -e "${YELLOW}Installation: https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html${NC}"
    exit 1
fi

# Check AWS credentials
if ! aws sts get-caller-identity &> /dev/null; then
    echo -e "${RED}Error: AWS credentials not configured. Please run 'aws configure'${NC}"
    exit 1
fi

# Check if bucket exists
if ! aws s3 ls "s3://$BUCKET_NAME" &> /dev/null; then
    echo -e "${RED}Error: S3 bucket '$BUCKET_NAME' not found or not accessible${NC}"
    exit 1
fi

echo -e "${BLUE}Build path: $BUILD_PATH${NC}"
echo -e "${BLUE}Deploying to: s3://$BUCKET_NAME${NC}"

# Create deployment directory structure
DEPLOY_PATH=""
case $PROFILE in
    "development")
        DEPLOY_PATH="dev"
        ;;
    "staging")
        DEPLOY_PATH="staging"
        ;;
    "production")
        DEPLOY_PATH=""
        ;;
esac

# Add version to path if not latest
if [ "$VERSION" != "latest" ]; then
    if [ -n "$DEPLOY_PATH" ]; then
        DEPLOY_PATH="$DEPLOY_PATH/v$VERSION"
    else
        DEPLOY_PATH="v$VERSION"
    fi
fi

# S3 destination
S3_DESTINATION="s3://$BUCKET_NAME"
if [ -n "$DEPLOY_PATH" ]; then
    S3_DESTINATION="$S3_DESTINATION/$DEPLOY_PATH"
fi

echo -e "${BLUE}Deployment path: $S3_DESTINATION${NC}"

# Create CloudFront invalidation function
invalidate_cloudfront() {
    local distribution_id="$1"
    if [ -n "$distribution_id" ]; then
        echo -e "${BLUE}Invalidating CloudFront cache...${NC}"
        aws cloudfront create-invalidation \
            --distribution-id "$distribution_id" \
            --paths "/*" \
            --region "$REGION"
        echo -e "${GREEN}CloudFront invalidation initiated${NC}"
    fi
}

# Upload files to S3
echo -e "${BLUE}Uploading files to S3...${NC}"

# Upload with proper content types and cache headers
aws s3 sync "$BUILD_PATH" "$S3_DESTINATION" \
    --delete \
    --cache-control "max-age=31536000" \
    --exclude "*.html" \
    --exclude "*.js" \
    --exclude "*.css" \
    --region "$REGION"

# Upload HTML files with no-cache
aws s3 sync "$BUILD_PATH" "$S3_DESTINATION" \
    --delete \
    --cache-control "no-cache" \
    --include "*.html" \
    --region "$REGION"

# Upload JS and CSS files with shorter cache
aws s3 sync "$BUILD_PATH" "$S3_DESTINATION" \
    --delete \
    --cache-control "max-age=3600" \
    --include "*.js" \
    --include "*.css" \
    --region "$REGION"

# Set proper content types
echo -e "${BLUE}Setting content types...${NC}"

# Set MIME types for Unity WebGL files
aws s3 cp "$S3_DESTINATION/Build/Build.data" "$S3_DESTINATION/Build/Build.data" \
    --content-type "application/octet-stream" \
    --cache-control "max-age=31536000" \
    --region "$REGION"

aws s3 cp "$S3_DESTINATION/Build/Build.framework.js" "$S3_DESTINATION/Build/Build.framework.js" \
    --content-type "application/javascript" \
    --cache-control "max-age=31536000" \
    --region "$REGION"

aws s3 cp "$S3_DESTINATION/Build/Build.loader.js" "$S3_DESTINATION/Build/Build.loader.js" \
    --content-type "application/javascript" \
    --cache-control "max-age=31536000" \
    --region "$REGION"

# Set index.html content type
if [ -f "$BUILD_PATH/index.html" ]; then
    aws s3 cp "$S3_DESTINATION/index.html" "$S3_DESTINATION/index.html" \
        --content-type "text/html" \
        --cache-control "no-cache" \
        --region "$REGION"
fi

# Configure bucket for static website hosting (optional)
echo -e "${BLUE}Configuring bucket for static website hosting...${NC}"
aws s3 website "$S3_DESTINATION" \
    --index-document index.html \
    --error-document index.html \
    --region "$REGION"

# Create deployment info
DEPLOY_INFO="$BUILD_PATH/deploy-info.json"
cat > "$DEPLOY_INFO" << EOF
{
    "deployment": {
        "profile": "$PROFILE",
        "version": "$VERSION",
        "bucket": "$BUCKET_NAME",
        "region": "$REGION",
        "deploy_path": "$DEPLOY_PATH",
        "s3_url": "$S3_DESTINATION",
        "deploy_date": "$(date -u +"%Y-%m-%dT%H:%M:%SZ")",
        "build_path": "$BUILD_PATH"
    },
    "urls": {
        "s3": "https://$BUCKET_NAME.s3.$REGION.amazonaws.com/$DEPLOY_PATH",
        "cloudfront": "https://$BUCKET_NAME.cloudfront.net/$DEPLOY_PATH"
    }
}
EOF

echo -e "${GREEN}Deployment completed successfully!${NC}"
echo -e "${BLUE}Deployment info saved to: $DEPLOY_INFO${NC}"
echo -e "${GREEN}Access your game at:${NC}"
echo -e "${YELLOW}S3: https://$BUCKET_NAME.s3.$REGION.amazonaws.com/$DEPLOY_PATH${NC}"
echo -e "${YELLOW}CloudFront: https://$BUCKET_NAME.cloudfront.net/$DEPLOY_PATH${NC}"

# Optional: Invalidate CloudFront if distribution ID is provided
if [ -n "$CLOUDFRONT_DISTRIBUTION_ID" ]; then
    invalidate_cloudfront "$CLOUDFRONT_DISTRIBUTION_ID"
fi

echo -e "${GREEN}Deployment script completed!${NC}" 