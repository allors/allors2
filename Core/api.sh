#!/bin/bash

ASPNETCORE_ENVIRONMENT=Development
dotnet run --project database/api --no-build --configuration Debug
