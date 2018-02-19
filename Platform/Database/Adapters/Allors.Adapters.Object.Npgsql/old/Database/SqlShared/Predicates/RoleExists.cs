// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleExists.cs" company="Allors bvba">
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
    using Allors.Meta;

    public sealed class RoleExists : Predicate
    {
        private readonly IRoleType role;

        public RoleExists(ExtentFiltered extent, IRoleType role)
        {
            extent.CheckRole(role);
            CompositePredicateAssertions.ValidateRoleExists(role);
            this.role = role;
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            if (this.role.ObjectType is IUnit)
            {
                statement.Append(" " + alias + "." + schema.Column(this.role) + " IS NOT NULL");
            }
            else
            {
                if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveClasses)
                {
                    statement.Append(" " + this.role.SingularPropertyName + "_R." + schema.RoleId + " IS NOT NULL");
                }
                else
                {
                    if (this.role.IsMany)
                    {
                        statement.Append(" " + this.role.SingularPropertyName + "_R." + schema.ObjectId + " IS NOT NULL");
                    }
                    else
                    {
                        statement.Append(" " + alias + "." + schema.Column(this.role) + " IS NOT NULL");
                    }
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