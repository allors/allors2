// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using global::Npgsql;
    using Meta;
    using Microsoft.Extensions.DependencyInjection;
    using MysticMind.PostgresEmbed;

    public class Profile : Adapters.Profile
    {
        private readonly PgServer pgServer;

        public Profile(PgServer pgServer)
        {
            this.pgServer = pgServer;
            var services = new ServiceCollection();
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; set; }

        public override Action[] Markers
        {
            get
            {
                var markers = new List<Action>
                {
                    () => { },
                    () => this.Session.Commit(),
                };

                return markers.ToArray();
            }
        }

        protected string ConnectionString => (this.pgServer != null) ?
            $"Server=localhost; Port={this.pgServer.PgPort}; User Id=postgres; Password=test; Database=postgres; Pooling=false; Timeout=300; CommandTimeout=300" :
            $"Server=localhost; User Id=allors; Password=allors; Database=adapters; Pooling=false; Timeout=300; CommandTimeout=300";

        public IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init)
        {
            var configuration = new Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.ConnectionString,
                IsolationLevel = IsolationLevel.Serializable,
            };

            var database = new Database(this.ServiceProvider, configuration);

            if (init)
            {
                database.Init();
            }

            return database;
        }

        public override IDatabase CreateDatabase()
        {
            var configuration = new Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.ConnectionString,
            };

            var database = new Database(this.ServiceProvider, configuration);

            return database;
        }

        public override IDatabase CreatePopulation() => new Memory.Database(this.ServiceProvider, new Memory.Configuration { ObjectFactory = this.ObjectFactory });

        protected NpgsqlConnection CreateDbConnection() => new NpgsqlConnection(this.ConnectionString);

        public void DropTable(string tableName)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql = new StringBuilder();
                    sql.Append("DROP TABLE IF EXISTS " + tableName);
                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ExistIndex(string table, string column)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql = new StringBuilder();
                    sql.Append("SELECT COUNT(*)\n");
                    sql.Append("FROM pg_class, pg_attribute, pg_index\n");
                    sql.Append("WHERE pg_class.oid = pg_attribute.attrelid AND\n");
                    sql.Append("pg_class.oid = pg_index.indrelid AND\n");
                    sql.Append("pg_index.indkey[0] = pg_attribute.attnum\n");

                    sql.Append("AND lower(pg_class.relname) = '" + table.ToLower() + "'\n");
                    sql.Append("AND lower(pg_attribute.attname) = '" + column.ToLower() + "'\n");

                    command.CommandText = sql.ToString();
                    var count = (long)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool ExistProcedure(string procedure)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql =
@"SELECT ROUTINE_NAME, ROUTINE_DEFINITION 
FROM INFORMATION_SCHEMA.ROUTINES
WHERE lower(ROUTINE_NAME) = '" + procedure.ToLower() + @"'";

                    command.CommandText = sql;
                    var count = (long)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool ExistPrimaryKey(string table, string column)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql =
@"select count(*) 
from information_schema.constraint_column_usage 
where lower(table_name) = '" + table.ToLowerInvariant() + "' and lower(constraint_name) = '" + table.ToLowerInvariant() + "_pk'";

                    command.CommandText = sql;
                    var count = (long)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool IsInteger(string table, string column)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql =
@"SELECT count(*)
FROM information_schema.columns
WHERE lower(table_name) = '" + table.ToLower() + @"'
AND lower(column_name) = '" + column.ToLower() + @"'
AND data_type = 'integer'";

                    command.CommandText = sql;
                    var count = (long)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool IsLong(string table, string column)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql =
@"SELECT count(*)
FROM information_schema.columns
WHERE lower(table_name) = '" + table.ToLower() + @"'
AND lower(column_name) = '" + column.ToLower() + @"'
AND data_type = 'bigint'";

                    command.CommandText = sql;
                    var count = (long)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool IsUnique(string table, string column)
        {
            using (var connection = this.CreateDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql =
@"SELECT count(*)
FROM information_schema.columns
WHERE lower(table_name) = '" + table.ToLower() + @"'
AND lower(column_name) = '" + column.ToLower() + @"'
AND data_type = 'uuid'";

                    command.CommandText = sql;
                    var count = (long)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }
    }
}
