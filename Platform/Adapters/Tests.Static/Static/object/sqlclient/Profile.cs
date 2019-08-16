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

namespace Allors.Adapters.Object.SqlClient
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public abstract class Profile : Adapters.Profile
    {
        protected abstract string ConnectionString { get; }

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

        protected abstract bool Match(ColumnTypes columnType, string dataType);
    }
}
