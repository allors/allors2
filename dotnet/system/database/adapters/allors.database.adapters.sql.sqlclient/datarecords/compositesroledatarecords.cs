// <copyright file="CompositesRoleDataRecords.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient.Server;

    internal class CompositesRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly IEnumerable<long> ids;

        internal CompositesRoleDataRecords(Mapping mapping, IEnumerable<long> ids)
        {
            this.mapping = mapping;
            this.ids = ids;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var objectArrayElement = this.mapping.TableTypeColumnNameForObject;
            var metaData = new SqlMetaData(objectArrayElement, SqlDbType.BigInt);
            var sqlDataRecord = new SqlDataRecord(metaData);
            foreach (var id in this.ids)
            {
                sqlDataRecord.SetInt64(0, id);
                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
