// <copyright file="ObjectsReader.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class ObjectsReader : IDataReader
    {
        private readonly IEnumerator<object[]> enumerable;

        private object[] xmlObject;

        public ObjectsReader(Objects xmlObjects) => this.enumerable = xmlObjects.GetEnumerator();

        public int FieldCount => 3;

        public bool Read()
        {
            while (this.enumerable.MoveNext())
            {
                this.xmlObject = this.enumerable.Current;
                return true;
            }

            this.xmlObject = null;
            return false;
        }

        public object GetValue(int i) => this.xmlObject[i];

        public void Dispose()
        {
        }

        #region Not Implemented

        public int Depth { get; }

        public bool IsClosed { get; }

        public int RecordsAffected { get; }

        public object this[int i] => throw new NotImplementedException();

        public object this[string name] => throw new NotImplementedException();

        public bool GetBoolean(int i) => throw new NotImplementedException();

        public byte GetByte(int i) => throw new NotImplementedException();

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

        public char GetChar(int i) => throw new NotImplementedException();

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

        public IDataReader GetData(int i) => throw new NotImplementedException();

        public string GetDataTypeName(int i) => throw new NotImplementedException();

        public DateTime GetDateTime(int i) => throw new NotImplementedException();

        public decimal GetDecimal(int i) => throw new NotImplementedException();

        public double GetDouble(int i) => throw new NotImplementedException();

        public Type GetFieldType(int i) => throw new NotImplementedException();

        public float GetFloat(int i) => throw new NotImplementedException();

        public Guid GetGuid(int i) => throw new NotImplementedException();

        public short GetInt16(int i) => throw new NotImplementedException();

        public int GetInt32(int i) => throw new NotImplementedException();

        public long GetInt64(int i) => throw new NotImplementedException();

        public string GetName(int i) => throw new NotImplementedException();

        public int GetOrdinal(string name) => throw new NotImplementedException();

        public string GetString(int i) => throw new NotImplementedException();

        public int GetValues(object[] values) => throw new NotImplementedException();

        public bool IsDBNull(int i) => throw new NotImplementedException();

        public void Close() => throw new NotImplementedException();

        public DataTable GetSchemaTable() => throw new NotImplementedException();

        public bool NextResult() => throw new NotImplementedException();

        #endregion Not Implemented
    }
}
