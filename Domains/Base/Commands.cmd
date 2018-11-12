@echo off

dotnet run --project Database/Commands

set /p args="Enter arguments: "

echo.

dotnet run --project Database/Commands -- %args%

echo.
pause


