#!/bin/bash
set -e

if [ "$1" = 'migrate' ]; then
  echo "Running database migration..."
  dotnet ef database update --project /src/WebCargoService/WebCargoService.csproj
else
  echo "Starting WebCargoService..."
  exec "$@"
fi
