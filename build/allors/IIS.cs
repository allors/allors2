using System;
using System.IO;
using Nuke.Common.Tooling;

internal class IIS : IDisposable
{
    private const string Appcmd = @"C:\Windows\System32\inetsrv\appcmd.exe";

    private readonly string[] appPoolNames;

    public IIS(params string[] appPoolNames)
    {
        if (File.Exists(Appcmd))
        {
            this.appPoolNames = appPoolNames;

            foreach (var appPoolName in this.appPoolNames)
            {
                ProcessTasks.StartProcess(Appcmd, @$"STOP APPPOOL ""{appPoolName}""").WaitForExit();
            }
        }
    }

    public void Dispose()
    {
        if (File.Exists(Appcmd))
        {
            foreach (var appPoolName in appPoolNames)
            {
                ProcessTasks.StartProcess(Appcmd, @$"START APPPOOL ""{appPoolName}""").WaitForExit();
            }
        }
    }
}
