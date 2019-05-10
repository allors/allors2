@echo off

set ASPNETCORE_ENVIRONMENT=Development
cd database\server
dotnet run --configuration Debug

pause

