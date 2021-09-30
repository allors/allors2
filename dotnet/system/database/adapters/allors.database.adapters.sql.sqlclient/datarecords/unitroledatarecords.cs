// <copyright file="UnitRoleDataRecords.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Meta;
    using Microsoft.Data.SqlClient.Server;

    internal class UnitRoleDataRecords : IEnumerable<SqlDataRecord>
    {
        private readonly Mapping mapping;
        private readonly IRoleType roleType;
        private readonly IEnumerable<UnitRelation> relations;

        internal UnitRoleDataRecords(Mapping mapping, IRoleType roleType, IEnumerable<UnitRelation> relations)
        {
            this.mapping = mapping;
            this.roleType = roleType;
            this.relations = relations;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var metaData = new[]
                                {
                                    new SqlMetaData(this.mapping.TableTypeColumnNameForAssociation, SqlDbType.BigInt),
                                    this.GetSqlMetaData(this.mapping.TableTypeColumnNameForRole, this.roleType),
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

        private SqlMetaData GetSqlMetaData(string name, IRoleType roleType)
        {
            var unit = (IUnit)roleType.ObjectType;
            switch (unit.Tag)
            {
                case UnitTags.String:
                    if (roleType.Size == -1 || roleType.Size > 4000)
                    {
                        return new SqlMetaData(name, SqlDbType.NVarChar, -1);
                    }

                    return new SqlMetaData(name, SqlDbType.NVarChar, roleType.Size.Value);

                case UnitTags.Integer:
                    return new SqlMetaData(name, SqlDbType.Int);

                case UnitTags.Decimal:
                    return new SqlMetaData(name, SqlDbType.Decimal, (byte)roleType.Precision.Value, (byte)roleType.Scale.Value);

                case UnitTags.Float:
                    return new SqlMetaData(name, SqlDbType.Float);

                case UnitTags.Boolean:
                    return new SqlMetaData(name, SqlDbType.Bit);

                case UnitTags.DateTime:
                    return new SqlMetaData(name, SqlDbType.DateTime2);

                case UnitTags.Unique:
                    return new SqlMetaData(name, SqlDbType.UniqueIdentifier);

                case UnitTags.Binary:
                    if (roleType.Size == -1 || roleType.Size > 8000)
                    {
                        return new SqlMetaData(name, SqlDbType.VarBinary, -1);
                    }

                    return new SqlMetaData(name, SqlDbType.VarBinary, (long)roleType.Size);

                default:
                    throw new Exception("!UNKNOWN VALUE TYPE!");
            }
        }

    }
}
