#!/bin/bash
set -e

if [ "$1" = 'migrate' ]; then
  echo "Running database migration..."
  dotnet ef database update --project /src/UserService/UserService.csproj
else
  echo "Starting UserService..."
  exec "$@"
fi
