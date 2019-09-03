@echo off

set ASPNETCORE_ENVIRONMENT=Development
cd database\api
dotnet watch run --configuration Debug

pause

