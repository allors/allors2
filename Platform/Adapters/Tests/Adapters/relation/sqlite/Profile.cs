// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Relation.SQLite
{
    using System.Data;
    using System.Data.SQLite;
    using System.Reflection;
    using System.Text;

    public abstract class Profile : Adapters.Profile
    {
        public abstract string ConnectionString { get; }

        public void DropProcedure(string procedure)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
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
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                var cmdText = @"
DROP TABLE IF EXISTS " + table;
                using (var command = new SQLiteCommand(cmdText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ExistTable(string schema, string table)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT count(name) FROM sqlite_master WHERE name=@tableName AND type='table';
";

                    command.CommandText = cmdText;

                    command.Parameters.Add("@tableName", DbType.String).Value = table;

                    var count = (long)command.ExecuteScalar(); 
                    return count != 0;
                }
            }
        }

        public int ColumnCount(string schema, string table)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
pragma table_info(" + table + ")";

                    command.CommandText = cmdText;
                    using (var reader = command.ExecuteReader())
                    {
                        var count = 0;
                        while (reader.Read())
                        {
                            ++count;
                        }

                        return count;
                    }
                }
            }
        }

        public bool ExistColumn(string schema, string table, string column, ColumnTypes columnType)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
pragma table_info(" + table + ")";

                    command.CommandText = cmdText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = ((string)reader["name"]).Trim().ToLowerInvariant();
                            var type = ((string)reader["type"]).Trim().ToLowerInvariant();

                            if (name.Equals(column.Trim().ToLowerInvariant()))
                            {
                                switch (columnType)
                                {
                                    case ColumnTypes.ObjectId:
                                        return "integer".Equals(type);
                                    case ColumnTypes.TypeId:
                                        return "blob".Equals(type);
                                    case ColumnTypes.CacheId:
                                        return "integer".Equals(type);
                                    case ColumnTypes.Binary:
                                        return "blob".Equals(type);
                                    case ColumnTypes.Boolean:
                                        return "integer".Equals(type);
                                    case ColumnTypes.Decimal:
                                        return "text".Equals(type);
                                    case ColumnTypes.Float:
                                        return "real".Equals(type);
                                    case ColumnTypes.Integer:
                                        return "integer".Equals(type);
                                    case ColumnTypes.String:
                                        return "text".Equals(type);
                                    case ColumnTypes.Unique:
                                        return "text".Equals(type);
                                }

                                return true;
                            }
                        }
                    }

                    return false;
                }
            }
        }

        public bool ExistIndex(string schema, string table, string column)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var cmdText = @"
SELECT count(name) FROM sqlite_master WHERE name=@tableName type='index';
";

                    command.CommandText = cmdText;

                    command.Parameters.Add("@tableName", DbType.String).Value = table;
                    var count = (long)command.ExecuteScalar();
                    return count != 0;
                }
            }
        }

        public bool ExistProcedure(string schema, string procedure)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
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
                    command.Parameters.Add("@routineSchema", DbType.String).Value = schema;
                    command.Parameters.Add("@routineName", DbType.String).Value = procedure;

                    var count = (long)command.ExecuteScalar();
                    return count != 0;
                }
            }
        }

        public bool ExistPrimaryKey(string schema, string table, string column)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
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
                    command.Parameters.Add("@tableName", DbType.String).Value = table;
                    command.Parameters.Add("@tableSchema", DbType.String).Value = schema;
                    command.Parameters.Add("@columnName", DbType.String).Value = column;

                    var count = (long)command.ExecuteScalar();
                    return count != 0;
                }
            }
        }

        protected abstract bool Match(ColumnTypes columnType, string dataType);
    }
}