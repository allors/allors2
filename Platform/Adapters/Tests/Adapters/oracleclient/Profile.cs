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

namespace Allors.Database.Special.OracleClient
{
    using System;
    using System.Text;

    using Allors.Database.Sql;

    public abstract class Profile : Special.Profile
    {
        public void DropTable(string tableName)
        {
            using (var connection = ((Database)this.CreateDatabase()).CreateDbConnection())
            {
                connection.Open();

                string actualTableName = null;
                using (var command = connection.CreateCommand())
                {
                    var sql = new StringBuilder();
                    sql.Append("SELECT table_name\n");
                    sql.Append("FROM user_tables\n");
                    sql.Append("WHERE UPPER(table_name) = UPPER('" + tableName + "')\n");
                    command.CommandText = sql.ToString();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            actualTableName = (string)reader["table_name"];
                        }

                        reader.Close();
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    if (actualTableName != null)
                    {
                        try
                        {
                            var sql = new StringBuilder();
                            sql.Append("DROP TABLE " + actualTableName);
                            command.CommandText = sql.ToString();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Could not drop table " + actualTableName + " although it exists in the database.\nDo you have permissions on this table?", e);
                        }
                    }
                }
            }
        }

        public bool ExistProcedure(string procedure)
        {
            throw new NotImplementedException();
        }

        public bool ExistIndex(string table, string column)
        {
            throw new NotImplementedException();
        }

        public bool IsInteger(string table, string column)
        {
            throw new NotImplementedException();
        }

        public bool IsLong(string table, string column)
        {
            throw new NotImplementedException();
        }

        public bool IsUnique(string table, string column)
        {
            throw new NotImplementedException();
        }
    }
}