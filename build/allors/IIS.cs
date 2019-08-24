using System;
using Nuke.Common.Tooling;
using static Nuke.Common.IO.PathConstruction;

partial class IIS : IDisposable
{
    private IProcess Process { get; set; }

    public IIS()
    {
        ProcessTasks.StartProcess("iisreset", "/stop").WaitForExit();
    }

    public void Dispose()
    {
        ProcessTasks.StartProcess("iisreset", "/start").WaitForExit();
    }
}
