// <copyright file="SchemaProcedure.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SchemaProcedure
    {
        private static readonly Regex BodyRegex = new Regex(@"\$\$([\s\S]*)\$\$", RegexOptions.Compiled);

        public SchemaProcedure(string name, string definition)
        {
            this.Name = name;
            this.Definition = definition;
        }

        public string Name { get; }

        public string Definition { get; }

        public override string ToString() => this.Name;

        public bool IsDefinitionCompatible(string existingDefinition)
        {
            var match = BodyRegex.Match(existingDefinition);
            if (!match.Success)
            {
                return false;
            }

            var body = match.Groups[1].Value;
            return this.RemoveWhitespace(this.Definition).Equals(this.RemoveWhitespace(body));
        }

        private string RemoveWhitespace(string input) => new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}
