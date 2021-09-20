using System;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using static Nuke.Common.Logger;

internal class SqlServer : IDisposable
{
    private ISqlLocalDbInstanceInfo dbInstance;
    private ISqlLocalDbInstanceManager manager;
    private SqlLocalDbApi sqlLocalDbApi;

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

    public void Dispose()
    {
        sqlLocalDbApi?.Dispose();

        sqlLocalDbApi = null;
        dbInstance = null;
        manager = null;
    }

    public void Drop(string database) => ExecuteCommand($"DROP DATABASE IF EXISTS [{database}]");

    public void Create(string database) => ExecuteCommand($"CREATE DATABASE [{database}]");

    private int ExecuteCommand(string commandText)
    {
        using var connection = manager.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(commandText, connection);
        return command.ExecuteNonQuery();
    }
}
