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

SET PATH=%PATH%;C:\Program Files\MSBuild\14.0\Bin;C:\Program Files (x86)\MSBuild\14.0\Bin;C:\Windows\Microsoft.NET\Framework64\v4.0.30319;C:\Windows\Microsoft.NET\Framework\v4.0.30319

:: purge files
rmdir /s /q .\Meta\src\meta\generated >nul 2>&1
rmdir /s /q .\Domain\src\domain\generated >nul 2>&1

@echo ====================
@echo Workspace Typescript
@echo ====================

:: Workspace Typescript

msbuild Meta/Generate.proj /verbosity:minimal
msbuild Domain/Generate.proj /verbosity:minimal
msbuild Angular/Generate.proj /verbosity:minimal

:END
IF "%interactive%"=="1" PAUSE
ENDLOCAL
ECHO ON
@EXIT /B %errno%