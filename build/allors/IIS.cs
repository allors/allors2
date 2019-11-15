using System;
using Nuke.Common.Tooling;

partial class IIS : IDisposable
{
    private const string Appcmd = @"C:\Windows\System32\inetsrv\appcmd.exe";

    private readonly string[] appPoolNames;

    public IIS(params string[] appPoolNames)
    {
        this.appPoolNames = appPoolNames;

        foreach (var appPoolName in this.appPoolNames)
        {
            ProcessTasks.StartProcess(Appcmd, @$"STOP APPPOOL ""{appPoolName}""").WaitForExit();
        }
    }

    public void Dispose()
    {
        foreach (var appPoolName in this.appPoolNames)
        {
            ProcessTasks.StartProcess(Appcmd, @$"START APPPOOL ""{appPoolName}""").WaitForExit();
        }
    }
}
