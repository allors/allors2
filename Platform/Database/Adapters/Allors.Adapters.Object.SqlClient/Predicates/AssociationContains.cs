// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationContains.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Object.SqlClient
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationContains : Predicate
    {
        private readonly IObject allorsObject;
        private readonly IAssociationType association;

        internal AssociationContains(ExtentFiltered extent, IAssociationType association, IObject allorsObject)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContains(association, allorsObject);
            this.association = association;
            this.allorsObject = allorsObject;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveClasses)
            {
                statement.Append("\n");
                statement.Append("EXISTS(\n");
                statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject + "\n");
                statement.Append("FROM " + schema.TableNameForRelationByRelationType[this.association.RelationType] + "\n");
                statement.Append("WHERE " + Mapping.ColumnNameForAssociation + "=" + this.allorsObject.Strategy.ObjectId + "\n");
                statement.Append("AND " + Mapping.ColumnNameForRole + "=" + alias + "." + Mapping.ColumnNameForObject + "\n");
                statement.Append(")");
            }
            else
            {
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " = " + this.allorsObject.Strategy.ObjectId);
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}