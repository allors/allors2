// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
//   Copyright 2002-2010 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System.Text;

    using Allors.Adapters.Database.Npgsql.LongId;

    public abstract class Profile : Adapters.Profile
    {
        public void DropTable(string tableName)
        {
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
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