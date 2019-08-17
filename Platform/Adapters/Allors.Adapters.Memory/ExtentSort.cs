// <copyright file="ExtentSort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
