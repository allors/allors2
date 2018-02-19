// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleContains.cs" company="Allors bvba">
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
//   Defines the AllorsPredicateRoleContainsSql type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using Allors.Meta;

    public sealed class RoleContains : Predicate
    {
        private readonly IObject allorsObject;
        private readonly IRoleType role;

        public RoleContains(ExtentFiltered extent, IRoleType role, IObject allorsObject)
        {
            extent.CheckRole(role);
            CompositePredicateAssertions.ValidateRoleContains(role, allorsObject);
            this.role = role;
            this.allorsObject = allorsObject;
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveClasses)
            {
                statement.Append("\n");
                statement.Append("EXISTS(\n");
                statement.Append("SELECT " + alias + "." + schema.ObjectId + "\n");
                statement.Append("FROM " + schema.Table(this.role) + "\n");
                statement.Append("WHERE   " + schema.AssociationId + "=" + alias + "." + schema.ObjectId + "\n");
                statement.Append("AND " + schema.RoleId + "=" + this.allorsObject.Strategy.ObjectId + "\n");
                statement.Append(")\n");
            }
            else
            {
                var compositeType = (IComposite)this.role.ObjectType;

                // This join is not possible because of the 3VL (Three Valued Logic).
                // It should work for normal queries, but it fails when wrapped in a NOT( ... ) predicate.
                // The rows with NULL values should then return TRUE and not UNKNOWN.
                //
                // statement.sql.Append(" " + role.SingularPropertyName + "_R." + schema.O + " = " + allorsObject.ObjectId);

                statement.Append("\n");
                statement.Append("EXISTS(\n");
                statement.Append("SELECT " + schema.ObjectId + "\n");
                statement.Append("FROM " + schema.Table(compositeType.ExclusiveClass) + "\n");
                statement.Append("WHERE " + schema.ObjectId + "=" + this.allorsObject.Strategy.ObjectId + "\n");
                statement.Append("AND " + schema.Column(this.role.AssociationType) + "=" + alias + ".O\n");
                statement.Append(")\n");
            }

            return Include;
        }

        public override void Setup(ExtentStatement statement)
        {
            //extent.UseRole(role);
        }
    }
}