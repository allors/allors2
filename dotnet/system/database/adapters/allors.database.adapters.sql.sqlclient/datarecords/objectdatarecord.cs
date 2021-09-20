// <copyright file="ObjectDataRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient.Server;

    internal class ObjectDataRecord : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly IEnumerable<long> objectIds;

        internal ObjectDataRecord(Mapping mapping, IEnumerable<long> objectIds)
        {
            this.mapping = mapping;
            this.objectIds = objectIds;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var objectArrayElement = this.mapping.TableTypeColumnNameForObject;
            var metaData = new SqlMetaData(objectArrayElement, SqlDbType.BigInt);
            var sqlDataRecord = new SqlDataRecord(metaData);

            foreach (var objectId in this.objectIds)
            {
                sqlDataRecord.SetInt64(0, objectId);
                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
