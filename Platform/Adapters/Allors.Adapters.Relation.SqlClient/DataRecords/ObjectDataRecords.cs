// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectDataRecords.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

using Allors;

namespace Allors.Adapters.Relation.SqlClient
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.SqlServer.Server;

    public class ObjectDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;

        private readonly IEnumerable<long> objectIds;

        public ObjectDataRecords(Mapping mapping, IEnumerable<long> objectIds)
        {
            this.mapping = mapping;
            this.objectIds = objectIds;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var sqlDataRecord = new SqlDataRecord(this.mapping.SqlMetaDataForObject);
            foreach (var objectId in this.objectIds)
            {
                sqlDataRecord.SetInt64(0, objectId);
                yield return sqlDataRecord;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}