// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectsTableReader.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;

    using Allors.Meta;

    internal class ObjectsTableReader : DbDataReader
    {
        private readonly IEnumerator<KeyValuePair<long, IObjectType>> enumerator;

        private readonly Func<KeyValuePair<long, IObjectType>, object>[] getValueFuncs;

        internal ObjectsTableReader(Dictionary<long, IObjectType> objectTypeByObjectId, Dictionary<long, long> objectVersionByObjectId, string[] columnNames)
        {
            this.enumerator = objectTypeByObjectId.GetEnumerator();

            this.getValueFuncs = new Func<KeyValuePair<long, IObjectType>, object>[columnNames.Length];
            for (var i = 0; i < columnNames.Length; i++)
            {
                var columnName = columnNames[i];

                switch (columnName.ToLowerInvariant())
                {
                    case Mapping.ColumnNameForObject:
                        this.getValueFuncs[i] = current => current.Key;
                        break;
                    case Mapping.ColumnNameForClass:
                        this.getValueFuncs[i] = current => current.Value.Id;
                        break;
                    case Mapping.ColumnNameForVersion:
                        this.getValueFuncs[i] = current => objectVersionByObjectId[current.Key];
                        break;
                    default:
                        throw new Exception("Unknown column name " + columnName);
                }
            }
        }

        public override int FieldCount => 3;

        public override bool Read()
        {
            var result = this.enumerator.MoveNext();
            return result;
        }

        public override object GetValue(int i)
        {
            var current = this.enumerator.Current;
            var getValueFunc = this.getValueFuncs[i];
            var value = getValueFunc(current);
            return value;
        }

        #region Not Supported

        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override int Depth { get; }
        public override bool HasRows { get; }
        public override bool IsClosed { get; }

        public override object this[int ordinal]
        {
            get { throw new NotImplementedException(); }
        }

        public override object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public override int RecordsAffected { get; }

        #endregion
    }
}