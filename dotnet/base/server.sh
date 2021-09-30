#/bin/bash

export ASPNETCORE_ENVIRONMENT="Development"
cd database/server
dotnet watch run --configuration Debug
