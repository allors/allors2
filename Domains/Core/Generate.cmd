:: Name:     Generate.cmd
:: Purpose:  Phase 1 generates from Repository to Meta and phase 2 generates from Meta.
:: Author:   koen@allors.com
:: Revision: December 2016 - refactored from old cmd file

@ECHO OFF
SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

:: interactive
SET interactive=0
ECHO %CMDCMDLINE% | FINDSTR /L %COMSPEC% >NUL 2>&1
IF %ERRORLEVEL% == 0 SET interactive=1

:: variables
SET me=%~n0

:: error
SET /A errno=0
SET /A ERROR_GENERATE_META=1
SET /A ERROR_BUILD_META=2

:: purge files
rmdir /s /q .\Database\Domain\Generated >nul 2>&1

:: Repository
dotnet msbuild Repository.sln /target:Clean /verbosity:minimal
..\..\Tools\Generate\dist\Allors.Generate.Cmd.exe repository generate repository.sln domain repository/templates/meta.cs.stg database/meta/generated || SET /A errno^|=%ERROR_GENERATE_META% && GOTO :END

:END
IF "%interactive%"=="1" PAUSE
ENDLOCAL
ECHO ON
@EXIT /B %errno%