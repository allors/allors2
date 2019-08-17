// <copyright file="RelationTableReader.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;

    internal class RelationTableReader : DbDataReader
    {
        private readonly IEnumerator<KeyValuePair<long, object>> enumerator;

        private readonly Func<KeyValuePair<long, object>, object>[] getValueFuncs;

        public RelationTableReader(Dictionary<long, object> roleByAssociationId, string[] columnNames)
        {
            this.enumerator = CreateEnumerator(roleByAssociationId);

            this.getValueFuncs = new Func<KeyValuePair<long, object>, object>[columnNames.Length];
            for (var i = 0; i < columnNames.Length; i++)
            {
                var columnName = columnNames[i];

                switch (columnName.ToLowerInvariant())
                {
                    case Mapping.ColumnNameForAssociation:
                        this.getValueFuncs[i] = current => current.Key;
                        break;
                    case Mapping.ColumnNameForRole:
                        this.getValueFuncs[i] = current => current.Value;
                        break;
                    default:
                        throw new Exception("Unknown column name " + columnName);
                }
            }
        }

        public override int FieldCount => 2;

        public override bool Read()
        {
            var result = this.enumerator.MoveNext();
            return result;
        }

        public override object GetValue(int i)
        {
            var current = this.enumerator.Current;
            return this.getValueFuncs[i](current);
        }

        private static IEnumerator<KeyValuePair<long, object>> CreateEnumerator(Dictionary<long, object> roleByAssociationId)
        {
            foreach (var pair in roleByAssociationId)
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
