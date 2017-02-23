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
rmdir /s /q .\Workspace\CSharp\Domain\Generated >nul 2>&1

@echo ==========
@echo Repository
@echo ==========

dotnet msbuild Repository.sln /target:Clean /verbosity:minimal
..\..\Tools\Generate\dist\Allors.Generate.Cmd.exe repository generate repository.sln domain ../Core/Repository/Templates/meta.cs.stg database/meta/generated || SET /A errno^|=%ERROR_GENERATE_META% && GOTO :END

@echo ========
@echo Database
@echo ========

dotnet msbuild Base.sln /target:Clean /verbosity:minimal
dotnet msbuild Base.sln /target:Database\Meta:Rebuild /p:Configuration="Debug" /verbosity:minimal || SET /A errno^|=%ERROR_BUILD_META% && GOTO :END

dotnet msbuild Database/Domain/Generate.proj /verbosity:minimal
dotnet msbuild Database/Resources/Merge.proj /verbosity:minimal
dotnet msbuild Database/Domain.Diagrams/Generate.proj /verbosity:minimal

@echo =========
@echo Workspace
@echo =========

:: Workspace
dotnet msbuild Workspace/CSharp/Diagrams/Generate.proj /verbosity:minimal
dotnet msbuild Workspace/CSharp/Meta/Generate.proj /verbosity:minimal
dotnet msbuild Workspace/CSharp/Domain/Generate.proj /verbosity:minimal

dotnet msbuild Workspace/Typescript/Meta/Generate.proj /verbosity:minimal

:END
IF "%interactive%"=="1" PAUSE
ENDLOCAL
ECHO ON
@EXIT /B %errno%