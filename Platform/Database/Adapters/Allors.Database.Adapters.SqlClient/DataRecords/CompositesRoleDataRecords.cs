// <copyright file="CompositesRoleDataRecords.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.Data.SqlClient.Server;

    internal class CompositesRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly IEnumerable<Reference> strategies;

        internal CompositesRoleDataRecords(Mapping mapping, IEnumerable<Reference> strategies)
        {
            this.mapping = mapping;
            this.strategies = strategies;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var objectArrayElement = this.mapping.TableTypeColumnNameForObject;
            var metaData = new SqlMetaData(objectArrayElement, SqlDbType.BigInt);
            var sqlDataRecord = new SqlDataRecord(metaData);
            foreach (var strategy in this.strategies)
            {
                sqlDataRecord.SetInt64(0, strategy.ObjectId);
                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
