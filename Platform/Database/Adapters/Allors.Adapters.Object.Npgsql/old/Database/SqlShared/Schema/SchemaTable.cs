// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaTable.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;

    public sealed class SchemaTable : IEnumerable<SchemaColumn>
    {
        public readonly SchemaTableKind Kind;
        public readonly string Name;
        public readonly IRelationType RelationType;
        public readonly Schema Schema;
        public readonly IObjectType ObjectType;
        public readonly string StatementName;

        private readonly Dictionary<string, SchemaColumn> columnsByName;

        public SchemaTable(Schema schema, SchemaTableKind kind, IObjectType objectType)
            : this(schema, kind, objectType.SingularName)
        {
            this.ObjectType = objectType;
        }

        public SchemaTable(Schema schema, SchemaTableKind kind, IRelationType relationType)
            : this(schema, kind, relationType.AssociationType.SingularName + relationType.RoleType.SingularName)
        {
            this.RelationType = relationType;
        }

        public SchemaTable(Schema schema, SchemaTableKind kind, string name)
        {
            this.Schema = schema;
            this.Name = name.ToLowerInvariant();
            this.StatementName = schema.EscapeIfReserved(this.Name);
            this.Kind = kind;

            this.columnsByName = new Dictionary<string, SchemaColumn>();
        }

        public SchemaColumn FirstKeyColumn
        {
            get
            {
                foreach (var dictionaryEntry in this.columnsByName)
                {
                    var column = dictionaryEntry.Value;
                    if (column.IsKey)
                    {
                        return column;
                    }
                }

                return null;
            }
        }

        public SchemaColumn this[string columnName]
        {
            get { return this.columnsByName[columnName.ToLowerInvariant()]; }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<SchemaColumn>)this).GetEnumerator();
        }

        IEnumerator<SchemaColumn> IEnumerable<SchemaColumn>.GetEnumerator()
        {
            return this.columnsByName.Values.GetEnumerator();
        }

        /// <summary>
        /// The string value.
        /// </summary>
        /// <returns>The string value</returns>
        public override string ToString()
        {
            return this.StatementName;
        }

        public void AddColumn(SchemaColumn column)
        {
            this.columnsByName[column.Name] = column;
        }

        public bool Contains(string columName)
        {
            return this.columnsByName.ContainsKey(columName.ToLowerInvariant());
        }
    }
}