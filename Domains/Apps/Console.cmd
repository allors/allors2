@echo off

echo Preparing Console

dotnet msbuild Apps.sln /target:Database\Console /verbosity:quiet /nologo

dotnet Database\Console\bin\Debug\netcoreapp1.1\Console.dll -h

set /p args="Enter arguments: "

echo.

dotnet Database\Console\bin\Debug\netcoreapp1.1\Console.dll %args%

echo.
pause


