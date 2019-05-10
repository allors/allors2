@echo off

set ASPNETCORE_ENVIRONMENT=Development
cd database\server
dotnet watch run --configuration Debug

pause

