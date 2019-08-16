
// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Text;

    using global::Npgsql;

    public abstract class Profile : Adapters.Profile
    {
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

        protected abstract NpgsqlConnection CreateDbConnection();
    }
}
