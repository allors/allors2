// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionedObjectDataRecord.cs" company="Allors bvba">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.SqlServer.Server;

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
                new SqlMetaData(objectArrayElement, SqlDbType.BigInt)
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
