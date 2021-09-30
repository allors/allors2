// <copyright file="CompositeRoleDataRecords.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient.Server;

    internal class CompositeRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly IEnumerable<CompositeRelation> relations;

        internal CompositeRoleDataRecords(Mapping mapping, IEnumerable<CompositeRelation> relations)
        {
            this.mapping = mapping;
            this.relations = relations;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var metaData = new[]
            {
                new SqlMetaData(this.mapping.TableTypeColumnNameForAssociation, SqlDbType.BigInt),
                new SqlMetaData(this.mapping.TableTypeColumnNameForRole, SqlDbType.BigInt),
            };
            var sqlDataRecord = new SqlDataRecord(metaData);

            foreach (var relation in this.relations)
            {
                sqlDataRecord.SetInt64(0, relation.Association);
                sqlDataRecord.SetInt64(1, relation.Role);
                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
