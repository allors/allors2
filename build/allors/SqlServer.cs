using System;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using static Nuke.Common.Logger;

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
            Normal("SqlServer: Start");
            this.manager.Start();
        }
    }

    public void Restart()
    {
        if (this.dbInstance.IsRunning)
        {
            Normal("SqlServer: Stop");
            try
            {
                this.manager.Stop();
            }
            catch { }
        }

        if (!this.dbInstance.IsRunning)
        {
            Normal("SqlServer: Start");
            this.manager.Start();
        }
    }

    public void Populate(AbsolutePath commandsPath) => DotNet("Commands.dll Populate", commandsPath);
    
    public void Drop(string database) => this.ExecuteCommand($"DROP DATABASE [{database}]");

    public void Create(string database) => this.ExecuteCommand($"CREATE DATABASE [{database}]");

    public void Dispose()
    {
        this.sqlLocalDbApi?.Dispose();

        this.sqlLocalDbApi = null;
        this.dbInstance = null;
        this.manager = null;
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
