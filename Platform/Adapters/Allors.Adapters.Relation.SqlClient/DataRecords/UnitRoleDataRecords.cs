// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitRoleDataRecords.cs" company="Allors bvba">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;

    using Microsoft.SqlServer.Server;

    public class UnitRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;

        private readonly IRoleType roleType;
        private readonly IList<long> associations;
        private readonly Dictionary<long, object> roleByAssociation;

        public UnitRoleDataRecords(Mapping mapping, IRoleType roleType, IList<long> associations, Dictionary<long, object> roleByAssociation)
        {
            this.mapping = mapping;
            this.roleType = roleType;
            this.associations = associations;
            this.roleByAssociation = roleByAssociation;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var sqlDataRecord = new SqlDataRecord(this.mapping.GetSqlMetaData(this.roleType));
            var unitTypeTag = ((IUnit)this.roleType.ObjectType).UnitTag;

            switch (unitTypeTag)
            {
                case UnitTags.Binary:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetValue(1, this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;

                case UnitTags.Boolean:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetBoolean(1, (bool)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;
                case UnitTags.DateTime:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetDateTime(1, (DateTime)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;
                case UnitTags.Decimal:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetDecimal(1, (decimal)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;
                case UnitTags.Float:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetDouble(1, (double)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;
                case UnitTags.Integer:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetInt32(1, (int)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;
                case UnitTags.String:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetString(1, (string)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;

                case UnitTags.Unique:
                    foreach (var association in this.associations)
                    {
                        sqlDataRecord.SetInt64(0, association);
                        sqlDataRecord.SetGuid(1, (Guid)this.roleByAssociation[association]);
                        yield return sqlDataRecord;
                    }

                    break;
                default:
                    throw new NotSupportedException("Unit type tag " + unitTypeTag + " is not supported.");
            }
    }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}