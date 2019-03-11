// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleEqualsRole.cs" company="Allors bvba">
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
// <summary>
//   Defines the AllorsPredicateRoleEqualsRoleSql type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System;

    using Allors.Meta;

    public sealed class RoleEqualsRole : Predicate
    {
        private readonly IRoleType equalsRole;
        private readonly IRoleType role;

        public RoleEqualsRole(ExtentFiltered extent, IRoleType role, IRoleType equalsRole)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleEquals(role, equalsRole);
            this.role = role;
            this.equalsRole = equalsRole;
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            if (this.role.ObjectType is IUnit && this.equalsRole.ObjectType is IUnit)
            {
                statement.Append(" " + alias + "." + schema.Column(this.role) + "=" + alias + "." + schema.Column(this.equalsRole));
            }
            else
            {
                var roleCompositeType = this.role.ObjectType as IComposite;
                var equalsRoleCompositeType = this.equalsRole.ObjectType as IComposite;

                if (roleCompositeType != null && roleCompositeType.ExclusiveClass != null && 
                    equalsRoleCompositeType != null && equalsRoleCompositeType.ExclusiveClass != null)
                {
                    statement.Append(" " + alias + "." + schema.Column(this.role) + "=" + alias + "." + schema.Column(this.equalsRole));
                }
                
                throw new NotImplementedException();
            }

            return this.Include;
        }

        public override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.equalsRole);
        }
    }
}