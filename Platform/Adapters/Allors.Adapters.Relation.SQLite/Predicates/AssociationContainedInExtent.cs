//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateAssociationInExtentSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsPredicateAssociationInExtentSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;
using Allors.Meta;

namespace Allors.Adapters.Relation.SQLite
{
    using Adapters;

    internal sealed class AllorsPredicateAssociationInExtentSql : AllorsPredicateSql
    {
        private readonly IAssociationType association;
        private readonly AllorsExtentSql inExtent;

        internal AllorsPredicateAssociationInExtentSql(AllorsExtentFilteredSql extent, IAssociationType association, Extent inExtent)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContainedIn(association, inExtent);
            this.association = association;
            this.inExtent = (AllorsExtentSql) inExtent;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            AllorsExtentStatementSql inStatement = statement.CreateChild(inExtent, association);
            inStatement.UseRole(association.RoleType);

            statement.Append(" (" + association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
            statement.Append(" " + association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IN (\n");
            inExtent.BuildSql(inStatement);
            statement.Append(" ))\n");
            return Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseAssociation(association);
        }
    }
}