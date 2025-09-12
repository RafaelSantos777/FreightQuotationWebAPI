#!/bin/bash
set -e

JOB_NAME="${CONTAINER_APP_NAME}-migration"
echo "Starting migration job: $JOB_NAME"

az containerapp job start \
  --name "$JOB_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  
JOB_EXECUTION_NAME=$(az containerapp job execution list \
  --name "$JOB_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --query "sort_by(@, &properties.startTime)[-1].name" \
  -o tsv)

while true; do
  STATUS=$(az containerapp job execution show \
    --name "$JOB_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --job-execution-name "$JOB_EXECUTION_NAME" \
    --query "properties.status" -o tsv)
  if [[ "$STATUS" == "Succeeded" ]]; then
    echo "Migration job Succeeded"
    break
  elif [[ "$STATUS" == "Failed" ]]; then
    echo "Migration job Failed"
    exit 1
  elif [[ "$STATUS" == "Running" || "$STATUS" == "Unknown" ]]; then
    echo "Waiting for migration job completion..."
    sleep 10
  else
    echo "Unknown error"
    exit 1
  fi
done