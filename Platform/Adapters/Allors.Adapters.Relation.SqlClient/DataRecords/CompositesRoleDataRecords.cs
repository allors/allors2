// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositesRoleDataRecords.cs" company="Allors bvba">
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

    using Allors.Meta;

    using Microsoft.SqlServer.Server;

    public class CompositesRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly IRoleType roleType;
        private readonly Dictionary<long, long[]> roleByAssociation;

        public CompositesRoleDataRecords(Mapping mapping, IRoleType roleType, Dictionary<long, long[]> roleByAssociation)
        {
            this.mapping = mapping;
            this.roleType = roleType;
            this.roleByAssociation = roleByAssociation;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var sqlDataRecord = new SqlDataRecord(this.mapping.GetSqlMetaData(this.roleType));
            foreach (var entry in this.roleByAssociation)
            {
                var association = entry.Key;
                var roles = entry.Value;

                foreach (var role in roles)
                {
                    sqlDataRecord.SetInt64(0, association);
                    sqlDataRecord.SetInt64(1, role);
                    yield return sqlDataRecord;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}