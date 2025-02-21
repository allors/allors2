using System;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using Nuke.Common.IO;
using static Serilog.Log;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class SqlServer : IDisposable
{
    private SqlLocalDbApi sqlLocalDbApi;
    private ISqlLocalDbInstanceInfo dbInstance;
    private ISqlLocalDbInstanceManager manager;

    public SqlServer()
    {
        this.sqlLocalDbApi = new SqlLocalDbApi();
        this.dbInstance = this.sqlLocalDbApi.GetDefaultInstance();
        this.manager = this.dbInstance.Manage();

        if (!this.dbInstance.IsRunning)
        {
            Information("SqlServer: Start");
            this.manager.Start();
        }
    }

    public void Restart()
    {
        if (this.dbInstance.IsRunning)
        {
            Information("SqlServer: Stop");
            try
            {
                this.manager.Stop();
            }
            catch { }
        }

        if (!this.dbInstance.IsRunning)
        {
            Information("SqlServer: Start");
            this.manager.Start();
        }
    }

    public void Populate(AbsolutePath commandsPath) => DotNet("Commands.dll Populate", commandsPath);

    public void Drop(string database) => this.ExecuteCommand($"DROP DATABASE IF EXISTS [{database}]");

    public void Create(string database) => this.ExecuteCommand($"CREATE DATABASE [{database}]");

    public void Dispose()
    {
        try
        {
            this.manager.Stop();
        }
        catch { }

        this.sqlLocalDbApi?.Dispose();

        this.sqlLocalDbApi = null;
        this.dbInstance = null;
        this.manager = null;
    }

    private int ExecuteCommand(string commandText)
    {
        using var connection = this.manager.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(commandText, connection);
        return command.ExecuteNonQuery();
    }
}
