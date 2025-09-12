#!/bin/bash
set -e

if [ "$1" = 'migrate' ]; then
  echo "Running database migration..."
  dotnet ef database update --project /src/QuotationService/QuotationService.csproj
else
  echo "Starting QuotationService..."
  exec "$@"
fi
