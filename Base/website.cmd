@SET PATH=%PATH%;C:\Program Files\MSBuild\12.0\Bin;C:\Program Files (x86)\MSBuild\12.0\Bin;C:\Windows\Microsoft.NET\Framework\v4.0.30319

@msbuild Website/Generate.proj /verbosity:minimal
msbuild Diagrams.Website/Generate.proj /verbosity:minimal

@pause
