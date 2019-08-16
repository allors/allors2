
// <copyright file="ObjectTableReader.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;
    using System.Data.Common;

    internal class ObjectTableReader : DbDataReader
    {
        private readonly IEnumerator<long> enumerator;

        private readonly Func<long, object>[] getValueFuncs;

        public ObjectTableReader(IClass @class, Mapping mapping, IEnumerable<long> objectIds, Dictionary<IRelationType, Dictionary<long, long>> associationIdByRoleIdByRelationTypeId, Dictionary<IRelationType, Dictionary<long, object>> roleByAssociationIdByRelationTypeId, string[] columnNames)
        {
            this.enumerator = objectIds.GetEnumerator();

            var associationTypeByLowerCasedFieldName = new Dictionary<string, IAssociationType>();
            foreach (var associationType in @class.AssociationTypes)
            {
                var relationType = associationType.RelationType;
                var roleType = relationType.RoleType;
                if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && roleType.IsMany)
                {
                    var fieldName = mapping.UnescapedColumnNameByRelationType[relationType].ToLowerInvariant();
                    associationTypeByLowerCasedFieldName.Add(fieldName, associationType);
                }
            }

            var roleTypeByLowerCasedFieldName = new Dictionary<string, IRoleType>();
            foreach (var roleType in @class.RoleTypes)
            {
                var relationType = roleType.RelationType;
                var associationType = relationType.AssociationType;

                if (roleType.ObjectType.IsUnit)
                {
                    var fieldName = mapping.UnescapedColumnNameByRelationType[relationType].ToLowerInvariant();
                    roleTypeByLowerCasedFieldName.Add(fieldName, roleType);
                }
                else
                {
                    if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses && !roleType.IsMany)
                    {
                        var fieldName = mapping.UnescapedColumnNameByRelationType[relationType].ToLowerInvariant();
                        roleTypeByLowerCasedFieldName.Add(fieldName, roleType);
                    }
                }
            }

            this.getValueFuncs = new Func<long, object>[columnNames.Length];
            for (var i = 0; i < columnNames.Length; i++)
            {
                var columnName = columnNames[i];

                var lowerCasedColumnName = columnName.ToLowerInvariant();

                switch (lowerCasedColumnName)
                {
                    case Mapping.ColumnNameForObject:
                        this.getValueFuncs[i] = current => current;
                        break;

                    case Mapping.ColumnNameForClass:
                        this.getValueFuncs[i] = current => @class.Id;
                        break;

                    default:
                        IAssociationType associationType;
                        if (associationTypeByLowerCasedFieldName.TryGetValue(lowerCasedColumnName, out associationType))
                        {
                            Dictionary<long, long> associationIdByRoleId;
                            if (associationIdByRoleIdByRelationTypeId.TryGetValue(associationType.RelationType, out associationIdByRoleId))
                            {
                                this.getValueFuncs[i] = current =>
                                {
                                    long association;
                                    if (associationIdByRoleId.TryGetValue(current, out association))
                                    {
                                        return association;
                                    }

                                    return null;
                                };
                            }
                            else
                            {
                                this.getValueFuncs[i] = current => null;
                            }

                            continue;
                        }

                        IRoleType roleType;
                        if (roleTypeByLowerCasedFieldName.TryGetValue(lowerCasedColumnName, out roleType))
                        {
                            Dictionary<long, object> roleByAssociationId;
                            if (roleByAssociationIdByRelationTypeId.TryGetValue(roleType.RelationType, out roleByAssociationId))
                            {
                                this.getValueFuncs[i] = current =>
                                {
                                    object role;
                                    if (roleByAssociationId.TryGetValue(current, out role))
                                    {
                                        return role;
                                    }

                                    return null;
                                };
                            }
                            else
                            {
                                this.getValueFuncs[i] = current => null;
                            }

                            continue;
                        }

                        throw new Exception("Unhandled column " + columnName);
                }
            }

            this.FieldCount = columnNames.Length;
        }

        public override int FieldCount { get; }

        public override bool Read()
        {
            var result = this.enumerator.MoveNext();
            return result;
        }

        public override object GetValue(int i)
        {
            var current = this.enumerator.Current;
            var getFunction = this.getValueFuncs[i];
            var value = getFunction(current);
            return value;
        }

        #region Not Supported

        public override bool GetBoolean(int ordinal) => throw new NotImplementedException();

        public override byte GetByte(int ordinal) => throw new NotImplementedException();

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => throw new NotImplementedException();

        public override char GetChar(int ordinal) => throw new NotImplementedException();

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => throw new NotImplementedException();

        public override string GetDataTypeName(int ordinal) => throw new NotImplementedException();

        public override DateTime GetDateTime(int ordinal) => throw new NotImplementedException();

        public override decimal GetDecimal(int ordinal) => throw new NotImplementedException();

        public override double GetDouble(int ordinal) => throw new NotImplementedException();

        public override IEnumerator GetEnumerator() => throw new NotImplementedException();

        public override Type GetFieldType(int ordinal) => throw new NotImplementedException();

        public override float GetFloat(int ordinal) => throw new NotImplementedException();

        public override Guid GetGuid(int ordinal) => throw new NotImplementedException();

        public override short GetInt16(int ordinal) => throw new NotImplementedException();

        public override int GetInt32(int ordinal) => throw new NotImplementedException();

        public override long GetInt64(int ordinal) => throw new NotImplementedException();

        public override string GetName(int ordinal) => throw new NotImplementedException();

        public override int GetOrdinal(string name) => throw new NotImplementedException();

        public override string GetString(int ordinal) => throw new NotImplementedException();

        public override int GetValues(object[] values) => throw new NotImplementedException();

        public override bool IsDBNull(int ordinal) => throw new NotImplementedException();

        public override bool NextResult() => throw new NotImplementedException();

        public override int Depth { get; }

        public override bool HasRows { get; }

        public override bool IsClosed { get; }

        public override object this[int ordinal] => throw new NotImplementedException();

        public override object this[string name] => throw new NotImplementedException();

        public override int RecordsAffected { get; }
        #endregion
    }
}
