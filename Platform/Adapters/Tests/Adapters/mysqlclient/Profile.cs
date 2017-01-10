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

namespace Allors.Database.Special.MySqlClient
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

        public bool ExistProcedure(string procedure)
        {
            throw new NotImplementedException();
        }
    }
}