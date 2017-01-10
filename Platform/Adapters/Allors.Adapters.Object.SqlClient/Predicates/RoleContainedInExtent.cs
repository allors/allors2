// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleContainedInExtent.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Allors.Adapters.Object.SqlClient
{
    using Adapters;

    using Meta;

    internal sealed class RoleContainedInExtent : In
    {
        private readonly SqlExtent inExtent;
        private readonly IRoleType role;

        internal RoleContainedInExtent(ExtentFiltered extent, IRoleType role, Allors.Extent inExtent)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContainedIn(role, inExtent);
            this.role = role;
            this.inExtent = ((Extent)inExtent).ContainedInExtent;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            var inStatement = statement.CreateChild(this.inExtent, this.role);

            inStatement.UseAssociation(this.role.AssociationType);

            if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveClasses)
            {
                statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForAssociation + " IN (");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))");
            }
            else
            {
                if (this.role.IsMany)
                {
                    statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
                    statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IN (");
                    this.inExtent.BuildSql(inStatement);
                    statement.Append(" ))");
                }
                else
                {
                    statement.Append(" (" + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NOT NULL AND ");
                    statement.Append(" " + schema.ColumnNameByRelationType[this.role.RelationType] + " IN (");
                    this.inExtent.BuildSql(inStatement);
                    statement.Append(" ))");
                }
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
        }
    }
}