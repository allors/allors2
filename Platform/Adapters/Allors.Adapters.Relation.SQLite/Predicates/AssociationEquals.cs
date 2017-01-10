//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateAssociationEqualsSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateAssociationEqualsSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateAssociationEqualsSql : AllorsPredicateSql
    {
        private readonly IObject allorsObject;
        private readonly IAssociationType association;

        internal AllorsPredicateAssociationEqualsSql(AllorsExtentFilteredSql extent, IAssociationType association, IObject allorsObject)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationEquals(association, allorsObject);
            this.association = association;
            this.allorsObject = allorsObject;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL AND ");
            statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + "=" + this.allorsObject.Strategy.ObjectId + ")");
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}