#!/bin/bash
set -e

echo "Updating migration job: ${CONTAINER_APP_NAME}-migration"

az containerapp job update \
  --name "${CONTAINER_APP_NAME}-migration" \
  --resource-group "$RESOURCE_GROUP" \
  --image "${CONTAINER_REGISTRY}.azurecr.io/${CONTAINER_APP_NAME}:${GITHUB_SHA}" \
  --args "migrate"

echo "Migration job update complete"
