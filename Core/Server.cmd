@echo off

set ASPNETCORE_ENVIRONMENT=Development
cd database\server

rem dotnet run --no-build --configuration Debug
dotnet run --configuration Debug

pause

