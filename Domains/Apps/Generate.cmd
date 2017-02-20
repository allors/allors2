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

rmdir /s /q .\Domain\Generated >nul 2>&1

@echo ==========
@echo Repository
@echo ==========

msbuild Repository.sln /target:Clean /verbosity:minimal
..\..\Tools\Generate\dist\Allors.Generate.Cmd.exe repository generate repository.sln repository ../Core/Repository/Templates/meta.cs.stg database/meta/generated || SET /A errno^|=%ERROR_GENERATE_META% && GOTO :END

@echo ========
@echo Database
@echo ========

msbuild Apps.sln /target:Clean /verbosity:minimal
msbuild Apps.sln /target:Database\Meta:Rebuild /p:Configuration="Debug" /verbosity:minimal || SET /A errno^|=%ERROR_BUILD_META% && GOTO :END

msbuild Database/Domain/Generate.proj /verbosity:minimal
msbuild Database/Resources/Merge.proj /verbosity:minimal
msbuild Database/Domain.Diagrams/Generate.proj /verbosity:minimal

:END
IF "%interactive%"=="1" PAUSE
ENDLOCAL
ECHO ON
@EXIT /B %errno%