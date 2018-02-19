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

namespace Allors.Adapters.Database.Sql
{
    using Meta;

    public sealed class RoleContainedInExtent : In
    {
        private readonly SqlExtent inExtent;
        private readonly IRoleType role;

        public RoleContainedInExtent(ExtentFiltered extent, IRoleType role, Allors.Extent inExtent)
        {
            extent.CheckRole(role);
            CompositePredicateAssertions.ValidateRoleContainedIn(role, inExtent);
            this.role = role;
            this.inExtent = ((Extent)inExtent).ContainedInExtent;
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            var inStatement = statement.CreateChild(this.inExtent, this.role);

            inStatement.UseAssociation(this.role.AssociationType);

            if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveClasses)
            {
                statement.Append(" (" + this.role.SingularPropertyName + "_R." + schema.RoleId + " IS NOT NULL AND ");
                statement.Append(" " + this.role.SingularPropertyName + "_R." + schema.AssociationId + " IN (");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))");
            }
            else
            {
                if (this.role.IsMany)
                {
                    statement.Append(" (" + this.role.SingularPropertyName + "_R." + schema.ObjectId + " IS NOT NULL AND ");
                    statement.Append(" " + this.role.SingularPropertyName + "_R." + schema.ObjectId + " IN (");
                    this.inExtent.BuildSql(inStatement);
                    statement.Append(" ))");
                }
                else
                {
                    statement.Append(" (" + schema.Column(this.role) + " IS NOT NULL AND ");
                    statement.Append(" " + schema.Column(this.role) + " IN (");
                    this.inExtent.BuildSql(inStatement);
                    statement.Append(" ))");
                }
            }

            return this.Include;
        }

        public override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
        }
    }
}