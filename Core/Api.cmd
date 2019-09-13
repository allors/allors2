@echo off

set ASPNETCORE_ENVIRONMENT=Development
cd database\api

dotnet run --no-build --configuration Debug
rem dotnet run --configuration Debug

pause

