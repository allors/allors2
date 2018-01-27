// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationContains.cs" company="Allors bvba">
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

    public sealed class AssociationContains : Predicate
    {
        private readonly IObject allorsObject;
        private readonly IAssociationType association;

        public AssociationContains(ExtentFiltered extent, IAssociationType association, IObject allorsObject)
        {
            extent.CheckAssociation(association);
            CompositePredicateAssertions.AssertAssociationContains(association, allorsObject);
            this.association = association;
            this.allorsObject = allorsObject;
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveLeafClasses)
            {
                statement.Append("\n");
                statement.Append("EXISTS(\n");
                statement.Append("SELECT " + alias + "." + schema.ObjectId + "\n");
                statement.Append("FROM " + schema.Table(this.association) + "\n");
                statement.Append("WHERE " + schema.AssociationId + "=" + this.allorsObject.Strategy.ObjectId + "\n");
                statement.Append("AND " + schema.RoleId + "=" + alias + "." + schema.ObjectId + "\n");
                statement.Append(")");
            }
            else
            {
                statement.Append(" " + this.association.SingularName + "_A." + schema.ObjectId + " = " + this.allorsObject.Strategy.ObjectId);
            }

            return this.Include;
        }

        public override void Setup(ExtentStatement statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}