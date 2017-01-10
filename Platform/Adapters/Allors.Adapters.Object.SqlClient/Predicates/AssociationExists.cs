// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationExists.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.SqlClient
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationExists : Predicate
    {
        private readonly IAssociationType association;

        internal AssociationExists(ExtentFiltered extent, IAssociationType association)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.ValidateAssociationExists(association);
            this.association = association;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if ((this.association.IsMany && this.association.RelationType.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveClasses)
            {
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL");
            }
            else
            {
                if (this.association.RelationType.RoleType.IsMany)
                {
                    statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NOT NULL");
                }
                else
                {
                    statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NOT NULL");
                }
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}