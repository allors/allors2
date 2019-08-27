#!/bin/bash

ASPNETCORE_ENVIRONMENT=Development
dotnet run --project database/server --no-build --configuration Debug
