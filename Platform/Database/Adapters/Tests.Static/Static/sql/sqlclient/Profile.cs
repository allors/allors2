// <copyright file="Profile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Text;
    using Adapters;
    using Caching;
    using Debug;
    using Meta;
    using Microsoft.Extensions.DependencyInjection;

    public class Profile : Adapters.Profile
    {
        private readonly Prefetchers prefetchers = new Prefetchers();

        private readonly DebugConnectionFactory connectionFactory;
        private readonly DefaultCacheFactory cacheFactory;

        public Profile()
        {
            var services = new ServiceCollection();
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public Profile(DebugConnectionFactory connectionFactory, DefaultCacheFactory cacheFactory)
        : this()
        {
            this.connectionFactory = connectionFactory;
            this.cacheFactory = cacheFactory;
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

                if (Settings.ExtraMarkers)
                {
                    markers.Add(
                        () =>
                        {
                            foreach (var @class in this.Session.Database.MetaPopulation.Classes)
                            {
                                var prefetchPolicy = this.prefetchers[@class];
                                this.Session.Prefetch(prefetchPolicy, this.Session.Extent(@class).ToArray());
                            }
                        });
                }

                return markers.ToArray();
            }
        }

        protected string ConnectionString
        {
            get
            {
                if (Settings.IsWindows)
                {
                    //return @"Server=(local);Database=Adapters;Integrated Security=true";
                    return @"Server=(localdb)\MSSQLLocalDB;Database=Adapters;Integrated Security=true";
                }

                return "server=localhost;database=adapters;User Id=SA;Password=Allors123";
            }
        }

        public IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init)
        {
            var configuration = new SqlClient.Configuration
            {
                ObjectFactory = this.CreateObjectFactory(metaPopulation),
                ConnectionString = this.ConnectionString,
                ConnectionFactory = this.connectionFactory,
                CacheFactory = this.cacheFactory,
            };
            var database = new Database(this.ServiceProvider, configuration);

            if (init)
            {
                database.Init();
            }

            return database;
        }

        public override IDatabase CreatePopulation() => new Adapters.Memory.Database(this.ServiceProvider, new Adapters.Memory.Configuration { ObjectFactory = this.ObjectFactory });

        public override IDatabase CreateDatabase()
        {
            var configuration = new SqlClient.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.ConnectionString,
                ConnectionFactory = this.connectionFactory,
                CacheFactory = this.cacheFactory,
            };

            var database = new Database(this.ServiceProvider, configuration);

            return database;
        }

        protected bool Match(ColumnTypes columnType, string dataType)
        {
            dataType = dataType.Trim().ToLowerInvariant();

            switch (columnType)
            {
                case ColumnTypes.ObjectId:
                    return dataType.Equals("int");
                case ColumnTypes.TypeId:
                    return dataType.Equals("uniqueidentifier");
                case ColumnTypes.CacheId:
                    return dataType.Equals("int");
                case ColumnTypes.Binary:
                    return dataType.Equals("varbinary");
                case ColumnTypes.Boolean:
                    return dataType.Equals("bit");
                case ColumnTypes.Decimal:
                    return dataType.Equals("decimal");
                case ColumnTypes.Float:
                    return dataType.Equals("float");
                case ColumnTypes.Integer:
                    return dataType.Equals("int");
                case ColumnTypes.String:
                    return dataType.Equals("nvarchar");
                case ColumnTypes.Unique:
                    return dataType.Equals("uniqueidentifier");
                default:
                    throw new Exception("Unsupported columntype " + columnType);
            }
        }

        public void DropProcedure(string procedure)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var sql = new StringBuilder();
                    sql.Append("DROP PROCEDURE " + procedure);

                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DropTable(string schema, string table)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                var cmdText = @"
IF EXISTS (SELECT * FROM information_schema.tables WHERE table_name = @tableName AND table_schema = @tableSchema)
BEGIN
DROP TABLE " + schema + "." + table + @"
END
";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.Add("@tableName", SqlDbType.NVarChar).Value = table;
                    command.Parameters.Add("@tableSchema", SqlDbType.NVarChar).Value = schema;
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ExistTable(string schema, string table)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT COUNT(*) 
FROM information_schema.tables 
WHERE table_name = @tableName AND table_schema = @tableSchema";

                    command.CommandText = cmdText;

                    command.Parameters.Add("@tableName", SqlDbType.NVarChar).Value = table;
                    command.Parameters.Add("@tableSchema", SqlDbType.NVarChar).Value = schema;
                    var count = (int)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public int ColumnCount(string schema, string table)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT count(table_name)
FROM information_schema.columns
WHERE table_name = @tableName AND table_schema = @tableSchema";

                    command.CommandText = cmdText;
                    command.Parameters.Add("@tableName", SqlDbType.NVarChar).Value = table;
                    command.Parameters.Add("@tableSchema", SqlDbType.NVarChar).Value = schema;
                    var count = (int)command.ExecuteScalar();

                    return count;
                }
            }
        }

        public bool ExistColumn(string schema, string table, string column, ColumnTypes columnType)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT data_type
FROM information_schema.columns
WHERE table_name = @tableName AND table_schema = @tableSchema AND
column_name=@columnName";

                    command.CommandText = cmdText;

                    command.Parameters.Add("@tableName", SqlDbType.NVarChar).Value = table;
                    command.Parameters.Add("@tableSchema", SqlDbType.NVarChar).Value = schema;
                    command.Parameters.Add("@columnName", SqlDbType.NVarChar).Value = column;

                    var dataType = (string)command.ExecuteScalar();
                    if (string.IsNullOrWhiteSpace(dataType))
                    {
                        return false;
                    }

                    return this.Match(columnType, dataType);
                }
            }
        }

        public bool ExistIndex(string schema, string table, string column)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT COUNT(*)
FROM sys.indexes AS idx
JOIN sys.index_columns idxcol
ON idx.object_id = idxcol.object_id AND idx.index_id=idxcol.index_id
WHERE idx.type = 2 -- Non Clusterd
and key_ordinal = 1 -- 1 based

and object_name(idx.object_id) = @tableName
and object_schema_name(idx.object_id) = @tableSchema
and col_name(idx.object_id,idxcol.column_id) = @columnName";

                    command.CommandText = cmdText;
                    command.Parameters.Add("@tableName", SqlDbType.NVarChar).Value = table;
                    command.Parameters.Add("@tableSchema", SqlDbType.NVarChar).Value = schema;
                    command.Parameters.Add("@columnName", SqlDbType.NVarChar).Value = column;

                    var count = (int)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool ExistProcedure(string schema, string procedure)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT count(*)
FROM INFORMATION_SCHEMA.ROUTINES
WHERE routine_schema = @routineSchema 
AND routine_name=@routineName";

                    command.CommandText = cmdText;
                    command.Parameters.Add("@routineSchema", SqlDbType.NVarChar).Value = schema;
                    command.Parameters.Add("@routineName", SqlDbType.NVarChar).Value = procedure;

                    var count = (int)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }

        public bool ExistPrimaryKey(string schema, string table, string column)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT count(*)
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1
AND table_name = @tableName 
AND table_schema = @tableSchema 
AND column_name=@columnName";

                    command.CommandText = cmdText;
                    command.Parameters.Add("@tableName", SqlDbType.NVarChar).Value = table;
                    command.Parameters.Add("@tableSchema", SqlDbType.NVarChar).Value = schema;
                    command.Parameters.Add("@columnName", SqlDbType.NVarChar).Value = column;

                    var count = (int)command.ExecuteScalar();

                    return count != 0;
                }
            }
        }
    }
}
