dotnet msbuild Database.sln /target:Database\Generate:Rebuild /p:Configuration="Debug" /verbosity:minimal
dotnet Database\Generate\bin\Debug\netcoreapp2.2\Generate.dll

pause



