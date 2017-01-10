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


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Allors.Adapters.Object.SqlClient
{

    internal class RelationTableReader : IDataReader
    {
        private readonly IEnumerator<KeyValuePair<long, object>> enumerator;

        private readonly Func<KeyValuePair<long, object>, object>[] getValueFuncs;

        public RelationTableReader(Dictionary<long, object> roleByAssociationId, DataColumnCollection columns)
        {
            this.enumerator = CreateEnumerator(roleByAssociationId);

            this.getValueFuncs = new Func<KeyValuePair<long, object>, object>[columns.Count];
            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                switch (column.ColumnName.ToLowerInvariant())
                {
                    case Mapping.ColumnNameForAssociation:
                        this.getValueFuncs[i] = current => current.Key;
                        break;
                    case Mapping.ColumnNameForRole:
                        this.getValueFuncs[i] = current => current.Value;
                        break;
                }
            }
        }
        
        public int FieldCount => 2;

        public void InitMappings(SqlBulkCopy sqlBulkCopy)
        {
            sqlBulkCopy.ColumnMappings.Add(Mapping.ColumnNameForAssociation, 0);
            sqlBulkCopy.ColumnMappings.Add(Mapping.ColumnNameForRole, 1);
        }

        public bool Read()
        {
            var result = this.enumerator.MoveNext();
            return result;
        }

        public int GetOrdinal(string name)
        {
            switch (name)
            {
                case Mapping.ColumnNameForAssociation:
                    return 0;
                case Mapping.ColumnNameForRole:
                    return 1;
                default:
                    return -1;
            }
        }

        public object GetValue(int i)
        {
            var current = this.enumerator.Current;
            return this.getValueFuncs[i](current);
        }

        private static IEnumerator<KeyValuePair<long, object>> CreateEnumerator(Dictionary<long, object> roleByAssociationId)
        {
            foreach(var pair in roleByAssociationId)
            {
                var assciationId = pair.Key;
                var role = pair.Value;

                var roleIds = role as long[];
                if (roleIds != null)
                {
                    foreach (var roleId in roleIds)
                    {
                        yield return new KeyValuePair<long, object>(assciationId, roleId);
                    }
                }
                else
                {
                    yield return pair;
                }
            }
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

        public string GetName(int i)
        {
            throw new NotSupportedException();
        }

        public DataTable GetSchemaTable()
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