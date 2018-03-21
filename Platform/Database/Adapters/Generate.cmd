@echo off
@echo =====
@echo Clean
@echo =====

rmdir /s /q .\Meta\Generated >nul 2>&1
rmdir /s /q .\Domain\Generated >nul 2>&1

@echo ==========
@echo Repository
@echo ==========

dotnet msbuild Repository.sln /target:Clean /verbosity:minimal

cd repository
dotnet ..\..\..\Repository\dist\Generate.dll repository.csproj ../../../../domains/core/repository/templates/meta.cs.stg ../meta/generated
cd ../

@echo ========
@echo Adapters
@echo ========

dotnet msbuild Adapters.sln /target:Clean /verbosity:minimal
dotnet msbuild Adapters.sln /target:Generate:Rebuild /p:Configuration="Debug" /verbosity:minimal

dotnet Generate\bin\Debug\netcoreapp2.0\Generate.dll

