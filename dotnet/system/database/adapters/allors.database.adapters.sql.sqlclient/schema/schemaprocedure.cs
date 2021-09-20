// <copyright file="SchemaProcedure.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Linq;

    public class SchemaProcedure
    {
        public SchemaProcedure(Schema schema, string name, string definition)
        {
            this.Name = name;
            this.Definition = definition;
        }

        public string Name { get; }

        public string Definition { get; }

        public override string ToString() => this.Name;

        public bool IsDefinitionCompatible(string existingDefinition) => this.RemoveWhitespace(this.Definition).Equals(this.RemoveWhitespace(existingDefinition));

        private string RemoveWhitespace(string input) => new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}
