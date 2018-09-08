#/bin/sh

rm -rf Meta/Generated
rm -rf Domain/Generated

dotnet restore ../../Repository/Repository.sln
dotnet msbuild ../../Repository/Repository.sln

dotnet restore Repository.sln

cd repository
dotnet ../../../Repository/Generate/bin/Debug/netcoreapp2.0/Generate.dll repository.csproj ../../../../domains/core/repository/templates/meta.cs.stg ../meta/generated
cd ..

read -p "Press enter to continue"

dotnet msbuild Adapters.sln /target:Clean /verbosity:minimal

dotnet build Generate/Generate.csproj

dotnet Generate/bin/Debug/netcoreapp2.0/Generate.dll

