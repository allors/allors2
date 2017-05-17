@echo off

echo Preparing Console

dotnet msbuild Base.sln /target:Database\Console /verbosity:quiet /nologo

set /p args="Enter arguments: "

echo.

dotnet Database\Console\bin\Debug\netcoreapp1.1\Console.dll %args%

echo.
pause


