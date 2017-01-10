// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleContainedInExtent.cs" company="Allors bvba">
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

using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    using Adapters;

    using Meta;

    internal sealed class AllorsPredicateRoleInExtentSql : AllorsPredicateSql
    {
        private readonly AllorsExtentSql inExtent;
        private readonly IRoleType role;

        public AllorsPredicateRoleInExtentSql(AllorsExtentFilteredSql extent, IRoleType role, Allors.Extent inExtent)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContainedIn(role, inExtent);
            this.role = role;
            this.inExtent = (AllorsExtentSql)inExtent;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            var inStatement = statement.CreateChild(this.inExtent, this.role);

            inStatement.UseAssociation(this.role.AssociationType);

            statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
            statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForAssociation + " IN (");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" ))");

            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
        }
    }
}