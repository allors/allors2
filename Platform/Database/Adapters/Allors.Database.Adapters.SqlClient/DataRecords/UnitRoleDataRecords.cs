// <copyright file="UnitRoleDataRecords.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    using Allors.Meta;

    using Microsoft.Data.SqlClient.Server;

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
                                    this.database.GetSqlMetaData(this.database.Mapping.TableTypeColumnNameForRole, this.roleType),
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

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
