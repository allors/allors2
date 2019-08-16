// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitRoleDataRecords.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    using Allors.Meta;

    using Microsoft.SqlServer.Server;

    internal class UnitRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Database database;
        private readonly IRoleType roleType;
        private readonly IEnumerable<UnitRelation> relations;

        internal UnitRoleDataRecords(Database database, IRoleType roleType, IEnumerable<UnitRelation> relations)
        {
            this.database = database;
            this.roleType = roleType;
            this.relations = relations;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var metaData = new[]
                                {
                                    new SqlMetaData(this.database.Mapping.TableTypeColumnNameForAssociation, SqlDbType.BigInt),
                                    this.database.GetSqlMetaData(this.database.Mapping.TableTypeColumnNameForRole, this.roleType)
                                };
            var sqlDataRecord = new SqlDataRecord(metaData);

            foreach (var relation in this.relations)
            {
                sqlDataRecord.SetInt64(0, relation.Association);

                if (relation.Role == null)
                {
                    sqlDataRecord.SetValue(1, DBNull.Value);
                }
                else
                {
                    sqlDataRecord.SetValue(1, relation.Role);
                }

                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
