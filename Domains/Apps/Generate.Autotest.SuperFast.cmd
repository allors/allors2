@echo ========
@echo Angular
@echo ========

cd %~dp0
cd .\Workspace\Autotest\Angular

call npm run autotest

@echo ========
@echo Autotest
@echo ========

cd %~dp0

dotnet run --project .\Workspace\Autotest\Generate\Generate.csproj

pause



