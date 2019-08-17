// <copyright file="SchemaTableType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    using System.Collections.Generic;

    public class SchemaTableType
    {
        private readonly Dictionary<string, SchemaTableTypeColumn> columnByLowercaseColumnName;

        public SchemaTableType(Schema schema, string name)
        {
            this.Name = name;
            this.columnByLowercaseColumnName = new Dictionary<string, SchemaTableTypeColumn>();
        }

        public string Name { get; }

        public Dictionary<string, SchemaTableTypeColumn> ColumnByLowercaseColumnName => this.columnByLowercaseColumnName;

        public override string ToString() => this.Name;

        public SchemaTableTypeColumn GetColumn(string columnName)
        {
            this.columnByLowercaseColumnName.TryGetValue(columnName.ToLowerInvariant(), out var tableColumn);
            return tableColumn;
        }
    }
}
