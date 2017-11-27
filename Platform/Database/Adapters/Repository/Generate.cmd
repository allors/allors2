@SET PATH=%PATH%;C:\Program Files\MSBuild\14.0\Bin;C:\Program Files (x86)\MSBuild\14.0\Bin;C:\Windows\Microsoft.NET\Framework64\v4.0.30319;C:\Windows\Microsoft.NET\Framework\v4.0.30319

msbuild ..\Adapters.sln /target:Meta:Rebuild /p:Configuration="Debug" /verbosity:minimal
msbuild Generate.proj 

@pause