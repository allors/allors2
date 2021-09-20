using System;
using System.Collections.Generic;
using MysticMind.PostgresEmbed;
using Npgsql;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build
{
    private Target PostgresDocker => _ => _
        .Executes(() =>
        {
            DockerImagePull(v => v.SetName("postgres"));
            DockerRun(v => v
                .SetDetach(true)
                .SetImage("postgres")
                .SetName("pg")
                .SetPublish("5432:5432")
                .AddEnv("POSTGRES_USER=allors")
                .AddEnv("POSTGRES_PASSWORD=Password1234"));
        });

    private class Postgres : IDisposable
    {
        private readonly PgServer pgServer;

        public Postgres()
        {
            var pgServerParams = new Dictionary<string, string> {{"timezone", "UTC"}, {"synchronous_commit", "off"}};

            pgServer = new PgServer(
                "10.7.1",
                "allors",
                port: 5432,
                pgServerParams: pgServerParams,
                addLocalUserAccessPermission: true,
                locale: "English_Belgium.1252");

            pgServer.Start();
        }

        public void Dispose()
        {
            pgServer.Stop();
            pgServer.Dispose();
        }

        public void Init(string database)
        {
            using var conn = new NpgsqlConnection("Server=localhost;User Id=allors;Database=postgres");

            using var cmd =
                new NpgsqlCommand(
                    @$"DROP DATABASE IF EXISTS {database};
                            CREATE DATABASE {database};",
                    conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
