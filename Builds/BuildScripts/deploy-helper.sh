#!/bin/bash

# Deployment Helper Script
# Provides common deployment operations

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Load configuration
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/config.sh"

# Functions
show_help() {
    echo -e "${BLUE}=== Deployment Helper ===${NC}"
    echo "Usage: $0 [command] [options]"
    echo ""
    echo "Commands:"
    echo "  build [version]     - Build the project"
    echo "  deploy [version]    - Deploy to S3"
    echo "  build-deploy [version] - Build and deploy"
    echo "  list-builds                   - List available builds"
    echo "  list-deployments              - List deployed versions"
    echo "  rollback [version]            - Rollback to specific version"
    echo "  cleanup [days]                - Clean up old builds"
    echo "  status                        - Show deployment status"
    echo "  config                        - Show current configuration"
    echo "  help                          - Show this help"
    echo ""
    echo "Version: optional, defaults to timestamp"
}

build_project() {
    local version=${1:-$(date +"$VERSION_FORMAT")}
    
    echo -e "${BLUE}Building project...${NC}"
    "$SCRIPT_DIR/build.sh" "$version"
}

deploy_project() {
    local version=${1:-"latest"}
    
    echo -e "${BLUE}Deploying project...${NC}"
    "$SCRIPT_DIR/deploy.sh" "$version" "$S3_BUCKET_NAME" "$S3_REGION"
}

build_and_deploy() {
    local version=${1:-$(date +"$VERSION_FORMAT")}
    
    echo -e "${BLUE}Building and deploying...${NC}"
    build_project "$version"
    deploy_project "$version"
}

list_builds() {
    echo -e "${BLUE}Available builds:${NC}"
    if [ -d "$BUILDS_PATH/Version" ]; then
        find "$BUILDS_PATH/Version" -maxdepth 2 -type d -name "v*" | sort
    else
        echo -e "${YELLOW}No builds found${NC}"
    fi
}

list_deployments() {
    echo -e "${BLUE}Checking S3 deployments...${NC}"
    if command -v aws &> /dev/null; then
        aws s3 ls "s3://$S3_BUCKET_NAME/" --recursive --human-readable --summarize
    else
        echo -e "${RED}AWS CLI not found${NC}"
    fi
}

rollback() {
    local version=$1
    if [ -z "$version" ]; then
        echo -e "${RED}Error: Version required for rollback${NC}"
        exit 1
    fi
    
    echo -e "${BLUE}Rolling back to version: $version${NC}"
    deploy_project "production" "$version"
}

cleanup() {
    local days=${1:-30}
    echo -e "${BLUE}Cleaning up builds older than $days days...${NC}"
    
    # Clean up local builds
    find "$BUILDS_PATH/Version" -maxdepth 1 -type d -name "v*" -mtime +$days -exec rm -rf {} \;
    
    # Clean up S3 (optional)
    if [ "$ENABLE_BACKUP" = "true" ]; then
        echo -e "${YELLOW}Note: S3 cleanup not implemented for safety${NC}"
    fi
}

show_status() {
    echo -e "${BLUE}=== Deployment Status ===${NC}"
    echo -e "${YELLOW}Current Configuration:${NC}"
    echo "  Bucket: $S3_BUCKET_NAME"
    echo "  Region: $S3_REGION"
    echo "  Latest Build: $(ls -t "$BUILDS_PATH/Version" 2>/dev/null | head -n1 || echo "None")"
    
    if command -v aws &> /dev/null; then
        echo -e "${YELLOW}S3 Status:${NC}"
        aws s3 ls "s3://$S3_BUCKET_NAME/" --recursive --summarize 2>/dev/null || echo "  Bucket not accessible"
    fi
}

show_config() {
    echo -e "${BLUE}=== Current Configuration ===${NC}"
    echo "AWS_DEFAULT_REGION: $AWS_DEFAULT_REGION"
    echo "S3_BUCKET_NAME: $S3_BUCKET_NAME"
    echo "S3_REGION: $S3_REGION"
    echo "CLOUDFRONT_DISTRIBUTION_ID: $CLOUDFRONT_DISTRIBUTION_ID"
    echo "BUILDS_PATH: $BUILDS_PATH"
    echo "VERSION_FORMAT: $VERSION_FORMAT"
}

# Main script
case "${1:-help}" in
    "build")
        build_project "$2" "$3"
        ;;
    "deploy")
        deploy_project "$2" "$3"
        ;;
    "build-deploy")
        build_and_deploy "$2" "$3"
        ;;
    "list-builds")
        list_builds
        ;;
    "list-deployments")
        list_deployments
        ;;
    "rollback")
        rollback "$2"
        ;;
    "cleanup")
        cleanup "$2"
        ;;
    "status")
        show_status
        ;;
    "config")
        show_config
        ;;
    "help"|*)
        show_help
        ;;
esac 