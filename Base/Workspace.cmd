@SET PATH=%PATH%;C:\Program Files\MSBuild\14.0\Bin;C:\Program Files (x86)\MSBuild\14.0\Bin;C:\Windows\Microsoft.NET\Framework64\v4.0.30319;C:\Windows\Microsoft.NET\Framework\v4.0.30319

msbuild Workspace.Meta/Generate.proj /verbosity:minimal
msbuild Workspace.Domain/Generate.proj /verbosity:minimal

@pause