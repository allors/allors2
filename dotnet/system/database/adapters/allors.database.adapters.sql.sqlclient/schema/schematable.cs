// <copyright file="SchemaTable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Collections.Generic;

    public class SchemaTable
    {
        public SchemaTable(Schema schema, string name)
        {
            this.Schema = schema;
            this.Name = name;

            this.ColumnByLowercaseColumnName = new Dictionary<string, SchemaTableColumn>();
        }

        public string Name { get; }

        public Dictionary<string, SchemaTableColumn> ColumnByLowercaseColumnName { get; }

        public Schema Schema { get; }

        public SchemaTableColumn GetColumn(string columnName)
        {
            this.ColumnByLowercaseColumnName.TryGetValue(columnName.ToLowerInvariant(), out var tableColumn);
            return tableColumn;
        }

        public override string ToString() => this.Name;
    }
}
