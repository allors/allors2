// <copyright file="SchemaTableColumn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    public class SchemaTableColumn
    {
        private readonly int? numericScale;

        public SchemaTableColumn(SchemaTable table, string name, string dataType, int? characterMaximumLength, int? numericPrecision, int? numericScale)
        {
            this.Table = table;
            this.Name = name;
            this.LowercaseName = name.ToLowerInvariant();
            this.DataType = dataType;
            this.CharacterMaximumLength = characterMaximumLength;
            this.NumericPrecision = numericPrecision;
            this.numericScale = numericScale;
        }

        public SchemaTable Table { get; }

        public string Name { get; }

        public string LowercaseName { get; }

        public string DataType { get; }

        public string SqlType
        {
            get
            {
                if (this.DataType.Equals("nvarchar"))
                {
                    var length = this.CharacterMaximumLength == -1 ? "max" : this.CharacterMaximumLength.ToString();
                    return $"nvarchar({length})";
                }

                if (this.DataType.Equals("varbinary"))
                {
                    var length = this.CharacterMaximumLength == -1 ? "max" : this.CharacterMaximumLength.ToString();
                    return $"varbinary({length})";
                }

                if (this.DataType.Equals("decimal"))
                {
                    return $"decimal({this.NumericPrecision},{this.numericScale})";
                }

                return this.DataType;
            }
        }

        public int? CharacterMaximumLength { get; }

        public int? NumericPrecision { get; }

        public int? NumericScale => this.numericScale;

        public override string ToString() => this.Name;
    }
}
