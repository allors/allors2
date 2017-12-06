@echo off

echo Preparing Console

dotnet msbuild Base.sln /target:Database\Console /verbosity:quiet /nologo

cd Database\Console

dotnet bin\Debug\netcoreapp2.0\Console.dll -h

set /p args="Enter arguments: "

echo.

dotnet bin\Debug\netcoreapp2.0\Console.dll %args%

echo.
pause


