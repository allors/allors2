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
..\Repository\Generate\dist\Allors.Generate.Cmd.exe repository generate Repository.sln repository ../../domains/core/repository/templates/meta.cs.stg meta/generated || SET /A errno^|=%ERROR_BUILD_META% && GOTO :END

@echo ========
@echo Adapters
@echo ========

msbuild Adapters.sln /target:Clean /verbosity:minimal
msbuild Adapters.sln /target:Meta:Rebuild /p:Configuration="Debug" /verbosity:minimal || SET /A errno^|=%ERROR_BUILD_META% && GOTO :END

msbuild Domain\Generate.proj /verbosity:minimal || SET /A errno^|=%ERROR_BUILD_META% && GOTO :END

:END
IF "%interactive%"=="1" PAUSE
ENDLOCAL
ECHO ON
@EXIT /B %errno%