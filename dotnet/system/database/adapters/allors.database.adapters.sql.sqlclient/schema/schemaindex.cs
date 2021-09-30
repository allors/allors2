// <copyright file="SchemaIndex.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    public class SchemaIndex
    {
        public SchemaIndex(Schema schema, string name)
        {
            this.Schema = schema;
            this.Name = name;
            this.LowercaseName = name.ToLowerInvariant();
        }

        public string Name { get; }

        public string LowercaseName { get; }

        public Schema Schema { get; }

        public override string ToString() => this.Name;
    }
}
