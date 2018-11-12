@echo off
IF "%~1"=="" GOTO Interactive

dotnet run -v q --project Database/Commands -- %*
GOTO End

:Interactive
dotnet run -v q --project Database/Commands
set /p args="Enter arguments: "
echo.
dotnet run -v q --project Database/Commands -- %args%

:End
