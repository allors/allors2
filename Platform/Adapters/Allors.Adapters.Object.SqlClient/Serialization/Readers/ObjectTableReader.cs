// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectsTableReader.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Allors.Meta;

    internal class ObjectTableReader : IDataReader
    {
        private readonly IEnumerator<long> enumerator;

        private readonly Func<long, object>[] getValueFuncs;

        public ObjectTableReader(IClass @class, Mapping mapping, IEnumerable<long> objectIds, Dictionary<IRelationType, Dictionary<long, long>> associationIdByRoleIdByRelationTypeId, Dictionary<IRelationType, Dictionary<long, object>> roleByAssociationIdByRelationTypeId, DataColumnCollection columns)
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

            this.getValueFuncs = new Func<long, object>[columns.Count];
            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                var lowerCasedColumnName = column.ColumnName.ToLowerInvariant();

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

                        throw new Exception("Unhandled column " + column.ColumnName);
                }
            }

            this.FieldCount = columns.Count;
        }

        public int FieldCount { get; }
        
        public bool Read()
        {
            var result = this.enumerator.MoveNext();
            return result;
        }
        
        public object GetValue(int i)
        {
            var current = this.enumerator.Current;
            var getFunction = this.getValueFuncs[i];
            var value = getFunction(current);
            return value;
        }

        #region Not Supported
        public void Close()
        {
            throw new NotSupportedException();
        }

        public int Depth
        {
            get { throw new NotSupportedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException();
        }

        public string GetName(int i)
        {
            throw new NotSupportedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotSupportedException();
        }

        public bool IsClosed
        {
            get { throw new NotSupportedException(); }
        }

        public bool NextResult()
        {
            throw new NotSupportedException();
        }

        public int RecordsAffected
        {
            get { throw new NotSupportedException(); }
        }

        public void Dispose()
        {
            throw new NotSupportedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotSupportedException();
        }

        public byte GetByte(int i)
        {
            throw new NotSupportedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public char GetChar(int i)
        {
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotSupportedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotSupportedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotSupportedException();
        }

        public double GetDouble(int i)
        {
            throw new NotSupportedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotSupportedException();
        }

        public float GetFloat(int i)
        {
            throw new NotSupportedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotSupportedException();
        }

        public short GetInt16(int i)
        {
            throw new NotSupportedException();
        }

        public int GetInt32(int i)
        {
            throw new NotSupportedException();
        }

        public long GetInt64(int i)
        {
            throw new NotSupportedException();
        }

        public string GetString(int i)
        {
            throw new NotSupportedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotSupportedException();
        }

        public object this[string name]
        {
            get { throw new NotSupportedException(); }
        }

        public object this[int i]
        {
            get { throw new NotSupportedException(); }
        }
        #endregion
    }
}