//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleContainsSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsPredicateRoleContainsSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SqlClient
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateRoleContainsSql : AllorsPredicateSql
    {
        private readonly IObject allorsObject;
        private readonly IRoleType role;

        internal AllorsPredicateRoleContainsSql(AllorsExtentFilteredSql extent, IRoleType role, IObject allorsObject)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContains(role, allorsObject);
            this.role = role;
            this.allorsObject = allorsObject;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append("\n");
            statement.Append("EXISTS(\n");
            statement.Append("SELECT " + Mapping.ColumnNameForObject + "\n");
            statement.Append("FROM " + extent.Session.Database.SchemaName + "." + mapping.GetTableName(this.role) + "\n");
            statement.Append("WHERE " + Mapping.ColumnNameForAssociation + "=" + alias + "." + Mapping.ColumnNameForObject + "\n");
            statement.Append("AND " + Mapping.ColumnNameForRole + "=" + this.allorsObject.Strategy.ObjectId + "\n");
            statement.Append(")\n");
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
        }
    }
}