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

@SET PATH=%PATH%;C:\Program Files\MSBuild\14.0\Bin;C:\Program Files (x86)\MSBuild\14.0\Bin;C:\Windows\Microsoft.NET\Framework64\v4.0.30319;C:\Windows\Microsoft.NET\Framework\v4.0.30319


rmdir /s /q .\Database\Domain\Generated >nul 2>&1
rmdir /s /q .\Database\Diagrams\Generated >nul 2>&1
rmdir /s /q .\Workspace\Typescript\Domain\src\allors\meta\generated >nul 2>&1
rmdir /s /q .\Workspace\Typescript\Domain\src\allors\domain\generated >nul 2>&1
rmdir /s /q .\Workspace\Typescript\Angular\src\allors\meta\generated >nul 2>&1
rmdir /s /q .\Workspace\Typescript\Angular\src\allors\domain\generated >nul 2>&1

@echo ==========
@echo Repository
@echo ==========

dotnet msbuild Repository.sln /target:Clean /verbosity:minimal
..\..\Platform\Repository\Generate\dist\Allors.Generate.Cmd.exe repository generate repository.sln domain ../Core/Repository/Templates/meta.cs.stg database/meta/generated || SET /A errno^|=%ERROR_GENERATE_META% && GOTO :END

@echo ====================
@echo Domain and Workspace
@echo ====================

dotnet restore Database.sln
dotnet msbuild Database.sln /target:Clean /verbosity:minimal
dotnet msbuild Database.sln /target:Database\Generate:Rebuild /p:Configuration="Debug" /verbosity:minimal || SET /A errno^|=%ERROR_BUILD_META% && GOTO :END

@echo Generating

dotnet Database\Generate\bin\Debug\netcoreapp2.0\Generate.dll
msbuild Database\Resources/Merge.proj /verbosity:minimal

:END
IF "%interactive%"=="1" PAUSE
ENDLOCAL
ECHO ON
@EXIT /B %errno%