// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndPredicate.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.Npgsql
{
    internal sealed class AndPredicate : CompositePredicate
    {
        internal AndPredicate(ExtentFiltered extent) : base(extent)
        {
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            if (this.Include)
            {
                var root = this.Extent.Filter == null || this.Extent.Filter.Equals(this);
                if (root)
                {
                    var wherePresent = !this.Extent.ObjectType.ExistExclusiveClass;
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
