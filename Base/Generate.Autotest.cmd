@echo off
@echo =====
@echo Clean
@echo =====

rmdir /s /q .\Workspace\Typescript\Material.Tests\generated >nul 2>&1

@echo ========
@echo Angular
@echo ========

cd %~dp0
cd .\Workspace\Typescript\Material

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
dotnet msbuild .\Workspace\Typescript\Autotest\Autotest.sln

dotnet .\Workspace\Typescript\Autotest\Generate\bin\Debug\netcoreapp2.2\Generate.dll

pause



