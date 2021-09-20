@echo off
@echo =====
@echo Clean
@echo =====

rmdir /s /q .\Workspace\Typescript\Intranet.Tests\generated >nul 2>&1

@echo ========
@echo Angular
@echo ========

cd %~dp0
cd .\Workspace\Typescript\Intranet

call npm install
call npm run autotest

cd %~dp0
cd .\Workspace\Typescript\Autotest\Angular

call npm install
call npm run autotest

@echo ========
@echo Autotest
@echo ========

cd %~dp0

dotnet restore .\Workspace\Typescript\Autotest\Autotest.sln
dotnet run --project .\Workspace\Typescript\Autotest\Generate\Generate.csproj

pause



