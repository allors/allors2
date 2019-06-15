using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using MartinCostello.SqlLocalDb;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Logger;
using static Nuke.Common.IO.PathConstruction;

partial class SqlServer : IDisposable
{
    SqlLocalDbApi api;
    ISqlLocalDbInstanceInfo dbInstance;
    ISqlLocalDbInstanceManager manager;

    public SqlServer()
    {
        api = new SqlLocalDbApi();
        dbInstance = api.GetOrCreateInstance("MyInstance");
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

    public void Dispose()
    {
        api?.Dispose();

        api = null;
        dbInstance = null;
        manager = null;
    }
}
