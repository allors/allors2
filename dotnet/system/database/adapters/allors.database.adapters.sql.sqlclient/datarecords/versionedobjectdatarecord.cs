// <copyright file="VersionedObjectDataRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient.Server;

    internal class VersionedObjectDataRecord : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly Dictionary<long, long> versionedObjects;

        internal VersionedObjectDataRecord(Mapping mapping, Dictionary<long, long> versionedObjects)
        {
            this.mapping = mapping;
            this.versionedObjects = versionedObjects;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var objectArrayElement = this.mapping.TableTypeColumnNameForObject;
            var metaData = new SqlMetaData[]
            {
                new SqlMetaData(objectArrayElement, SqlDbType.BigInt),
                new SqlMetaData(objectArrayElement, SqlDbType.BigInt),
            };
            var sqlDataRecord = new SqlDataRecord(metaData);

            foreach (var dictionaryEntry in this.versionedObjects)
            {
                var objectId = dictionaryEntry.Key;
                var objectVersion = dictionaryEntry.Value;

                sqlDataRecord.SetInt64(0, objectId);
                sqlDataRecord.SetInt64(1, objectVersion);
                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
