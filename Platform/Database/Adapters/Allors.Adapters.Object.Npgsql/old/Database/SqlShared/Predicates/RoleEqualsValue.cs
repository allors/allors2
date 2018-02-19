// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleEqualsValue.cs" company="Allors bvba">
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
    using System;

    using Allors.Meta;

    public sealed class RoleEqualsValue : Predicate
    {
        private readonly object obj;
        private readonly IRoleType roleType;

        public RoleEqualsValue(ExtentFiltered extent, IRoleType roleType, object obj)
        {
            extent.CheckRole(roleType);
            CompositePredicateAssertions.ValidateRoleEquals(roleType, obj);
            this.roleType = roleType;
            if (obj is Enum)
            {
                var unitType = roleType.ObjectType as IUnit;
                if (unitType != null && unitType.IsInteger)
                {
                    this.obj = (int)obj;
                }
                else
                {
                    throw new Exception("Role Object Type " + roleType.ObjectType.SingularName + " doesn't support enumerations.");
                }
            }
            else
            {
                this.obj = roleType.ObjectType is IUnit ? extent.Session.SqlDatabase.Internalize(obj, roleType) : obj;
            }
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            if (this.roleType.ObjectType is IUnit)
            {
                statement.Append(" " + alias + "." + schema.Column(this.roleType) + "=" + statement.AddParameter(this.obj));
            }
            else
            {
                var allorsObject = (IObject)this.obj;

                if (this.roleType.RelationType.ExistExclusiveClasses)
                {
                    statement.Append(" (" + alias + "." + schema.Column(this.roleType) + " IS NOT NULL AND ");
                    statement.Append(" " + alias + "." + schema.Column(this.roleType) + "=" + allorsObject.Strategy.ObjectId + ")");
                }
                else
                {
                    statement.Append(" (" + this.roleType.SingularPropertyName + "_R." + schema.RoleId + " IS NOT NULL AND ");
                    statement.Append(" " + this.roleType.SingularPropertyName + "_R." + schema.RoleId + "=" + allorsObject.Strategy.ObjectId + ")");
                }
            }

            return this.Include;
        }

        public override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.roleType);
        }
    }
}