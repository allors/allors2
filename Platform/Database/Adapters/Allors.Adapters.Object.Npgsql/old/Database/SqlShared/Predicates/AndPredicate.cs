// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndPredicate.cs" company="Allors bvba">
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
    public sealed class AndPredicate : CompositePredicate
    {
        public AndPredicate(ExtentFiltered extent) : base(extent)
        {
        }

        public override bool BuildWhere(ExtentStatement statement, string alias)
        {
            if (this.Include)
            {
                var root = this.Extent.Filter == null || this.Extent.Filter.Equals(this);
                if (root)
                {
                    var wherePresent = !this.Extent.ObjectType.ExistExclusiveLeafClass;
                    statement.Append(wherePresent ? " AND " : " WHERE ");
                }
                else
                {
                    statement.Append("(");
                }

                var atLeastOneChildIncluded = false;
                foreach (var filter in this.Filters)
                {
                    if (atLeastOneChildIncluded)
                    {
                        statement.Append(" AND ");
                    }

                    if (filter.BuildWhere(statement, alias))
                    {
                        atLeastOneChildIncluded = true;
                    }
                }

                if (!root)
                {
                    statement.Append(")");
                }

                return atLeastOneChildIncluded;
            }

            return false;
        }
    }
}