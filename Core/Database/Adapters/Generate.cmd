@echo off
@echo =====
@echo Clean
@echo =====

rmdir /s /q .\Meta\Generated >nul 2>&1
rmdir /s /q .\Domain\Generated >nul 2>&1

@echo ==========
@echo Repository
@echo ==========

dotnet restore ..\..\Repository\Repository.sln
dotnet msbuild ..\..\Repository\Repository.sln

dotnet restore Repository.sln

cd repository
dotnet ..\..\..\Repository\Generate\bin\Debug\netcoreapp2.2\Generate.dll repository.csproj ../../../repository/templates/meta.cs.stg ../meta/generated
cd ../

@echo ========
@echo Adapters
@echo ========

dotnet restore Adapters.sln

dotnet msbuild Adapters.sln /target:Clean /verbosity:minimal
dotnet msbuild Adapters.sln /target:Generate:Rebuild /p:Configuration="Debug" /verbosity:minimal

dotnet Generate\bin\Debug\netcoreapp2.2\Generate.dll

pause

