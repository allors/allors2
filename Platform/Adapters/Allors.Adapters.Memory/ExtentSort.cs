// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentSort.cs" company="Allors bvba">
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

using Allors;

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using Allors.Meta;

    public sealed class ExtentSort : IComparer<Strategy>
    {
        private readonly SortDirection direction;
        private readonly IRoleType roleType;
        private ExtentSort subSorter;

        internal ExtentSort(IRoleType roleType, SortDirection direction)
        {
            this.roleType = roleType;
            this.direction = direction;
        }

        public int Compare(Strategy thisStrategy, Strategy thatStrategy)
        {
            var thisValue = thisStrategy.GetInternalizedUnitRole(this.roleType) as IComparable;
            var thatValue = thatStrategy.GetInternalizedUnitRole(this.roleType) as IComparable;

            if (thisValue == null || thatValue == null)
            {
                // Ascending
                if (this.direction == SortDirection.Ascending)
                {
                    if (thisValue == null)
                    {
                        return 1;
                    }

                    return -1;
                }

                // Descending
                if (thisValue == null)
                {
                    return -1;
                }

                return 1;
            }

            // Ascending
            if (this.direction == SortDirection.Ascending)
            {
                var thisResult = thisValue.CompareTo(thatValue);
                if (thisResult == 0 && this.subSorter != null)
                {
                    return this.subSorter.Compare(thisStrategy, thatStrategy);
                }

                return thisResult;
            }

            // Descending
            var thatResult = thatValue.CompareTo(thisValue);
            if (thatResult == 0 && this.subSorter != null)
            {
                return this.subSorter.Compare(thisStrategy, thatStrategy);
            }

            return thatResult;
        }

        internal void AddSort(IRoleType subSortRoleType, SortDirection subSortDirection)
        {
            if (this.subSorter == null)
            {
                this.subSorter = new ExtentSort(subSortRoleType, subSortDirection);
            }
            else
            {
                this.subSorter.AddSort(subSortRoleType, subSortDirection);
            }
        }

        internal void CopyToConnected(Allors.Extent connectedExtent)
        {
            connectedExtent.AddSort(this.roleType, this.direction);
            if (this.subSorter != null)
            {
                this.subSorter.CopyToConnected(connectedExtent);
            }
        }
    }
}