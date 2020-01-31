using System;
using System.Dynamic;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using Nuke.Common.IO;
using static Nuke.Common.Logger;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class SqlServer : IDisposable
{
    SqlLocalDbApi sqlLocalDbApi;
    ISqlLocalDbInstanceInfo dbInstance;
    ISqlLocalDbInstanceManager manager;

    public SqlServer()
    {
        sqlLocalDbApi = new SqlLocalDbApi();
        dbInstance = sqlLocalDbApi.GetDefaultInstance();
        manager = dbInstance.Manage();

        if (!dbInstance.IsRunning)
        {
            Normal("SqlServer: Start");
            manager.Start();
        }
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

    public void Populate(AbsolutePath commandsPath) => DotNet("Commands.dll Populate", commandsPath);


    public void Drop(string database) => this.ExecuteCommand($"DROP DATABASE [{database}]");

    public void Create(string database) => this.ExecuteCommand($"CREATE DATABASE [{database}]");

    public void Dispose()
    {
        sqlLocalDbApi?.Dispose();

        sqlLocalDbApi = null;
        dbInstance = null;
        manager = null;
    }

    private void ExecuteCommand(string commandText)
    {
        using var connection = this.manager.CreateConnection();
        try
        {
            connection.Open();
            using var command = new SqlCommand(commandText, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch { }
        }
        finally
        {
            connection.Close();
        }
    }
}
