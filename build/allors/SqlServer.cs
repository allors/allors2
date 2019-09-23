using System;
using MartinCostello.SqlLocalDb;
using static Nuke.Common.Logger;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class SqlServer : IDisposable
{
    SqlLocalDbApi sqlLocalDbApi;
    ISqlLocalDbInstanceInfo dbInstance;
    ISqlLocalDbInstanceManager manager;

    public SqlServer()
    {
        sqlLocalDbApi = new SqlLocalDbApi();
        dbInstance = sqlLocalDbApi.GetOrCreateInstance("MyInstance");
        manager = dbInstance.Manage();
    }

    public void Restart()
    {
        if (dbInstance.IsRunning)
        {
            Normal("SqlServer: Stop");
            manager.Stop();
        }

        if (!dbInstance.IsRunning)
        {
            Normal("SqlServer: Start");
            manager.Start();
        }
    }

    public void Populate(AbsolutePath commandsPath)
    {
        DotNet("Commands.dll Populate", commandsPath);
    }

    public void Dispose()
    {
        sqlLocalDbApi?.Dispose();

        sqlLocalDbApi = null;
        dbInstance = null;
        manager = null;
    }
}
