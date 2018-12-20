@echo off
@echo =====
@echo Clean
@echo =====

rmdir /s /q .\Database\Meta\Generated >nul 2>&1
rmdir /s /q .\Database\Domain\Generated >nul 2>&1

@echo ==========
@echo Repository
@echo ==========

dotnet restore ..\..\Platform\Repository\Repository.sln
dotnet msbuild ..\..\Platform\Repository\Repository.sln

dotnet restore Repository.sln

cd repository/domain
dotnet ..\..\..\..\Platform\Repository\Generate\bin\Debug\netcoreapp2.2\Generate.dll repository.csproj ../../../Core/Repository/Templates/meta.cs.stg ../../database/meta/generated
cd ../..

@echo ====================
@echo Domain and Workspace
@echo ====================

dotnet restore Database.sln
dotnet msbuild Database.sln /target:Clean /verbosity:minimal
dotnet msbuild Database.sln /target:Database\Generate:Rebuild /p:Configuration="Debug" /verbosity:minimal

@echo Generating

dotnet Database\Generate\bin\Debug\netcoreapp2.2\Generate.dll
rem dotnet msbuild Database\Resources/Merge.proj /verbosity:minimal

pause



