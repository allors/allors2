@echo ========
@echo Angular
@echo ========

cd %~dp0
cd .\Workspace\Typescript\Intranet

call npm run autotest

cd %~dp0
cd .\Workspace\Autotest\Angular

call npm run autotest

@echo ========
@echo Autotest
@echo ========

cd %~dp0

dotnet msbuild .\Workspace\Autotest\Autotest.sln

dotnet .\Workspace\Autotest\Generate\bin\Debug\netcoreapp2.2\Generate.dll

pause



