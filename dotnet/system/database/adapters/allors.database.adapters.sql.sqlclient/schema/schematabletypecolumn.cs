// <copyright file="SchemaTableTypeColumn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    public class SchemaTableTypeColumn
    {
        private readonly int? scale;

        public SchemaTableTypeColumn(SchemaTableType table, string name, string dataType, int? maximumLength, int? precision, int? scale)
        {
            this.Table = table;
            this.Name = name;
            this.DataType = dataType;
            this.MaximumLength = maximumLength;
            this.Precision = precision;
            this.scale = scale;
        }

        public SchemaTableType Table { get; }

        public string Name { get; }

        public string DataType { get; }

        public string SqlType
        {
            get
            {
                if (this.DataType.Equals("nvarchar"))
                {
                    var length = this.MaximumLength == -1 ? "max" : this.MaximumLength.ToString();
                    return $"nvarchar({length})";
                }

                if (this.DataType.Equals("varbinary"))
                {
                    var length = this.MaximumLength == -1 ? "max" : this.MaximumLength.ToString();
                    return $"varbinary({length})";
                }

                if (this.DataType.Equals("decimal"))
                {
                    return $"decimal({this.Precision},{this.scale})";
                }

                return this.DataType;
            }
        }

        public int? MaximumLength { get; }

        public int? Precision { get; }

        public int? Scale => this.scale;

        public override string ToString() => this.Name;
    }
}
