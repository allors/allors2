// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationExists.cs" company="Allors bvba">
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

    public sealed class AssociationExists : Predicate
    {
        private readonly IAssociationType association;

        public AssociationExists(ExtentFiltered extent, IAssociationType association)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.ValidateAssociationExists(association);
            this.association = association;
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Schema;
            if ((this.association.IsMany && this.association.RelationType.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveClasses)
            {
                statement.Append(" " + this.association.SingularFullName + "_A." + schema.AssociationId.StatementName + " IS NOT NULL");
            }
            else
            {
                if (this.association.RelationType.RoleType.IsMany)
                {
                    statement.Append(" " + alias + "." + schema.Column(this.association) + " IS NOT NULL");
                }
                else
                {
                    statement.Append(" " + this.association.SingularFullName + "_A." + schema.ObjectId + " IS NOT NULL");
                }
            }

            return this.Include;
        }

        public override void Setup(ExtentStatement statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}