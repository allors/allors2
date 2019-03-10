// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaColumn.cs" company="Allors bvba">
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
    using System.Data;

    using Allors.Meta;

    public class SchemaColumn
    {
        public readonly string Name;
        public readonly DbType DbType;

        public readonly bool IsIdentity;
        public readonly bool IsKey;
        public readonly SchemaIndexType IndexType;

        public readonly SchemaParameter Param;
        public readonly string StatementName;

        public readonly int? Precision;
        public readonly int? Scale;
        public readonly int? Size;

        public readonly IRelationType RelationType;

        public SchemaColumn(Schema schema, string name, DbType dbType, bool isIdentity, bool isKey, SchemaIndexType indexType)
            : this(schema, name, dbType, isIdentity, isKey, indexType, null, null, null, null)
        {
        }

        public SchemaColumn(Schema schema, string name, DbType dbType, bool isIdentity, bool isKey, SchemaIndexType indexType, IRelationType relationType)
            : this(schema, name, dbType, isIdentity, isKey, indexType, relationType, null, null, null)
        {
        }

        public SchemaColumn(Schema schema, string name, DbType dbType, bool isIdentity, bool isKey, SchemaIndexType indexType, IRelationType relationType, int? size, int? precision, int? scale)
        {
            this.Name = name.ToLowerInvariant();
            this.StatementName = schema.EscapeIfReserved(this.Name);
            this.Param = schema.CreateParameter(name, dbType);

            this.IsKey = isKey;
            this.IsIdentity = isIdentity;
            this.IndexType = indexType;

            this.Size = size;
            this.Precision = precision;
            this.Scale = scale;

            this.DbType = dbType;

            this.RelationType = relationType;
        }

        /// <summary>
        /// The string value.
        /// </summary>
        /// <returns>The string value</returns>
        public override string ToString()
        {
            return this.StatementName;
        }
    }
}