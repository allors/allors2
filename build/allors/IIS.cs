using System;
using Nuke.Common.Tooling;

partial class IIS : IDisposable
{
    private const string Appcmd = @"%SYSTEMROOT%\System32\inetsrv\appcmd";

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
