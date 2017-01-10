// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationContainedInEnumerable.cs" company="Allors bvba">
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

using Allors;
using Allors.Meta;

namespace Allors.Adapters.Relation.SQLite
{
    using System.Collections.Generic;
    using System.Text;

    using Adapters;

    using Meta;

    internal sealed class AssociationContainedInEnumerable : AllorsPredicateSql
    {
        private readonly IAssociationType association;
        private readonly IEnumerable<IObject> enumerable;

        public AssociationContainedInEnumerable(AllorsExtentFilteredSql extent, IAssociationType association, IEnumerable<IObject> enumerable)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContainedIn(association, this.enumerable);
            this.association = association;
            this.enumerable = enumerable;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            var inStatement = new StringBuilder("0");
            foreach (var inObject in this.enumerable)
            {
                inStatement.Append(",");
                inStatement.Append(inObject.Id);
            }

            statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL AND ");
            statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IN (\n");
            statement.Append(inStatement.ToString());
            statement.Append(" ))\n");

            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}