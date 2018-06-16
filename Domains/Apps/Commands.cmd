@echo off

dotnet restore Database.sln /verbosity:quiet /nologo
dotnet msbuild Database.sln /target:Database\Commands /verbosity:quiet /nologo

dotnet Database\Commands\bin\Debug\netcoreapp2.0\Commands.dll

set /p args="Enter arguments: "

echo.

cd Database\Commands\
dotnet bin\Debug\netcoreapp2.0\Commands.dll %args%

echo.
pause


